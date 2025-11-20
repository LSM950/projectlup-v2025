using UnityEngine;

namespace LUP.PCR
{
    public class GoBackToOriginPosition : WorkerBlackboardNode
    {
        public GoBackToOriginPosition(WorkerBlackboard blackboard) : base(blackboard) { }
        bool arrived = false;
        public override NodeState Evaluate()
        {
            {
            if (!arrived)
                Debug.Log("원래 자리로 복귀 중...");
                //@TODO : AStar 의 Move()로 변경
                //MoveTo(originSpot);
  
                //if (!IsAt(originSpot))
                    return NodeState.RUNNING;
  
                arrived = true;
                Debug.Log("원래 자리 복귀 완료.");
            }
            return NodeState.SUCCESS;
        }
    }
}