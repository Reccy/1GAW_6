using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class GravityObjectPrediction : MonoBehaviour
{
    private GravityObject m_gravityObject;
    private GravityField[] m_gravityFields;

    [SerializeField] private Polyline m_polyline;
    [SerializeField] int m_iterations;

    PolylinePoint[] m_points;

    private void Awake()
    {
        m_gravityObject = GetComponent<GravityObject>();
        m_gravityFields = FindObjectsOfType<GravityField>();
        m_points = new PolylinePoint[m_iterations];
    }

    private void Update()
    {
        //CalculatePoints();
        m_polyline.SetPoints(m_points);
    }
    /*
    private void CalculatePoints()
    {
        GravityObjectStub stub = m_gravityObject.GetStub();

        for (int i = 0; i < m_iterations; ++i)
        {
            foreach (GravityField field in m_gravityFields)
            {
                field.CalculateGravity(position);

            }
            m_points[i] = new PolylinePoint();
        }
    }
    */
}
