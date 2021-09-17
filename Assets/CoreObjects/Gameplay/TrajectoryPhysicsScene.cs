using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class TrajectoryPhysicsScene : MonoBehaviour
{
    private const string SCENE_NAME = "TRAJECTORY_PHYSICS_SCENE";
    private Scene m_scene;
    private PhysicsScene2D m_physicsScene;
    private GamePhysicsScene m_root;
    [SerializeField] private GamePhysicsScene m_rootPrefab;
    private GravityField[] m_fields;
    private GravityObject m_prediction;

    private void Awake()
    {
        InitScene();
    }

    private void InitScene()
    {
        if (m_scene != null && m_scene.isLoaded)
        {
            Debug.LogWarning("Scene {SCENE_NAME} already loaded!", this);
            return;
        }

        CreateSceneParameters sceneParams = new CreateSceneParameters();
        sceneParams.localPhysicsMode = LocalPhysicsMode.Physics2D;

        if (Application.isPlaying)
        {
            m_scene = SceneManager.CreateScene(SCENE_NAME, sceneParams);
        }
        else
        {
#if UNITY_EDITOR
            m_scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            m_scene.name = SCENE_NAME;
#endif
        }

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

            copy.Unregister();

            SceneManager.MoveGameObjectToScene(copy.gameObject, m_scene);

            // Only need to manually register in editor because of MonoBehaviour callbacks not being called
            copy.Register(m_root);

            m_fields[i] = copy;
        }
    }

    public Vector2[] SimulateTrajectory(GravityObject prediction, int iterations, Vector2 initialVelocity)
    {
        if (m_scene == null || !m_scene.isLoaded)
        {
            InitScene();
        }

        Physics2D.simulationMode = SimulationMode2D.Script;
        
        // todo multiple predictions per sim
        if (m_prediction != null)
        {
            if (Application.isPlaying)
                Destroy(m_prediction.gameObject);
            else
                DestroyImmediate(m_prediction.gameObject);
        }

        m_prediction = prediction;

        SceneManager.MoveGameObjectToScene(m_prediction.gameObject, m_scene);

        m_prediction.GetComponent<Rigidbody2D>().velocity = initialVelocity;

        Vector2[] points = new Vector2[iterations];

        for (int i = 0; i < iterations; ++i)
        {
            points[i] = m_prediction.transform.position;

            //Debug.Log(m_prediction.GetComponent<GravityObject>().Velocity);

            if (m_prediction.enabled)
                m_prediction.Simulate();

            if (!m_physicsScene.Simulate(Time.fixedDeltaTime))
                Debug.LogWarning("Simulation fail!");
        }

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        return points;
    }
}
