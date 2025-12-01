using UnityEngine;

namespace LUP.PCR
{ 
    public class IsNewTaskChecker : WorkerBlackboardNode
    {
        public IsNewTaskChecker(WorkerBlackboard blackboard) : base(blackboard) { }
        protected override NodeState OnUpdate()
        {
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);
            Vector2Int entrancePos = GetData<Vector2Int>(BBKeys.TargetPosition);

            return building != null && entrancePos != null && OwnerAI != null 
                && OwnerAI.HasNewTask ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}