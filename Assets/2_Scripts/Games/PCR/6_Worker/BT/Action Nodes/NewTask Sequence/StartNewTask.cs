using UnityEngine;

namespace LUP.PCR
{
    public class StartNewTask : WorkerBlackboardNode
    {
        public StartNewTask(WorkerBlackboard bb) : base(bb) { }
        
        protected override NodeState OnUpdate()
        {
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.AssignedWorkplace);
            //OwnerAI.HasTask = GetData<bool>(BBKeys.HasTask);
            
            if (building == null)
            {
                return NodeState.FAILURE;
            }

            building.EnterWorker();

            return NodeState.SUCCESS;
        }
    }
}
//OwnerAI.HasTask = true;
//return NodeState.RUNNING;
//BB.Remove(BBKeys.AssignedWorkplace);
//OwnerAI.HasTask = false;
