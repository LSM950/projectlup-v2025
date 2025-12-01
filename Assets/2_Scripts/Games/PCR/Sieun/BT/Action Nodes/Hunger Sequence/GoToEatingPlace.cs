using UnityEngine;

namespace LUP.PCR
{
    public class GoToEatingPlace : WorkerBlackboardNode
    {
        private UnitMover mover;
        private Vector2Int eatingPlace;

        public GoToEatingPlace(WorkerBlackboard blackboard) : base(blackboard) { }

        protected override void OnStart()
        {
            mover = GetData<UnitMover>(BBKeys.UnitMover);

            if (HasData(BBKeys.Restaurant))
            {
                BuildingBase restaurantBuilding = GetData<BuildingBase>(BBKeys.Restaurant);
                SetData<Vector2Int>(BBKeys.TargetPosition, restaurantBuilding.entrancePos);

                eatingPlace = GetData<Vector2Int>(BBKeys.TargetPosition);

                if (eatingPlace == null)
                {
                    Debug.Log("1-3. 식당이 없습니다.");
                }
                if (mover != null)
                {
                    mover.SetDestination(eatingPlace);
                }
            }
        }
        protected override NodeState OnUpdate()
        {
            if (mover == null || eatingPlace == null) { return NodeState.FAILURE; }

            if (mover.IsArrived())
            {
                Debug.Log("1-3. 식당 도착!");
                return NodeState.SUCCESS;
            }
            else
            {
                mover.MoveAlongPath();
                Debug.Log("1-3. 식당으로 이동 중...");
                return NodeState.RUNNING;
            }
        }
    }

}
