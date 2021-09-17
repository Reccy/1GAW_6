using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    private Rewired.Player m_player;
    [SerializeField] private int m_firstLevel;
    [SerializeField] private GameObject m_introTextObj;
    [SerializeField] private GameObject m_loadingTextObj;

    private void Start()
    {
        m_player = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        if (m_player.GetButtonDown("NextLevel"))
        {
            m_introTextObj.SetActive(false);
            m_loadingTextObj.SetActive(true);

            SceneManager.LoadScene(m_firstLevel);
        }
    }
}
