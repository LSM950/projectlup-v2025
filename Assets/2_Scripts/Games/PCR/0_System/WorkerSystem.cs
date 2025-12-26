using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class WorkerSystem : MonoBehaviour
    {
        private ProductionRuntimeData pcrRuntimeData;

        private List<int> curReservedBuildingIdList;
        private List<int> curAssignedBuildingIdList;
        private List<WorkerInfo> curWorkerInfoList;
        private Dictionary<int, BuildingBase> curBuildings; // 건물 Id로 BuildingBase 읽기전용
        private TileMap tileMap;

        private BuildingBase restaurant;
        private BuildingBase workStation;
        private List<BuildingBase> workStationList;

        [SerializeField] private GameObject workerPrefab;
        
        private AGridMap aGrid;
        private int maxWorkerCount = 50;
        private List<WorkerAI> activeWorkers;
        private bool isInitialized = false;
        private Queue<StructureBase> taskQueue = new Queue<StructureBase>();

        public void InitWorkerSystem(BuildingSystem buildingSystem, TileMap tileMap)
        {
            aGrid = GetComponentInChildren<AGridMap>();

            activeWorkers = new List<WorkerAI>(maxWorkerCount);
            workStationList = new List<BuildingBase>();

            // 타일맵 & 그리드맵 초기화
            this.tileMap = tileMap;
            aGrid.InitMap(tileMap.tiles);
            
            // 건물목록 초기화
            curBuildings = buildingSystem.GetCurrentBuildingDictionary();

            //if (curBuildings[0] is BuildingWorkStation)
            //{
            //    workStation = curBuildings[0];
            //}
            //else
            //{
            //    Debug.Log("WorkStation is empty!");
            //}

            // Restaurant를 buildingId: 1로 배정 예정. 추후 바뀔 수 있음.
            if (curBuildings[1] is BuildingRestaurant)
            {
                restaurant = curBuildings[1];
            }
            else
            {
                Debug.Log("Restaurant is empty!");
            }




            foreach (BuildingBase building in curBuildings.Values)
            {
                if (building is BuildingWorkStation)
                {
                    workStationList.Add(building);
                }
            }

            ProductionStage stage = StageManager.Instance.GetCurrentStage() as ProductionStage;
            pcrRuntimeData = stage.productionRuntimeData;
            curReservedBuildingIdList = pcrRuntimeData.ReservedBuildingIdList;
            curAssignedBuildingIdList = pcrRuntimeData.AssignedBuildingIdList;
            curWorkerInfoList = pcrRuntimeData.WorkerInfoList;

            // 위 데이터 기반으로 초기화.
           curWorkerInfoList.Clear();
           activeWorkers.Clear();

            InitDefaultWorkers();

            isInitialized = true;
        }
        
        private void InitDefaultWorkers()
        {
            // ID 0~4 까지는 기본 제공되는 작업자.
            int defaultWorkerCount = 5;
            if (curWorkerInfoList.Count == 0)
            {
                for (int i = 0; i < defaultWorkerCount; i++)
                {
                    WorkerInfo testInfo = new WorkerInfo();
                    testInfo.id = i;
                    testInfo.name = $"DefaultWorker_{i}";

                    curWorkerInfoList.Add(testInfo);
                    CreateWorkerObject(testInfo);
                }
            }
        }

        private void CreateWorkerObject(WorkerInfo info)
        {
            //GameObject newWorker = Instantiate(workerPrefab, (Vector2)workStationList[0].entrancePos, Quaternion.identity);
            Vector3 defaultPos = aGrid.GridToWorldPosition(restaurant.entrancePos);
            GameObject newWorker = Instantiate(workerPrefab, defaultPos, Quaternion.identity);

            WorkerAI ai = newWorker.GetComponent<WorkerAI>();
            if (ai == null) ai = ai.GetComponentInChildren<WorkerAI>();

            if (!activeWorkers.Contains(ai))
            {
                activeWorkers.Add(ai);
            }

            BuildingBase defaultStation = workStationList.Count > 0 ? workStationList[0] : null;
            ai.Initialize(info, restaurant, defaultStation);
        }


        private void Update()
        {
            if (!isInitialized || activeWorkers == null) return;

            int count = activeWorkers.Count;

            for (int i = 0; i < count; i++)
            {
                if (activeWorkers[i] != null)
                {
                    if (i >= activeWorkers.Count)
                    {
                        break;
                    }

                    activeWorkers[i].UpdateBT();
                }

            }

            AssignPendingTasks();
        }

        // 대기중인 일감을 노는 일꾼에게 배정
        private void AssignPendingTasks()
        {
            if (taskQueue.Count == 0) return;

            // 노는 일꾼 찾기
            List<WorkerAI> idleWorkers = GetIdleWorkers();
            if (idleWorkers.Count == 0) return;

            // 큐에서 일감 꺼내서 배정
            while (taskQueue.Count > 0 && idleWorkers.Count > 0)
            {
                StructureBase targetStructure = taskQueue.Peek();

                // 해당 장소로 갈 수 있는 가장 가까운 일꾼 찾기
                WorkerAI bestWorker = GetBestInIdleWorkers(idleWorkers, targetStructure);

                if (bestWorker != null)
                {
                    taskQueue.Dequeue(); // 큐에서 제거
                    bestWorker.AssignTask(targetStructure); // 작업 할당
                    idleWorkers.Remove(bestWorker); // 목록에서 제외
                }
                else
                {
                    // 지금 갈 수 있는 일꾼이 없다면 다음 기회에
                    break;
                }
            }
        }

        public void RegisterTask(StructureBase structure)
        {
            if (!taskQueue.Contains(structure))
            {
                taskQueue.Enqueue(structure);
            }
        }

        public List<WorkerAI> GetIdleWorkers()
        {
            List<WorkerAI> idleList = new List<WorkerAI>();

            for (int i = 0; i < activeWorkers.Count; i++)
            {
                WorkerAI w = activeWorkers[i];
                
                // 작업자가 존재하고, 예약된 작업이 없을 때만 추가
                if (w != null && !w.HasTask)
                {
                    idleList.Add(w);
                }
            }

            return idleList;
        }
        private WorkerAI GetBestInIdleWorkers(List<WorkerAI> candidates, StructureBase structure)
        {
            if (candidates == null || candidates.Count == 0) return null;

            ANode targetNode = aGrid.GetNodeFromWorldPosition(structure.transform.position);

            if (targetNode == null || !targetNode.isWalkable) return null;

            WorkerAI bestWorker = null;
            float minScore = float.MaxValue;
            float tolerance = 0.1f;

            foreach (var w in candidates)
            {
                if (w == null) continue; // 이미 idle 상태인 것만 넘겨받았으므로 HasTask 체크 불필요

                ANode workerNode = aGrid.GetNodeFromWorldPosition(w.transform.position);
                if (workerNode == null) continue;

                // 맨해튼 거리 계산
                int dx = Mathf.Abs(workerNode.indexX - targetNode.indexX);
                int dy = Mathf.Abs(workerNode.indexY - targetNode.indexY);
                float distScore = dx + dy;

                if (distScore < minScore - tolerance)
                {
                    minScore = distScore;
                    bestWorker = w;
                }
                else if (Mathf.Abs(distScore - minScore) <= tolerance)
                {
                    // 거리가 비슷하면 ID가 낮은 순 (일관성 유지)
                    if (bestWorker != null && w.GetInstanceID() < bestWorker.GetInstanceID())
                    {
                        bestWorker = w;
                    }
                    else if (bestWorker == null)
                    {
                        bestWorker = w;
                    }
                }
            }

            return bestWorker;
        }
        

    }
}

/*

        //// [새로운 워커 고용] 게임 도중 버튼을 눌러 추가할 때 사용
        //public void HireNewWorker(string name)
        //{
        //    int newId = curWorkerInfoList.Count + 1; // ID 생성
        //    //WorkerInfo newInfo = new WorkerInfo { workerId = newId, workerName = name };
        //    //curWorkerInfoList.Add(newInfo);
        //    //CreateWorkerObject(newInfo);
        //}

        //// [워커 삭제]
        //public void RemoveWorker(WorkerAI worker)
        //{
        //    if (activeWorkers.Contains(worker))
        //    {
        //        activeWorkers.Remove(worker);
        //        // curWorkerInfoList.Remove(...) 
        //        Destroy(worker.gameObject);
        //    }
        //}


 */