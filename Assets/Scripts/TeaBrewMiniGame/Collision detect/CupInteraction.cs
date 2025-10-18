using UnityEngine;

public class CupInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pouringMinigameObject;  // Object containing PouringMinigameManager

    private TeaBrewManager game;

    void Start()
    {
        game = TeaBrewManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Teapot")) return;

        if (game.teapotBoiled)
        {
            Debug.Log("Teapot collided with Cup ¡ª ready to pour tea!");

            // TODO: Temporarliy directly pour tea, but trigger minigame here
            PourTea();
        }
        else
        {
            Debug.Log("Teapot not boiled yet ¡ª can't pour.");
        }
    }

    private void PourTea()
    {
        // Set PouringMinigame Object to active to trigger minigame
        pouringMinigameObject.SetActive(true);
        // now directly pour tea
        game.cupHasTea = true;
        Debug.Log("Tea poured successfully! Cup now has tea.");
    }
}
