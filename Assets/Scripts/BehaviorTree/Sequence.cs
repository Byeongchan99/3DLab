using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequence : Node

    {
        /* �� �������� ���� �ڽ� ���� */
        private List<Node> m_nodes = new List<Node>();

        /* �ʱ� �ڽ� ����� �ݵ�� �����ؾ� �Ѵ�. */
        public Sequence(List<Node> nodes)
        {
            m_nodes = nodes;
        }

        /* �ϳ��� �ڽ� ���� ���и� ��ȯ�ϸ� ��ü ���� �����Ѵ�.
         * ��� ��尡 ������ ��ȯ�ϸ� ���� ������ �����Ѵ�. */
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
