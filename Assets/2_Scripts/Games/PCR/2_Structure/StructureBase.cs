using UnityEngine;

namespace LUP.PCR
{
    public abstract class StructureBase : MonoBehaviour
    {
        public string placeName;
        public Vector2Int entrancePos;

        protected bool hasWork;
        public bool IsWorkRequested { get; protected set; } = false; // 작업 요청 여부
        public WorkerAI AssignedWorker { get; protected set; } // 배정된 작업자
        // @TODO : AssignedWorker를 안 쓰는 게 더 나은지 고민해보기


        public void ToggleWorkRequest()
        {
            IsWorkRequested = !IsWorkRequested;

            if (IsWorkRequested)
            {
                if (WorkerSystem.Instance != null)
                {
                    WorkerSystem.Instance.RegisterTask(this);
                }
            }
            else
            {
                // 만약 이미 배정된 작업자가 있다면 ReleaseWorker()를 호출할 수도 있음
            }
        }
        public virtual void SetWorker(WorkerAI worker)
        {
            this.AssignedWorker = worker;
        }

        // 작업자 해제 (내보내거나 떠날 때)
        public virtual void ReleaseWorker()
        {
            if (AssignedWorker != null)
            {
                AssignedWorker.HasTask = false;
                AssignedWorker = null;
            }

            ExitWorker();

            if (IsWorkRequested && WorkerSystem.Instance != null)
            {
                WorkerSystem.Instance.RegisterTask(this);
            }
            else
            {
                // 예약목록에서 해제 (작업요청 토글 Off)
            }
        }

        public void EnterWorker()
        {
            hasWork = true;
        }

        public void ExitWorker()
        {
            hasWork = false;
        }
        public bool HasWorker() => AssignedWorker != null;
    }
}