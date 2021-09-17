using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_header;
    [SerializeField] TMP_Text m_value;

    private Player m_player;

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        if (m_player.IsInOrbit && !m_player.IsThrusting)
        {
            m_header.gameObject.SetActive(true);
            m_value.gameObject.SetActive(true);
        }
        else
        {
            m_header.gameObject.SetActive(false);
            m_value.gameObject.SetActive(false);
        }
    }
}
