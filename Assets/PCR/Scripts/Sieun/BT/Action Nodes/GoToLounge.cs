using UnityEngine;

namespace LUP.PCR
{
    public class GoToLounge : WorkerBlackboardNode
    {
        public GoToLounge(WorkerBlackboard blackboard) : base(blackboard) { }
        bool arrived = false;

        public override NodeState Evaluate()
        {
            if (!arrived)
            {
                Debug.Log("라운지로 이동 중...");
                //worker.MoveTo(worker.loungeSpot);

                //if (!worker.IsAt(worker.loungeSpot))
                //    return NodeState.RUNNING;

                arrived = true;
                Debug.Log("라운지 도착. 휴식 중...");
            }

            return NodeState.RUNNING; // 계속 대기
        }
    }


}
