using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    //void LateUpdate()
    //{
    //    if (Camera.main != null)
    //        transform.rotation = Quaternion.Euler(45, 0, 0);
    //}

    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Get the camera's X rotation (the tilt)
        float camX = Camera.main.transform.eulerAngles.x;

        // Apply only the camera¡¯s X rotation, keep Y and Z unchanged
        transform.rotation = Quaternion.Euler(camX, 0f, 0f);
    }

}
