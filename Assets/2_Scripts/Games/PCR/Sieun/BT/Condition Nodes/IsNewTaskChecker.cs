using UnityEngine;

namespace LUP.PCR
{ 
    public class IsNewTaskChecker : WorkerBlackboardNode
    {
        public IsNewTaskChecker(WorkerBlackboard blackboard) : base(blackboard) { }
        protected override NodeState OnUpdate()
        {

            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);

            bool hasNewTask = GetData<bool>(BBKeys.HasNewTask);

            return building != null && hasNewTask ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}