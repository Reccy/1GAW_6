using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class SpaceLevel : MonoBehaviour
{
    private Player m_player;
    private bool m_wonScreenDisplayed = false;

    [SerializeField] private RectTransform m_wonCanvas;
    [SerializeField] private int m_nextLevel;

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        if (m_player.Won)
        {
            DisplayWonScreen();

            if (m_player.RewiredPlayer.GetButtonDown("NextLevel"))
            {
                SceneManager.LoadScene(m_nextLevel);
            }
        }
    }

    private void DisplayWonScreen()
    {
        if (m_wonScreenDisplayed)
            return;

        m_wonScreenDisplayed = true;

        m_wonCanvas.gameObject.SetActive(true);
    }
}
