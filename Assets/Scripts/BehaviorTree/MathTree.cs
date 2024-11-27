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

    /* 노드는 아래쪽부터 인스턴스화하고 차례로 자식들을 할당한다. */
    void Start()
    {
        /** 가장 깊은 레벨의 노드는 Node 3으로 자식을 갖지 않는다. */
        m_node3 = new ActionNode(NotEqualToTarget);

        /** 다음으로, 레벨 2 노드를 생성한다. */
        m_node2A = new ActionNode(AddTen);

        /** Node 2B는 셀렉터로 노드 3을 자식으로 가지므로 생성자에 이를 전달한다. */
        m_node2B = new Inverter(m_node3);
        m_node2C = new ActionNode(AddTen);

        /** 마지막은 루트 노드로, 일단 여기에 전달할 자식 목록을 만든다. */
        List<Node> rootChildren = new List<Node>();
        rootChildren.Add(m_node2A);
        rootChildren.Add(m_node2B);
        rootChildren.Add(m_node2C);

        /** 그런 후 루트 노드 오브젝트를 만들고 여기에 목록을 전달한다. */
        m_rootNode = new Selector(rootChildren);

        m_valueLabel.text = m_currentValue.ToString();

        m_rootNode.Evaluate();

        UpdateBoxes();
    }

    private void UpdateBoxes()
    {
        /** 루트 노드 박스 갱신 */
        if (m_rootNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_rootNodeBox);
        }
        else if (m_rootNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_rootNodeBox);
        }

        /** 2A 노드 박스 갱신 */
        if (m_node2A.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2aBox);
        }
        else if (m_node2A.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2aBox);
        }

        /** 2B 노드 박스 갱신 */
        if (m_node2B.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2bBox);
        }
        else if (m_node2B.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2bBox);
        }

        /** 2C 노드 박스 갱신 */
        if (m_node2C.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2cBox);
        }
        else if (m_node2C.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2cBox);
        }

        /** 3 노드 박스 갱신 */
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
        // box의 색상을 m_succeeded로 변경
        Renderer renderer;

        renderer = box.GetComponent<Renderer>();
        renderer.material.color = m_succeeded;
    }

    void SetFailed(GameObject box)
    {
        // box의 색상을 m_failed로 변경
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
