using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Selector : Node
    {
        /* 이 셀렉터를 위한 자식 노드들 */
        protected List<Node> m_nodes = new List<Node>();

        /* 생성자는 자식 노드의 목록을 필요로 한다 */
        public Selector(List<Node> nodes)
        {
            m_nodes = nodes;
        }

        /* 자식 중 하나가 성공을 보고하면 셀렉터는 즉시 상위로 성공을 보고한다.
         * 만약 모든 자식이 실패하면 실패를 보고한다. */
        public override NodeStates Evaluate()
        {
            foreach (Node node in m_nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.FAILURE:
                        continue;
                    case NodeStates.SUCCESS:
                        m_nodeState = NodeStates.SUCCESS;
                        return m_nodeState;
                    case NodeStates.RUNNING:
                        m_nodeState = NodeStates.RUNNING;
                        return m_nodeState;
                    default:
                        continue;
                }
            }
            m_nodeState = NodeStates.FAILURE;
            return m_nodeState;
        }
    }
}
