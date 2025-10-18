using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneExitButton : MonoBehaviour
{
    [Header("Target Scene")]
    public string sceneToLoad = "Map1";

    void Start()
    {
        // Automatically hook the button
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(ReturnToMap);
    }

    public void ReturnToMap()
    {
        Debug.Log($"[DEBUG] Returning to scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }
}
