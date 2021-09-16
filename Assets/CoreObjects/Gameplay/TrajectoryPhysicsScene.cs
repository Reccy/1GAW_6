using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryPhysicsScene : MonoBehaviour
{
    private const string SCENE_NAME = "TRAJECTORY_PHYSICS_SCENE";
    private Scene m_scene;
    private PhysicsScene2D m_physicsScene;
    private TrajectoryPhysicsSceneManager m_root;
    [SerializeField] private TrajectoryPhysicsSceneManager m_rootPrefab;
    private GravityField[] m_fields;
    private GravityObject m_prediction;

    public void Awake()
    {
        if (m_scene != null && m_scene.isLoaded)
        {
            Debug.LogWarning("Scene {SCENE_NAME} already loaded!", this);
            return;
        }

        CreateSceneParameters sceneParams = new CreateSceneParameters();
        sceneParams.localPhysicsMode = LocalPhysicsMode.Physics2D;
        m_scene = SceneManager.CreateScene(SCENE_NAME, sceneParams);

        m_physicsScene = m_scene.GetPhysicsScene2D();

        m_root = Instantiate(m_rootPrefab);
        SceneManager.MoveGameObjectToScene(m_root.gameObject, m_scene);

        GravityField[] fields = FindObjectsOfType<GravityField>();
        m_fields = new GravityField[fields.Length];

        for (int i = 0; i < fields.Length; ++i)
        {
            GravityField original = fields[i];
            GravityField copy = Instantiate(original);

            foreach (Renderer r in copy.GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }

            foreach (Shapes.ShapeRenderer r in copy.GetComponentsInChildren<Shapes.ShapeRenderer>())
            {
                r.enabled = false;
            }

            SceneManager.MoveGameObjectToScene(copy.gameObject, m_scene);
        }
    }

    public Vector2[] SimulateTrajectory(GravityObject prediction, int iterations, Vector2 initialVelocity)
    {
        if (m_prediction != null)
            Destroy(m_prediction.gameObject);

        m_prediction = prediction;

        SceneManager.MoveGameObjectToScene(m_prediction.gameObject, m_scene);

        m_prediction.GetComponent<Rigidbody2D>().velocity = initialVelocity;

        Vector2[] points = new Vector2[iterations];

        for (int i = 0; i < iterations; ++i)
        {
            points[i] = m_prediction.transform.position;

            m_prediction.Simulate();

            if (!m_physicsScene.Simulate(Time.fixedDeltaTime))
                Debug.LogWarning("Simulation fail!");
        }

        for (int i = 0; i < iterations - 1; ++i)
        {
            Debug2.DrawArrow(points[i], points[i + 1], Color.green);
        }

        return points;
    }
}
