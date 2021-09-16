using UnityEngine;

public class GravityField : MonoBehaviour
{
    [SerializeField] private float m_surfaceGravity;
    [SerializeField] private float m_radius;

    public Vector2 CalculateGravity(GravityObject otherBody)
    {
        Vector2 dir = (transform.position - otherBody.transform.position).normalized;

        float distanceToSurface = Vector2.Distance(transform.position, otherBody.transform.position) - m_radius;

        float gravity = m_surfaceGravity * Mathf.Pow(1 + distanceToSurface, -2);

        return dir * gravity;
    }
}
