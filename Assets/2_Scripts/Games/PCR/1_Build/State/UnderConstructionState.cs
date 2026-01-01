using UnityEngine;

namespace LUP.PCR
{
    public class UnderConstructionState : IBuildState
    {
        public float totalTime;         // 총 건설 시간
        public float progressRatio;     // 진행률 (누적 진행 시간 / 총 건설 시간)
        public bool isCompledted;       // 완료 여부
        public bool isStarted;          // 생산 시작 여부

        private BuildingBase building;
        private ConstructionInfo currentConstructionInfo;

        public void Enter(BuildingBase building)
        {
            Debug.Log("UnderContructionState Enter");

            this.building = building;

            // 건설중 UI 활성화
            if (building.ConstructScreen)
            {
                building.ConstructScreen.SetActive(true);
            }

            if (building.constructionOverlay != null)
            {
                building.constructionOverlay.Show();
            }

            currentConstructionInfo = building.GetConstructionInfo();
            building.GetBuildingInfo().isConstructing = true;

            Start();
        }
        public void Exit()
        {
            if (building.ConstructScreen)
            {
                building.ConstructScreen.SetActive(false);
            }

            if (building.constructionOverlay != null)
            {
                building.constructionOverlay.Hide();
            }

            Stop();
            // 건설 취소.
            Debug.Log("UnderContructionState Exit");
        }
        public void Tick(float deltaTime)
        {
            if (!isStarted)
            {
                return;
            }
            if (isCompledted)
            {
                return;
            }

            currentConstructionInfo.elapsedTime += deltaTime;
            progressRatio = Mathf.Clamp01(currentConstructionInfo.elapsedTime / totalTime);

            if (building.constructionOverlay != null)
            {
                float remainingTime = Mathf.Max(0, totalTime - currentConstructionInfo.elapsedTime);
                building.constructionOverlay.UpdateView(progressRatio, remainingTime);
            }

            if (progressRatio >= 1f)
            {
                isCompledted = true;
            }

            if (isCompledted)
            {
                building.CompleteContruction();
            }
        }

        public void Reset()
        {
            totalTime = building.currentConstructionData.constructionTime;
            progressRatio = 0f;
            isCompledted = false;
            isStarted = false;
        }

        public void Start()
        {
            Reset();
            isStarted = true;
            isCompledted = false;
        }

        public void Stop()
        {
            Reset();
            currentConstructionInfo.elapsedTime = 0f;
        }
    }
}