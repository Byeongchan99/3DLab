using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class ActionNode : Node
    {
        /* 액션에 대한 메소드 시그니처 */
        public delegate NodeStates ActionNodeDelegate();

        /* 이 노드를 평가할 때 호출하는 델리게이트 */
        private ActionNodeDelegate m_action;

        /* 이 노드는 아무런 로직을 포함하지 않으므로 델리게이트 형태로 로직이 전달돼야 한다. 
        * 시그니처에 나와 있듯이 액션은 NodeStates 열거형을 반환해야 한다. */
        public ActionNode(ActionNodeDelegate action)
        {
            m_action = action;
        }

        /* 전달된 델리게이트로 노드를 평가하고 그에 맞는 상태를 보고한다. */
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
