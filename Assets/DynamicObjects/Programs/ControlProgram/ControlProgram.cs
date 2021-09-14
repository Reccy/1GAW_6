using UnityEngine;

[RequireComponent(typeof(ProgramMover))]
public class ControlProgram : MonoBehaviour
{
    private ProgramMover m_mover;

    private void Awake()
    {
        m_mover = GetComponent<ProgramMover>();
    }

    private void Update()
    {
        // TODO: Rewired Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_mover.TraverseTo(m_mover.Connections[0]);
        }
    }

    public bool CanTraverseTo(Node node)
    {
        return m_mover.CanTraverseTo(node);
    }

    public void TraverseTo(Node node)
    {
        m_mover.TraverseTo(node);
    }
}
