using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProgramMover : MonoBehaviour
{
    [SerializeField] private Node m_currentNode;
    public Node CurrentNode => m_currentNode;
    public List<Node> Connections => m_currentNode.Connections;

    public enum State { STATIONARY, MOVING }
    private State m_currentState = State.STATIONARY;
    public State CurrentState => m_currentState;

    public bool IsStationary => m_currentState == State.STATIONARY;
    public bool IsMoving => m_currentState == State.MOVING;

    [SerializeField] private float m_moveSpeed = 10.0f;
    [SerializeField] private float m_minDistanceToNode = 0.001f;
    [SerializeField] private bool m_propagatesVisibility = false;

    private Coroutine m_moveCoroutine;

    private void Awake()
    {
        transform.position = m_currentNode.transform.position;
        TraverseTo(m_currentNode);
    }

    public void TraverseTo(Node node)
    {
        if (IsMoving)
            return;

        m_currentState = State.MOVING;

        if (m_moveCoroutine == null)
            m_moveCoroutine = StartCoroutine(TraverseToCoroutine(node));
    }

    private IEnumerator TraverseToCoroutine(Node node)
    {
        m_currentNode = node;

        while (Vector3.Distance(transform.position, m_currentNode.transform.position) > m_minDistanceToNode)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_currentNode.transform.position, Time.deltaTime * m_moveSpeed);
            yield return new WaitForEndOfFrame();
        }

        m_moveCoroutine = null;
        m_currentState = State.STATIONARY;

        if (m_propagatesVisibility)
            m_currentNode.PropagateVisibility();
    }

    public bool CanTraverseTo(Node node)
    {
        if (IsMoving)
            return false;

        return Connections.Contains(node);
    }
}
