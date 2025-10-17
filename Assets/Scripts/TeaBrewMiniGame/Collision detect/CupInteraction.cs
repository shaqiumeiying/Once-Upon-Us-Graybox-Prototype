using UnityEngine;

public class CupInteraction : MonoBehaviour
{
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
            Debug.Log("Teapot collided with Cup �� ready to pour tea!");

            // TODO: Temporarliy directly pour tea, but trigger minigame here
            PourTea();
        }
        else
        {
            Debug.Log("Teapot not boiled yet �� can't pour.");
        }
    }

    private void PourTea()
    {

        // TODO: Trigger Pouring MiniGame
        // now directly pour tea
        game.cupHasTea = true;
        Debug.Log("Tea poured successfully! Cup now has tea.");
    }
}
