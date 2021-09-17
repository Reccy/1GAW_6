using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FuelConsumptionUI : MonoBehaviour
{
    private TMP_Text m_text;
    private Player m_player;

    private int FuelUsed => m_player.FuelUsed;

    private void Awake()
    {
        m_text = GetComponent<TMP_Text>();
        m_player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        m_text.text = $"{FuelUsed} m/s²";
    }
}
