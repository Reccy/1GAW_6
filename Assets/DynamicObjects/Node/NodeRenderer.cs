using UnityEngine;
using TMPro;

public class NodeRenderer : MonoBehaviour
{
    private IPAddress m_ip;

    private MeshRenderer m_renderer;
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private TMP_Text m_ipText;

    [SerializeField] private Material m_onHoverMaterialValid;
    [SerializeField] private Material m_onHoverMaterialInvalid;
    [SerializeField] private Material m_defaultMaterial;
    [SerializeField] private Material m_invisibleMaterial;

    private void Awake()
    {
        m_renderer = GetComponent<MeshRenderer>();
    }

    public void UpdateIP(IPAddress ip)
    {
        m_ip = ip;
        m_ipText.text = ip.ToString();
    }

    public void RenderMouseHoverValid()
    {
        m_renderer.material = m_onHoverMaterialValid;
        m_canvas.enabled = true;
    }

    public void RenderMouseHoverInvalid()
    {
        m_renderer.material = m_onHoverMaterialInvalid;
        m_canvas.enabled = true;
    }

    public void UnrenderMouseHover()
    {
        m_renderer.material = m_defaultMaterial;
        m_canvas.enabled = true;
    }

    public void RenderInvisible()
    {
        m_renderer.material = m_invisibleMaterial;
        m_canvas.enabled = false;
    }
}
