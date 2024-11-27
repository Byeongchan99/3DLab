using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MathTree : MonoBehaviour
{
    public Color m_evaluating;
    public Color m_succeeded;
    public Color m_failed;

    public Selector m_rootNode;

    public ActionNode m_node2A;
    public Inverter m_node2B;
    public ActionNode m_node2C;
    public ActionNode m_node3;

    public GameObject m_rootNodeBox;
    public GameObject m_node2aBox;
    public GameObject m_node2bBox;
    public GameObject m_node2cBox;
    public GameObject m_node3Box;

    public int m_targetValue = 20;
    private int m_currentValue = 0;

    [SerializeField]
    private Text m_valueLabel;

    /* ���� �Ʒ��ʺ��� �ν��Ͻ�ȭ�ϰ� ���ʷ� �ڽĵ��� �Ҵ��Ѵ�. */
    void Start()
    {
        /** ���� ���� ������ ���� Node 3���� �ڽ��� ���� �ʴ´�. */
        m_node3 = new ActionNode(NotEqualToTarget);

        /** ��������, ���� 2 ��带 �����Ѵ�. */
        m_node2A = new ActionNode(AddTen);

        /** Node 2B�� �����ͷ� ��� 3�� �ڽ����� �����Ƿ� �����ڿ� �̸� �����Ѵ�. */
        m_node2B = new Inverter(m_node3);
        m_node2C = new ActionNode(AddTen);

        /** �������� ��Ʈ ����, �ϴ� ���⿡ ������ �ڽ� ����� �����. */
        List<Node> rootChildren = new List<Node>();
        rootChildren.Add(m_node2A);
        rootChildren.Add(m_node2B);
        rootChildren.Add(m_node2C);

        /** �׷� �� ��Ʈ ��� ������Ʈ�� ����� ���⿡ ����� �����Ѵ�. */
        m_rootNode = new Selector(rootChildren);

        m_valueLabel.text = m_currentValue.ToString();

        m_rootNode.Evaluate();

        UpdateBoxes();
    }

    private void UpdateBoxes()
    {
        /** ��Ʈ ��� �ڽ� ���� */
        if (m_rootNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_rootNodeBox);
        }
        else if (m_rootNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_rootNodeBox);
        }

        /** 2A ��� �ڽ� ���� */
        if (m_node2A.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2aBox);
        }
        else if (m_node2A.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2aBox);
        }

        /** 2B ��� �ڽ� ���� */
        if (m_node2B.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2bBox);
        }
        else if (m_node2B.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2bBox);
        }

        /** 2C ��� �ڽ� ���� */
        if (m_node2C.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2cBox);
        }
        else if (m_node2C.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2cBox);
        }

        /** 3 ��� �ڽ� ���� */
        if (m_node3.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node3Box);
        }
        else if (m_node3.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node3Box);
        }
    }

    void SetSucceeded(GameObject box)
    {
        // box�� ������ m_succeeded�� ����
        Renderer renderer;

        renderer = box.GetComponent<Renderer>();
        renderer.material.color = m_succeeded;
    }

    void SetFailed(GameObject box)
    {
        // box�� ������ m_failed�� ����
        Renderer renderer;

        renderer = box.GetComponent<Renderer>();
        renderer.material.color = m_failed;
    }

    private NodeStates NotEqualToTarget()
    {
        if (m_currentValue != m_targetValue)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }

    private NodeStates AddTen()
    {
        m_currentValue += 10;
        m_valueLabel.text = m_currentValue.ToString();
        if (m_currentValue == m_targetValue)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }
}
