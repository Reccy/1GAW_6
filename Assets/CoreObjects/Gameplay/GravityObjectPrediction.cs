using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class GravityObjectPrediction : MonoBehaviour
{
    private TrajectoryPhysicsScene m_physicsScene;
    private Rigidbody2D m_rigidbody;

    [SerializeField] private Polyline m_polyline;
    [SerializeField] int m_iterations = 12;
    [SerializeField] GameObject m_predictionPrefab;
    [SerializeField] Color m_startColor = Color.white;
    [SerializeField] Color m_endColor = Color.black;

    private GravityObject m_gravityObject;

    PolylinePoint[] m_points;

    [InspectorButton("CheckOrbit")]
    public bool m_checkOrbit;

    private void CheckOrbit()
    {
        m_points = new PolylinePoint[m_iterations];
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_physicsScene = FindObjectOfType<TrajectoryPhysicsScene>();
        m_gravityObject = GetComponent<GravityObject>();

        m_points = CalculatePoints();

        Polyline polyline = Instantiate(m_polyline);
        //polyline.hideFlags = HideFlags.DontSave;

        polyline.transform.parent = null;
        polyline.transform.localPosition = Vector2.zero;

        polyline.SetPoints(m_points);
    }

    private void Awake()
    {
        m_points = new PolylinePoint[m_iterations];
        m_physicsScene = FindObjectOfType<TrajectoryPhysicsScene>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_gravityObject = GetComponent<GravityObject>();

        m_polyline.transform.parent = null;
        m_polyline.transform.localPosition = Vector2.zero;
    }

    private void Update()
    {
        m_points = CalculatePoints();
        m_polyline.SetPoints(m_points);
    }

    private PolylinePoint[] CalculatePoints()
    {
        Vector2[] points;

        points = m_physicsScene.SimulateTrajectory(CreateStub(), m_iterations, m_gravityObject.Velocity);

        for (int i = 0; i < points.Length; ++i)
        {
            float t = (float)i / (float)points.Length;

            Color c = Color.Lerp(m_startColor, m_endColor, t);

            m_points[i] = new PolylinePoint(points[i]);
            m_points[i].color = c;
        }

        return m_points;
    }

    private GravityObject CreateStub()
    {
        GameObject result = Instantiate(m_predictionPrefab);
        result.transform.position = transform.position;

        GravityObject obj = result.GetComponent<GravityObject>();
        obj.simInFixedUpdate = false;

        return obj;
    }

    private void OnEnable()
    {
        if (m_polyline != null)
            m_polyline.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (m_polyline != null)
            m_polyline.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (m_polyline != null)
            Destroy(m_polyline.gameObject);
    }
}
