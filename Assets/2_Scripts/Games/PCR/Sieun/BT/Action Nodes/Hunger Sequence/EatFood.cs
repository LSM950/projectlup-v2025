using UnityEngine;

namespace LUP.PCR
{
    public class EatFood : WorkerBlackboardNode
    {
        public EatFood(WorkerBlackboard blackboard) : base(blackboard) { }
        float timer = 0f;
        float duration = 1f;

        protected override NodeState OnUpdate()
        {
            float currentHunger = GetData<float>(BBKeys.Hunger);
            
            if (timer < duration)
            {
                timer += Time.deltaTime;
                Debug.Log($"1-4. 식사 중... {timer:F1}/{duration}");
                return NodeState.RUNNING;
            }
            else
            {
                currentHunger = 0f;
                SetData<float>(BBKeys.Hunger, currentHunger);

                Debug.Log("1-4. 식사 완료!");
                return NodeState.SUCCESS;
            }
        }
    }

}
