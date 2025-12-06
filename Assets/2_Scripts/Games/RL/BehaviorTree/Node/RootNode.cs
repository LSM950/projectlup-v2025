using UnityEngine;

namespace LUP.RL
{
    public class RootNode : Node
    {
        [HideInInspector]
        public Node topChildNode;
        public RootNode(Node singleChildNode)
        {
            topChildNode = singleChildNode;
        }
        public override NodeState Evaluate()
        {
            return topChildNode.Evaluate();
        }
    }
}

