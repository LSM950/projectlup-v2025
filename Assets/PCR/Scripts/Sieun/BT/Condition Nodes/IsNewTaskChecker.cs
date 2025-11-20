using UnityEngine;

namespace LUP.PCR
{ 
    public class IsNewTaskChecker : WorkerBlackboardNode
    {
        public IsNewTaskChecker(WorkerBlackboard blackboard) : base(blackboard) { }
        public override NodeState Evaluate()
        {
            bool hasNewTask = GetData<bool>(BBKeys.HasNewTask);

            Debug.Log("새 작업 명령 존재 여부 검사...");
            return hasNewTask ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}