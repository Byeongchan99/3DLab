using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class ActionNode : Node
    {
        /* �׼ǿ� ���� �޼ҵ� �ñ״�ó */
        public delegate NodeStates ActionNodeDelegate();

        /* �� ��带 ���� �� ȣ���ϴ� ��������Ʈ */
        private ActionNodeDelegate m_action;

        /* �� ���� �ƹ��� ������ �������� �����Ƿ� ��������Ʈ ���·� ������ ���޵ž� �Ѵ�. 
        * �ñ״�ó�� ���� �ֵ��� �׼��� NodeStates �������� ��ȯ�ؾ� �Ѵ�. */
        public ActionNode(ActionNodeDelegate action)
        {
            m_action = action;
        }

        /* ���޵� ��������Ʈ�� ��带 ���ϰ� �׿� �´� ���¸� �����Ѵ�. */
        public override NodeStates Evaluate()
        {
            switch (m_action())
            {
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
               
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    return m_nodeState;
                default:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
            }
        }
    }
}
