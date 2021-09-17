using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    [SerializeField] private float m_lifetimeSeconds = 10.0f;
    [SerializeField] private bool m_selfDestructEnabled = false;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        SelfDestructCheck();

        float angle = Mathf.Atan2(m_rigidbody.velocity.x, m_rigidbody.velocity.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);

        Debug.DrawLine(transform.position, transform.position + (Vector3)m_rigidbody.velocity * 10, Color.red);
    }

    private void SelfDestructCheck()
    {
        if (m_selfDestructEnabled)
        {
            m_lifetimeSeconds -= Time.deltaTime;

            if (m_lifetimeSeconds <= 0)
                DestroyBullet();
        }
    }

    public void SetVelocity(Vector2 vel)
    {
        m_rigidbody.velocity = vel;
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<GravityField>() || collision.gameObject.GetComponent<Player>())
        {
            DestroyBullet();
        }
    }
}
