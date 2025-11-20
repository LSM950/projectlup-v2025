using UnityEngine;

namespace LUP.PCR
{
    public class GoToPausedTaskLocation : WorkerBlackboardNode
    {
        public GoToPausedTaskLocation(WorkerBlackboard blackboard) : base(blackboard) { }
        bool arrived = false;

        public override NodeState Evaluate()
        {
            if (!arrived)
            {
                Debug.Log("중단된 작업 위치로 이동 중...");
                //worker.MoveTo(worker.pausedWorkSpot);


                //if (!worker.IsAt(worker.pausedWorkSpot))
                //    return NodeState.RUNNING;

                arrived = true;
                Debug.Log("중단된 작업 위치 도착.");
            }
            return NodeState.SUCCESS;
        }
    }

}


