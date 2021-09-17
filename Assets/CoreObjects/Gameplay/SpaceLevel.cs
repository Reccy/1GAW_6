using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SpaceLevel : MonoBehaviour
{
    private Player m_player;
    private bool m_wonScreenDisplayed = false;
    private bool m_lostScreenDisplayed = false;
    private bool m_outOfFuelScreenDisplayed = false;

    // These Color vals technically shouldn't be here. This is real hack code now babyyyyyy
    [SerializeField] private Color m_lowFuelColor;
    [SerializeField] private Color m_outOfFuelColor;

    [SerializeField] private RectTransform m_wonCanvas;
    [SerializeField] private RectTransform m_lostScreen;
    [SerializeField] private RectTransform m_outOfFuelScreen;
    [SerializeField] private int m_nextLevel;

    private TMP_Text m_outOfFuelText;

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
        m_outOfFuelText = m_outOfFuelScreen.GetComponentInChildren<TMP_Text>();
    }

    private void LateUpdate()
    {
        if (m_player.FuelRemaining < 100)
        {
            DisplayOutOfFuelScreen();

            m_outOfFuelText.text = "DANGER!\nLOW FUEL!";
            m_outOfFuelText.color = m_lowFuelColor;
        }

        if (m_player.IsOutOfFuel)
        {
            m_outOfFuelScreen.GetComponentInChildren<TMP_Text>().text = "CRITICAL!\nOUT OF FUEL!";
            m_outOfFuelText.color = m_outOfFuelColor;

            if (!m_player.IsInOrbit)
            {
                DisplayLostScreen();
            }

            if (m_lostScreenDisplayed)
            {
                if (m_player.RewiredPlayer.GetButtonDown("NextLevel"))
                {
                    SceneManager.LoadScene(gameObject.scene.buildIndex);
                }
            }
        }

        if (m_player.Won)
        {
            DisplayWonScreen();

            if (m_player.RewiredPlayer.GetButtonDown("NextLevel"))
            {
                SceneManager.LoadScene(m_nextLevel);
            }
            return;
        }

        if (m_player.IsDead)
        {
            DisplayLostScreen();

            if (m_player.RewiredPlayer.GetButton("NextLevel"))
            {
                SceneManager.LoadScene(gameObject.scene.buildIndex);
            }
            return;
        }
    }

    private void DisplayOutOfFuelScreen()
    {
        if (m_outOfFuelScreenDisplayed)
            return;

        m_outOfFuelScreenDisplayed = true;

        m_outOfFuelScreen.gameObject.SetActive(true);
    }

    private void DisplayWonScreen()
    {
        if (m_wonScreenDisplayed)
            return;

        m_wonScreenDisplayed = true;

        m_wonCanvas.gameObject.SetActive(true);
    }

    private void DisplayLostScreen()
    {
        if (m_lostScreenDisplayed)
            return;

        m_lostScreenDisplayed = true;

        m_lostScreen.gameObject.SetActive(true);
    }
}
