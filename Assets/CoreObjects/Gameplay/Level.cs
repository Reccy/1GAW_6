using UnityEngine;

public class Level : MonoBehaviour
{
    public delegate void OnPlayerTraverseEvent(Node from, Node to);
    public OnPlayerTraverseEvent OnPlayerTraverse;

    public void NotifyPlayerTraversed(Node from, Node to)
    {
        if (OnPlayerTraverse != null)
            OnPlayerTraverse.Invoke(from, to);
    }
}
