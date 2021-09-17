using UnityEngine;
using Rewired;

public class Player : MonoBehaviour
{
    private Rewired.Player m_player;
    private const int m_playerID = 0;

    public Rewired.Player RewiredPlayer => m_player;

    private bool m_isShooting = false;
    private bool m_isThrusting = false;
    private Vector2 m_aim = Vector2.zero;

    public bool IsThrusting => m_isThrusting;

    [SerializeField] private int m_fuelRemaining = 0;
    [SerializeField] private int m_fuelUsedPerThrust = 1;
    [SerializeField] public int FuelRemaining => m_fuelRemaining;

    public bool IsOutOfFuel => m_fuelRemaining == 0;

    [SerializeField] private float m_framesPerShoot = 4;
    private float m_shootFrames = 0;

    private Rigidbody2D m_rigidbody;

    [SerializeField] float m_thrustForce = 1.0f;
    [SerializeField] ParticleSystem m_exhaustParticles;
    [SerializeField] Bullet m_bulletPrefab;
    [SerializeField] Transform m_bulletSpawn;
    [SerializeField] float m_bulletInitialVelMul = 2.0f;

    [SerializeField] private float m_ammo = 100;
    public float Ammo => m_ammo;

    private bool m_isInOrbit = false;
    public bool IsInOrbit => m_isInOrbit;

    private bool m_dead = false;
    public bool IsDead => m_dead;

    private float m_timeInOrbit = 0;
    [SerializeField] private float m_timeInOrbitGoal = 10.0f;

    public float TimeInOrbit => m_timeInOrbit;
    public float TimeInOrbitGoal => m_timeInOrbitGoal;
    public bool Won => TimeInOrbit >= TimeInOrbitGoal;

    private OrbitGoal m_orbitGoals;

    private void Start()
    {
        m_player = ReInput.players.GetPlayer(m_playerID);
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_orbitGoals = FindObjectOfType<OrbitGoal>();
    }

    private void Update()
    {
        m_isShooting = m_player.GetButton("Shoot");
        m_isThrusting = m_player.GetButton("Thrust");

        if (m_ammo == 0)
            m_isShooting = false;

        if (IsOutOfFuel)
            m_isThrusting = false;

        m_aim = new Vector2(m_player.GetAxis("AimHorizontal"), m_player.GetAxis("AimVertical"));

        if (m_aim.sqrMagnitude > 0)
            m_aim = m_aim.normalized;
    }

    private void FixedUpdate()
    {
        // Won, don't accept any more player input!!!
        if (m_timeInOrbit > m_timeInOrbitGoal)
            return;

        if (m_dead)
            return;

        LookAtCursor();

        if (m_isShooting)
        {
            Shoot();
        }

        if (m_isThrusting)
        {
            Thrust();
        }
        else
        {
            m_exhaustParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }

        if (m_shootFrames > 0)
        {
            m_shootFrames -= 1;
        }

        m_isInOrbit = m_orbitGoals.PlayerInGoal(this);

        if (m_isInOrbit && !m_isThrusting)
        {
            m_timeInOrbit += Time.deltaTime;
        }
        else
        {
            m_timeInOrbit = 0;
        }
    }

    private void LookAtCursor()
    {
        // Mouse Input
        if (m_aim.sqrMagnitude == 0 && ReInput.controllers.Mouse.screenPositionDelta.sqrMagnitude > 0)
        {
            // todo cache plane creation
            Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, 10));

            Vector3 playerPosOnPlane = plane.ClosestPointOnPlane(transform.position);
            Vector3 mousePosOnPlane = plane.ClosestPointOnPlane(Camera.main.ScreenToWorldPoint(ReInput.controllers.Mouse.screenPosition));

            Debug2.DrawCross(playerPosOnPlane, Color.red);
            Debug2.DrawCross(mousePosOnPlane, Color.red);

            m_aim = (mousePosOnPlane - playerPosOnPlane).normalized;

            Debug2.DrawArrow(playerPosOnPlane, (Vector2)playerPosOnPlane + m_aim * 3, Color.green);
        }

        if (m_aim.sqrMagnitude > 0)
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(m_aim.y, m_aim.x) + 270);
    }

    private void Shoot()
    {
        if (m_shootFrames > 0)
            return;

        if (m_ammo == 0)
            return;

        m_shootFrames = m_framesPerShoot;

        // todo: pool objects (hahaha i dont have time)
        Bullet bullet = Instantiate<Bullet>(m_bulletPrefab);
        bullet.transform.position = m_bulletSpawn.position;
        bullet.transform.rotation = m_bulletSpawn.rotation;
        bullet.SetVelocity(m_rigidbody.velocity + (Vector2)transform.up * m_bulletInitialVelMul);

        m_ammo -= 1;
    }

    private void Thrust()
    {
        if (IsOutOfFuel)
            return;

        m_exhaustParticles.Play(false);
        m_rigidbody.AddForce(transform.up * m_thrustForce);

        Debug.DrawLine(transform.position, transform.position + transform.up, Color.red);

        m_fuelRemaining = Mathf.Max(0, m_fuelRemaining - m_fuelUsedPerThrust);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<GravityField>() || collision.gameObject.GetComponent<Bullet>())
        {
            DestroyPlayer();
        }
    }

    private void DestroyPlayer()
    {
        m_dead = true;

        gameObject.SetActive(false);
    }
}
