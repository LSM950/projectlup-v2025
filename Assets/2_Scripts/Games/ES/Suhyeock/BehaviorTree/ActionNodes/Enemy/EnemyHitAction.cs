using UnityEngine;

namespace LUP.ES
{
    public class EnemyHitAction : BTNode
    {
        EnemyBlackboard blackboard;

        public EnemyHitAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.healthComponent.isHit = false;
            blackboard.enemyHPUI.UIInstance.SetActive(true);
            blackboard.enemyHPUI.UpdateHPUI();
            return NodeState.Success;
        }

        public override void Reset()
        {

        }
    }
}

