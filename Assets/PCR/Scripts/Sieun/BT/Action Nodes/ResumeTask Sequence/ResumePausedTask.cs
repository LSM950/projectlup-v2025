using UnityEngine;

namespace LUP.PCR
{
    public class ResumePausedTask : WorkerBlackboardNode
    {
        public ResumePausedTask(WorkerBlackboard blackboard) : base(blackboard) { }

        float timer = 0f;
        float duration = 2f;

        public override NodeState Evaluate()
        {
            bool hasPausedTask = GetData<bool>(BBKeys.HasPausedTask);

            if (timer < duration)
            {
                timer += Time.deltaTime;
                Debug.Log($"작업 재개 중... {timer:F1}/{duration}");
                return NodeState.RUNNING;
            }

            hasPausedTask = false;
            Debug.Log("작업 재개 완료!");
            timer = 0f;

            return NodeState.SUCCESS;
        }
    }
}

