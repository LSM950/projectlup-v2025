using UnityEngine;

namespace LUP.PCR
{
    public class PerformTask : WorkerBlackboardNode
    {
        public PerformTask(WorkerBlackboard bb) : base(bb) { }
        protected override NodeState OnUpdate()
        {
            StructureBase workingPlace = GetData<StructureBase>(BBKeys.AssignedWorkplace);
            
            if (workingPlace == null)
            {
                ClearWorkData();
                return NodeState.FAILURE;
            }

            // 만약 건물이 파괴되거나 작업 취소(IsWorkRequested = false) 되면 FAILURE 반환 -> 작업 중단
            if (!workingPlace.IsWorkRequested)
            {
                workingPlace.ExitWorker();
                ClearWorkData();
                return NodeState.FAILURE;
            }

            if (!OwnerAI.HasTask)
            {
                OwnerAI.HasTask = true;
            }

            return NodeState.RUNNING;
        }

        private void ClearWorkData()
        {
            BB.Remove(BBKeys.AssignedWorkplace);
            OwnerAI.HasTask = false;
        }
    }
}