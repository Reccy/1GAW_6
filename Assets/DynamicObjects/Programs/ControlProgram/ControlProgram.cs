using UnityEngine;

public class ControlProgram : MonoBehaviour
{
    private ProgramMover m_mover;
    private Level m_level;

    private void Awake()
    {
        m_mover = GetComponent<ProgramMover>();
        m_level = FindObjectOfType<Level>();
    }

    public bool CanTraverseTo(Node node)
    {
        return m_mover.CanTraverseTo(node);
    }

    public void TraverseTo(Node node)
    {
        m_mover.TraverseTo(node);
        m_level.NotifyPlayerTraversed(m_mover.CurrentNode, node);
    }
}
