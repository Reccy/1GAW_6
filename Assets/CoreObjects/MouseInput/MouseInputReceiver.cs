using UnityEngine;
using UnityEngine.Events;

public class MouseInputReceiver : MonoBehaviour
{
    public UnityEvent<Vector3> OnMouseCursorHoverReceived;
    public UnityEvent<Vector3> OnMouseClickDownReceived;
    public UnityEvent<Vector3> OnMouseClickHoldReceived;
    public UnityEvent<Vector3> OnMouseClickUpReceived;

    public void ReceiveMouseCursorHover(Vector3 hitPointWorld)
    {
        OnMouseCursorHoverReceived.Invoke(hitPointWorld);
    }

    public void ReceiveMouseClickDown(Vector3 hitPointWorld)
    {
        OnMouseClickDownReceived.Invoke(hitPointWorld);
    }

    public void ReceiveMouseClickHold(Vector3 hitPointWorld)
    {
        OnMouseClickHoldReceived.Invoke(hitPointWorld);
    }

    public void ReceiveMouseClickUp(Vector3 hitPointWorld)
    {
        OnMouseClickUpReceived.Invoke(hitPointWorld);
    }
}
