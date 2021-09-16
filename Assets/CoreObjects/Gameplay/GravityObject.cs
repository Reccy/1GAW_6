using UnityEngine;
using System.Collections.Generic;

public class GravityObject : MonoBehaviour
{
    private GravityField[] m_gravityFields;

    private Rigidbody2D m_rigidbody;

    public bool simInFixedUpdate = true;

    private GravityField[] GravityFields
    {
        get
        {
            if (m_gravityFields == null)
            {
                m_gravityFields = FindObjectsOfType<GravityField>();
            }

            return m_gravityFields;
        }
    }

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (simInFixedUpdate)
            Simulate();
    }

    public void Simulate()
    {
        Vector2 newForce = Vector2.zero;

        foreach (GravityField field in GravityFields)
        {
            newForce += field.CalculateGravity(transform.position);
        }

        m_rigidbody.AddForce(newForce, ForceMode2D.Force);
    }
}
