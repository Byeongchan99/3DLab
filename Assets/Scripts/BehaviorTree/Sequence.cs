using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequence : Node

    {
        /* 이 시퀀스에 속한 자식 노드들 */
        private List<Node> m_nodes = new List<Node>();

        /* 초기 자식 목록을 반드시 제공해야 한다. */
        public Sequence(List<Node> nodes)
        {
            m_nodes = nodes;
        }

        /* 하나의 자식 노드라도 실패를 반환하면 전체 노드는 실패한다.
         * 모든 노드가 성공을 반환하면 노드는 성공을 보고한다. */
        public override NodeStates Evaluate()
        {
            bool anyChildRunning = false;
            foreach (Node node in m_nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.FAILURE:
                        m_nodeState = NodeStates.FAILURE;
                        return m_nodeState;
                    case NodeStates.SUCCESS:
                        continue;
                    case NodeStates.RUNNING:
                        anyChildRunning = true;
                        continue;
                    default:
                        m_nodeState = NodeStates.SUCCESS;
                        return m_nodeState;
                }
            }
            m_nodeState = anyChildRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
            return m_nodeState;
        }
    }
}
