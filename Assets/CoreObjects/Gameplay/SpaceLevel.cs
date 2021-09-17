using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceLevel : MonoBehaviour
{
    private Player m_player;
    private bool m_wonScreenDisplayed = false;
    private bool m_lostScreenDisplayed = false;
    private bool m_outOfFuelScreenDisplayed = false;

    [SerializeField] private RectTransform m_wonCanvas;
    [SerializeField] private RectTransform m_lostScreen;
    [SerializeField] private RectTransform m_outOfFuelScreen;
    [SerializeField] private int m_nextLevel;

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        if (m_player.IsOutOfFuel)
        {
            DisplayOutOfFuelScreen();

            if (!m_player.IsInOrbit)
                DisplayLostScreen();
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
