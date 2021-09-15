using UnityEngine;

public class ChaserProgram : MonoBehaviour
{
    private ProgramMover m_mover;
    private Level m_session;

    private void Awake()
    {
        m_mover = GetComponent<ProgramMover>();
        m_session = FindObjectOfType<Level>();
    }

    private void OnEnable()
    {
        m_session.OnPlayerTraverse += OnPlayerTraverse;
    }

    private void OnDisable()
    {
        m_session.OnPlayerTraverse -= OnPlayerTraverse;
    }

    private void OnPlayerTraverse(Node from, Node to)
    {
        NodePathfinder pathfinder = new NodePathfinder(m_mover.CurrentNode, to);

        m_mover.TraverseTo(pathfinder.Next);
    }
}
