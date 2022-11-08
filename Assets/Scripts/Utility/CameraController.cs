using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    private bool startPanning = false;

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.position) > 2f)
        {
            startPanning = true;
        }

        if (startPanning)
        {
            transform.position = AnimMath.Ease(transform.position, target.position, 0.01f);
            if (Vector3.Distance(transform.position, target.position) < 0.01f)
            {
                startPanning = false;
                transform.position = target.position;
            }
        }

    }
}
