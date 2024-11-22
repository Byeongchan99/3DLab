using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Inverter : Node
    {
        /* ���� �ڽ� ��� */
        private Node m_node;

        public Node node
        {
            get { return m_node; }
        }

        /* �����ڴ� �� �ι��� ���ڷ����Ͱ� ���� �ڽ� ��带 �ʿ�� �Ѵ�. */
        public Inverter(Node node)
        {
            m_node = node;
        }

        /* �ڽ��� �����ϸ� ������ �����ϰ� �ڽ��� �����ϸ� ���и� �����Ѵ�. 
        * RUNNING�� �״�� �����Ѵ�. */
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
