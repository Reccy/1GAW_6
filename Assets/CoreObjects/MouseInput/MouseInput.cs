using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            MouseInputReceiver receiver = hit.collider.gameObject.GetComponentInParent<MouseInputReceiver>();

            if (receiver == null)
                return;

            Vector3 pos = hit.point;

            receiver.ReceiveMouseCursorHover(pos);

            if (Input.GetMouseButtonDown(0))
            {
                receiver.ReceiveMouseClickDown(pos);
            }

            if (Input.GetMouseButton(0))
            {
                receiver.ReceiveMouseClickHold(pos);
            }

            if (Input.GetMouseButtonUp(0))
            {
                receiver.ReceiveMouseClickUp(pos);
            }
        }
    }
}
