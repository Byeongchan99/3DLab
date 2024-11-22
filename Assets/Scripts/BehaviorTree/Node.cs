using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    [System.Serializable]
    public abstract class Node
    {
        /* 노드의 상태를 반환하는 델리게이트 */
        public delegate NodeStates NodeReturn();

        /* 노드의 현재 상태 */
        protected NodeStates m_nodeState;

        public NodeStates nodeState
        {
            get { return m_nodeState; }
        }

        /* 노드를 위한 생성자 */
        public Node() { }

        /* 원하는 조건 세트를 평가하기 위해 이 메소드를 구현 */
        public abstract NodeStates Evaluate();
    }

    /* 노드의 상태 */
    public enum NodeStates
    {
        FAILURE,
        SUCCESS,
        RUNNING,
        ERROR
    }
}
