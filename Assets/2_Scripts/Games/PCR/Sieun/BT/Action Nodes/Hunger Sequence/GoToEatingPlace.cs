using UnityEngine;

namespace LUP.PCR
{
    public class GoToEatingPlace : WorkerBlackboardNode
    {
        private BuildingBase eatingPlace;

        public GoToEatingPlace(WorkerBlackboard blackboard) : base(blackboard) { }

        protected override void OnStart()
        {
            if (HasData(BBKeys.Restaurant))
            {
                eatingPlace = GetData<BuildingBase>(BBKeys.Restaurant);

                if (eatingPlace == null)
                {
                    Debug.Log("1-3. 식당이 없습니다.");
                }
                else if (Mover != null)
                {
                    Mover.SetDestination(eatingPlace.entrancePos);
                    //SetData<Vector2Int>(BBKeys.TargetPosition, restaurantBuilding.entrancePos);
                }
            }
        }
        protected override NodeState OnUpdate()
        {
            if (Mover == null || eatingPlace == null) { return NodeState.FAILURE; }

            if (Mover.IsArrived())
            {
                Debug.Log("1-3. 식당 도착!");
                return NodeState.SUCCESS;
            }
            else
            {
                Mover.MoveAlongPath();
                
                Debug.Log("1-3. 식당으로 이동 중...");
                
                return NodeState.RUNNING;
            }
        }
    }

}
