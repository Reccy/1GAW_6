using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrbitStatusUI : MonoBehaviour
{
    private TMP_Text m_text;
    private Player m_player;

    [SerializeField] private Color m_goalColor;
    [SerializeField] private Color m_validatingColor;
    [SerializeField] private Color m_outOfGoalColor;
    [SerializeField] private Color m_thrustWinColor;
    [SerializeField] private string m_winText;
    [SerializeField] private string m_winningText;
    [SerializeField] private string m_loseText;
    [SerializeField] private string m_thrustWinText;

    private void Awake()
    {
        m_text = GetComponent<TMP_Text>();
        m_player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        if (m_player.IsInOrbit)
        {
            if (m_player.IsThrusting)
            {
                m_text.text = m_thrustWinText;
                m_text.color = m_thrustWinColor;
            }
            else if (m_player.TimeInOrbit < m_player.TimeInOrbitGoal)
            {
                m_text.text = m_winningText;
                m_text.color = m_validatingColor;
            }
            else
            {
                m_text.text = m_winText;
                m_text.color = m_goalColor;
            }
        }
        else
        {
            m_text.text = m_loseText;
            m_text.color = m_outOfGoalColor;
        }
    }
}
