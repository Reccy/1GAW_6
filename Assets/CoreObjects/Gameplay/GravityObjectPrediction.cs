using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class GravityObjectPrediction : MonoBehaviour
{
    private GravityObject m_gravityObject;
    private GravityField[] m_gravityFields;
    private TrajectoryPhysicsScene m_physicsScene;
    private Rigidbody2D m_rigidbody;

    [SerializeField] private Polyline m_polyline;
    [SerializeField] int m_iterations = 12;
    [SerializeField] GameObject m_predictionPrefab;
    [SerializeField] Color m_startColor = Color.white;
    [SerializeField] Color m_endColor = Color.black;

    PolylinePoint[] m_points;

    private void Awake()
    {
        m_gravityObject = GetComponent<GravityObject>();
        m_gravityFields = FindObjectsOfType<GravityField>();
        m_points = new PolylinePoint[m_iterations];
        m_physicsScene = FindObjectOfType<TrajectoryPhysicsScene>();
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_polyline.transform.parent = null;
        m_polyline.transform.localPosition = Vector2.zero;
    }

    private void Update()
    {
        CalculatePoints();
        m_polyline.SetPoints(m_points);
    }

    private void CalculatePoints()
    {
        Vector2[] points = m_physicsScene.SimulateTrajectory(CreateStub(), m_iterations, m_rigidbody.velocity);

        for (int i = 0; i < points.Length; ++i)
        {
            float t = (float)i / (float)points.Length;

            Color c = Color.Lerp(m_startColor, m_endColor, t);

            m_points[i] = new PolylinePoint(points[i]);
            m_points[i].color = c;
        }

        m_polyline.SetPoints(m_points);
    }

    private GravityObject CreateStub()
    {
        GameObject result = Instantiate(m_predictionPrefab);
        result.transform.position = transform.position;
        return result.GetComponent<GravityObject>();
    }
}
