using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Inverter : Node
    {
        /* 평가할 자식 노드 */
        private Node m_node;

        public Node node
        {
            get { return m_node; }
        }

        /* 생성자는 이 인버터 데코레이터가 감쌀 자식 노드를 필요로 한다. */
        public Inverter(Node node)
        {
            m_node = node;
        }

        /* 자식이 실패하면 성공을 보고하고 자식이 성공하면 실패를 보고한다. 
        * RUNNING은 그대로 보고한다. */
        public override NodeStates Evaluate()
        {
            switch (m_node.Evaluate())
            {
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    return m_nodeState;
            }
            m_nodeState = NodeStates.SUCCESS;
            return m_nodeState;
        }

    }
}
