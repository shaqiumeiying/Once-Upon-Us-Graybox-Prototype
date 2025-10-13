using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialUIPanel;
    public GameObject generalUIPanel;
    public Button continueButton;

    private PlayerMovement playerMovement;


    void Start()
    {
        // Find the player and disable movement at start
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Make sure the tutorial panel is visible
        tutorialUIPanel.SetActive(true);

        // Hook up button click
        continueButton.onClick.AddListener(CloseTutorial);
    }

    void CloseTutorial()
    {
        // Hide tutorial and enable movement
        tutorialUIPanel.SetActive(false);
    
        if (playerMovement != null)
            playerMovement.enabled = true;
        if (generalUIPanel != null)
            generalUIPanel.SetActive(true);
    }
}

