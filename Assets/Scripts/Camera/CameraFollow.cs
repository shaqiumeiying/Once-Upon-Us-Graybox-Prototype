using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

}
