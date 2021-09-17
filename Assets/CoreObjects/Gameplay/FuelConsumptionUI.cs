using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FuelConsumptionUI : MonoBehaviour
{
    private TMP_Text m_text;
    private Player m_player;

    [SerializeField] private Color m_nominalColor;
    [SerializeField] private Color m_warningColor;
    [SerializeField] private Color m_dangerColor;
    [SerializeField] private Color m_emptyColor;

    private int FuelUsed => m_player.FuelRemaining;

    private void Awake()
    {
        m_text = GetComponent<TMP_Text>();
        m_player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        if (FuelUsed > 100)
        {
            m_text.color = m_nominalColor;
        }
        else if (FuelUsed > 50)
        {
            m_text.color = m_warningColor;
        }
        else if (FuelUsed > 0)
        {
            m_text.color = m_dangerColor;
        }
        else
        {
            m_text.color = m_emptyColor;
        }

        m_text.text = $"{FuelUsed} m/s²";
    }
}
