using UnityEngine;

public class GravityField : MonoBehaviour
{
    private CircleCollider2D m_circleCollider;
    private float m_radius;

    [SerializeField] private float m_surfaceGravity;

    private GamePhysicsScene m_phsicsScene;

    private void Awake()
    {
        m_circleCollider = GetComponent<CircleCollider2D>();
        m_radius = m_circleCollider.radius;
    }

    private void OnEnable()
    {
        foreach (GameObject obj in gameObject.scene.GetRootGameObjects())
        {
            GamePhysicsScene sc = obj.GetComponent<GamePhysicsScene>();

            if (sc != null)
            {
                m_phsicsScene = sc;
                sc.Register(this);
                break;
            }
        }
    }

    private void OnDestroy()
    {
        m_phsicsScene.Unregister(this);
    }

    public void Register(GamePhysicsScene scene)
    {
        m_phsicsScene = scene;
        scene.Register(this);
    }

    public void Unregister()
    {
        if (m_phsicsScene == null)
            return;

        m_phsicsScene.Unregister(this);
    }

    // https://www.geeksforgeeks.org/acceleration-due-to-gravity/
    // https://www.toppr.com/guides/physics-formulas/acceleration-due-to-gravity-formula/
    public Vector2 CalculateGravity(Vector2 otherPosition)
    {
        Vector2 dir = ((Vector2)transform.position - otherPosition).normalized;

        float distanceToSurface = Vector2.Distance(transform.position, otherPosition) - m_radius;

        float gravity = m_surfaceGravity * Mathf.Pow(1 + distanceToSurface, -2);

        return dir * gravity;
    }
}
