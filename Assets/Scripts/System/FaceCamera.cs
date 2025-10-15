using UnityEngine;

public class FaceCamera : MonoBehaviour 
{ 
    void LateUpdate() 
    { 
        if (Camera.main != null) 
            transform.rotation = Quaternion.Euler(45, 0, 0); 
    } 
}