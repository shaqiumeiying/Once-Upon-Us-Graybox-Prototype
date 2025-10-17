using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTouching : MonoBehaviour
{
    public GameObject otherObject; // Assign the other object in the Inspector

    void Update()
    {
        if (otherObject == null) return;

        Collider thisCollider = GetComponent<Collider>();
        Collider otherCollider = otherObject.GetComponent<Collider>();

        if (thisCollider == null || otherCollider == null)
        {
            Debug.LogWarning("One of the objects is missing a Collider!");
            return;
        }

        if (thisCollider.bounds.Intersects(otherCollider.bounds))
        {
            Debug.Log(gameObject.name + " is touching " + otherObject.name);
        } else
        {
            Debug.Log(gameObject.name + " not touching " + otherObject.name);
        }
    }
}
