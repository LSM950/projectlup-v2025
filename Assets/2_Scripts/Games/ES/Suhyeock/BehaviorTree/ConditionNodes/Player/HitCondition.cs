using UnityEngine;

namespace LUP.ES
{
    public class HitCondition : BTNode
    {
        BaseBlackboard blackboard;

        public HitCondition(BaseBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if (blackboard.healthComponent.isHit == true)
            {
                return NodeState.Success;
            }
            return NodeState.Failure;
        }

        public override void Reset()
        {

        }
    }

}
