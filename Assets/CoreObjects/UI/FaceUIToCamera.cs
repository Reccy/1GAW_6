using UnityEngine;

public class FaceUIToCamera : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform.position + (Camera.main.transform.forward * 1000));
    }
}
