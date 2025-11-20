using UnityEngine;

namespace LUP.PCR
{
    public class IsHealthLowChecker : WorkerBlackboardNode
    {
        public IsHealthLowChecker(WorkerBlackboard blackboard) : base(blackboard) { }

        public WorkerStatus Status { get; private set; } = new WorkerStatus();
        
        int logLoopCount = -1;

        public override NodeState Evaluate()
        {
            float currentHunger = GetData<float>(BBKeys.Hunger);

            //@TODO : currentHunger 기준 배고프지 않은 상태면 FAILURE
            if (currentHunger >= 0.7f)
            {
                Debug.Log("배고픔 감지됨.");
                return NodeState.SUCCESS;
            }

            if(logLoopCount == 0)
            {
                Debug.Log("아직 배고프지 않음.");
                logLoopCount += 1;
            }
            return NodeState.FAILURE;
        }
    }


}

