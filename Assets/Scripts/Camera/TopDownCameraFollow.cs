using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TopDownCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target != null)
            transform.position = target.position + offset;
    }
}
