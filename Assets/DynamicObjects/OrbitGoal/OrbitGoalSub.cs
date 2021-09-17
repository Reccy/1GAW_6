using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitGoalSub : MonoBehaviour
{
    private Collider2D m_collider;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
    }

    public bool PlayerInSub(Player player)
    {
        return m_collider.OverlapPoint(player.transform.position);
    }
}
