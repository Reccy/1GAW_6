using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private float m_speed = 10.0f;

    private CinemachineVirtualCamera m_cam;
    private CinemachineOrbitalTransposer m_orbital;

    private void Awake()
    {
        m_cam = GetComponent<CinemachineVirtualCamera>();
        m_orbital = m_cam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void LateUpdate()
    {
        //m_orbital.m_XAxis.Value = 50;
    }
}
