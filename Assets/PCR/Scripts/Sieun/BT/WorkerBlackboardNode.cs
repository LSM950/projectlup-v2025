using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LUP.PCR
{
    // 1. 이 클래스가 생성될 때 Blackboard를 받음
    // 2. : base() 를 통해 부모(BTNode)의 생성자를 먼저 실행함
    public abstract class WorkerBlackboardNode : BTNode
    {
        protected WorkerBlackboard Blackboard { get; private set; }

        
        public WorkerBlackboardNode(WorkerBlackboard blackboard) : base() // 기존 BTNode 생성자 호출
        {
            this.Blackboard = blackboard;
        }

        // 자식 노드들이 편하게 쓰라고 만든 헬퍼 함수들
        // (굳이 Blackboard.GetValue 하지 않고 Node.GetData 로 짧게 쓰기 위함)
        protected void SetData<T>(string key, T value)
        {
            Blackboard.SetValue(key, value);
        }

        protected T GetData<T>(string key)
        {
            return Blackboard.GetValue<T>(key);
        }

        protected bool HasData(string key) => Blackboard.HasKey(key);
    }
}

