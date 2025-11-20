using UnityEngine;

namespace LUP.PCR
{
    public class IsPausedTaskChecker : WorkerBlackboardNode
    {
        public IsPausedTaskChecker(WorkerBlackboard blackboard) : base(blackboard) { }

        public override NodeState Evaluate()
        {
            bool hasPausedWork = GetData<bool>(BBKeys.HasPausedTask);

            Debug.Log("중단된 작업 존재 여부 검사...");
            return hasPausedWork ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}
