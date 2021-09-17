using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhysicsScene : MonoBehaviour
{
    private List<GravityField> m_fields;
    public List<GravityField> Fields
    {
        get
        {
            if (m_fields == null)
                m_fields = new List<GravityField>();

            return m_fields;
        }
    }

    public void Register(GravityField field)
    {
        Fields.Add(field);
    }

    public void Unregister(GravityField field)
    {
        Fields.Remove(field);
    }
}
