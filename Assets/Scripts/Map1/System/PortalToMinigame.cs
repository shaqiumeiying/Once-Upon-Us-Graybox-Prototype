using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToMinigame : MonoBehaviour
{
    public string sceneToLoad = "TeaBrewMiniGame";

    private InteractionIndicator indicator;

    void Start()
    {
        indicator = GetComponent<InteractionIndicator>();
    }

    void Update()
    {
        if (indicator != null && indicator.IsPlayerInRange() && (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(0)))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
