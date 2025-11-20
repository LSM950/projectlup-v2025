using UnityEngine;

namespace LUP.PCR
{
    public class StartNewTask : WorkerBlackboardNode
    {
        public StartNewTask(WorkerBlackboard blackboard) : base(blackboard) { }
        float timer = 0f;
        float duration = 3f;


        public override NodeState Evaluate()
        {
            bool hasNewTask = GetData<bool>(BBKeys.HasNewTask);

            if(!hasNewTask) { return NodeState.FAILURE; }

            if (timer < duration)
            {
                timer += Time.deltaTime;
                Debug.Log($"새 작업 수행 중... {timer:F1}/{duration}");

                return NodeState.RUNNING;
            }
            
            hasNewTask = false;
            Debug.Log("새 작업 완료!");
            timer = 0f;

            return NodeState.SUCCESS;
        }
    }
}

