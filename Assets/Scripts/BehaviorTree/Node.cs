using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    [System.Serializable]
    public abstract class Node
    {
        /* ����� ���¸� ��ȯ�ϴ� ��������Ʈ */
        public delegate NodeStates NodeReturn();

        /* ����� ���� ���� */
        protected NodeStates m_nodeState;

        public NodeStates nodeState
        {
            get { return m_nodeState; }
        }

        /* ��带 ���� ������ */
        public Node() { }

        /* ���ϴ� ���� ��Ʈ�� ���ϱ� ���� �� �޼ҵ带 ���� */
        public abstract NodeStates Evaluate();
    }

    /* ����� ���� */
    public enum NodeStates
    {
        FAILURE,
        SUCCESS,
        RUNNING,
        ERROR
    }
}
