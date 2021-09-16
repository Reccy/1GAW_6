using UnityEngine;
using System.Collections.Generic;

public class GravityObject : MonoBehaviour
{
    private GravityField[] m_gravityFields;

    private Rigidbody2D m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_gravityFields = FindObjectsOfType<GravityField>();
    }

    private void FixedUpdate()
    {
        if (Physics2D.simulationMode != SimulationMode2D.FixedUpdate)
            return;

        Simulate();
    }

    public void Simulate()
    {
        Vector2 newForce = Vector2.zero;

        foreach (GravityField field in m_gravityFields)
        {
            newForce += field.CalculateGravity(transform.position);
        }

        m_rigidbody.AddForce(newForce);
    }
}
