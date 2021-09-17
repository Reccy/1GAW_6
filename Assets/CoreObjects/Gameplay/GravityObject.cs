using UnityEngine;
using System.Collections.Generic;

public class GravityObject : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;

    public bool simInFixedUpdate = true;
    private bool m_initialVelocityApplied = false;

    [SerializeField] private bool m_applyInitialVelocityInEditMode = true;

    [SerializeField] private Vector2 m_initialVelocity;
    public Vector2 InitialVelocity => m_initialVelocity;

    private GamePhysicsScene m_scene;

    private List<GravityField> GravityFields => m_scene.Fields;

    private Rigidbody2D MyRigidbody
    {
        get
        {
            if (m_rigidbody == null)
            {
                m_rigidbody = GetComponent<Rigidbody2D>();
            }

            return m_rigidbody;
        }
    }

    public Vector2 Velocity
    {
        get
        {
            ApplyInitialVel();

            return MyRigidbody.velocity;
        }
    }

    private void Awake()
    {
        ApplyInitialVel();
        GetPhysicsScene();
    }

    private void ApplyInitialVel()
    {
        if (!Application.isPlaying && !m_applyInitialVelocityInEditMode)
            return;

        if (m_initialVelocityApplied)
            return;

        MyRigidbody.velocity = m_initialVelocity;

        m_initialVelocityApplied = true;
    }

    private void GetPhysicsScene()
    {
        foreach (GameObject obj in gameObject.scene.GetRootGameObjects())
        {
            m_scene = obj.GetComponent<GamePhysicsScene>();

            if (m_scene != null)
                return;
        }

        Debug.LogWarning("Physics Scene not found for " + gameObject.name + " :: " + m_scene);
    }

    private void FixedUpdate()
    {
        if (simInFixedUpdate)
            Simulate();
    }

    public void Simulate()
    {
        ApplyInitialVel();

        if (m_scene == null)
            GetPhysicsScene();

        if (m_scene == null)
        {
            Debug.LogWarning("Physics not yet loaded for object " + gameObject.name);
            return;
        }

        Vector2 newForce = Vector2.zero;

        foreach (GravityField field in GravityFields)
        {
            newForce += field.CalculateGravity(transform.position);
        }

        MyRigidbody.AddForce(newForce, ForceMode2D.Force);
    }
}
