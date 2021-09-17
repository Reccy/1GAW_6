using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    private TMP_Text m_text;
    private Player m_player;

    private void Awake()
    {
        m_text = GetComponent<TMP_Text>();
        m_player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        if (m_player.Ammo > 0)
            m_text.text = m_player.Ammo.ToString();
        else
            m_text.text = "empty";
    }
}
