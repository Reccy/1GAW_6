using UnityEngine;
using System.Collections.Generic;
using Shapes;

public class Node : MonoBehaviour
{
    [SerializeField] private List<Node> m_connections;
    public List<Node> Connections => m_connections;

    private ControlProgram m_controlProgram;
    private NodeRenderer m_nodeRenderer;

    private IPAddress m_ip;

    private bool m_mouseHovering;

    private bool m_isVisible = false;
    public bool IsVisible => m_isVisible;
    public bool IsInvisible => !m_isVisible;

    private bool m_linesVisible = false;
    public bool IsLinesVisible => m_linesVisible;
    public bool IsLinesInvisible => !m_linesVisible;

    private void Awake()
    {
        m_controlProgram = FindObjectOfType<ControlProgram>();
        m_nodeRenderer = GetComponentInChildren<NodeRenderer>();
        m_ip = IPAddress.BuildRandom();

        m_nodeRenderer.UpdateIP(m_ip);
    }

    private void OnDrawGizmos()
    {
        foreach (Node node in m_connections)
        {
            if (node == null)
                continue;

            Debug.DrawLine(transform.position, node.transform.position, Color.red);
        }    
    }

    private void Update()
    {
        if (IsInvisible)
        {
            m_nodeRenderer.RenderInvisible();
        }

        if (IsLinesVisible)
        {
            DrawLinesToOtherNodes();
        }

        if (IsInvisible)
            return;
        
        if (m_mouseHovering)
        {
            if (m_controlProgram.CanTraverseTo(this))
            {
                m_nodeRenderer.RenderMouseHoverValid();
            }
            else
            {
                m_nodeRenderer.RenderMouseHoverInvalid();
            }
        }
        else
        {
            m_nodeRenderer.UnrenderMouseHover();
        }
    }

    private void LateUpdate()
    {
        m_mouseHovering = false;
    }

    private void DrawLinesToOtherNodes()
    {
        using (Draw.Command(Camera.main))
        {
            foreach (Node n in m_connections)
            {
                Color a = Color.red;
                a.a = 0;
                Draw.Line(transform.position, n.transform.position, Color.red, a);
            }
        }
    }

    public void PropagateVisibility()
    {
        m_isVisible = true;
        m_linesVisible = true;

        foreach (Node node in Connections)
        {
            node.m_isVisible = true;
        }
    }

    #region LINKING
    // Use UnlinkSingle when using enumerator
    private void Unlink(Node other)
    {
        other.m_connections.Remove(this);
        m_connections.Remove(other);
    }

    private void UnlinkSingle(Node other)
    {
        other.m_connections.Remove(this);
    }

    private void Link(Node other)
    {
        other.m_connections.Add(this);
        m_connections.Add(other);
    }
    #endregion

    public void OnMouseHover(Vector3 worldPos)
    {
        m_mouseHovering = true;
    }

    public void OnMouseInputDown(Vector3 worldPos)
    {
        if (m_controlProgram.CanTraverseTo(this))
        {
            m_controlProgram.TraverseTo(this);
        }
    }
}
