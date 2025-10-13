using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
            transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
