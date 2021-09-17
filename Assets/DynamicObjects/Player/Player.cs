using UnityEngine;
using Rewired;

public class Player : MonoBehaviour
{
    private Rewired.Player m_player;
    private const int m_playerID = 0;

    private bool m_isShooting = false;
    private bool m_isThrusting = false;
    private Vector2 m_aim = Vector2.zero;

    private int m_fuelUsed = 0;
    [SerializeField] private int m_fuelUsedPerThrust = 1;
    public int FuelUsed => m_fuelUsed;

    [SerializeField] private float m_framesPerShoot = 4;
    private float m_shootFrames = 0;

    private Rigidbody2D m_rigidbody;

    [SerializeField] float m_thrustForce = 1.0f;
    [SerializeField] ParticleSystem m_exhaustParticles;
    [SerializeField] Bullet m_bulletPrefab;
    [SerializeField] Transform m_bulletSpawn;
    [SerializeField] float m_bulletInitialVelMul = 2.0f;

    private void Start()
    {
        m_player = ReInput.players.GetPlayer(m_playerID);
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        m_isShooting = m_player.GetButton("Shoot");
        m_isThrusting = m_player.GetButton("Thrust");

        m_aim = new Vector2(m_player.GetAxis("AimHorizontal"), m_player.GetAxis("AimVertical"));

        if (m_aim.sqrMagnitude > 0)
            m_aim = m_aim.normalized;
    }

    private void FixedUpdate()
    {
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

        m_shootFrames = m_framesPerShoot;

        // todo: pool objects
        Bullet bullet = Instantiate<Bullet>(m_bulletPrefab);
        bullet.transform.position = m_bulletSpawn.position;
        bullet.transform.rotation = m_bulletSpawn.rotation;
        bullet.SetVelocity(m_rigidbody.velocity + (Vector2)transform.up * m_bulletInitialVelMul);
    }

    private void Thrust()
    {
        m_exhaustParticles.Play(false);
        m_rigidbody.AddForce(transform.up * m_thrustForce);

        Debug.DrawLine(transform.position, transform.position + transform.up, Color.red);

        m_fuelUsed += m_fuelUsedPerThrust;
    }
}
