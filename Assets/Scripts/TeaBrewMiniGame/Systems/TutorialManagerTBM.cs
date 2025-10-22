using UnityEngine;
using UnityEngine.UI;

public class TutorialManagerTBM : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialUIPanel;
    public GameObject generalUIPanel;
    public GameObject tasklistPanel;
    public Button continueButton;

    private WorldDraggable worldDraggable;



    void Start()
    {
        worldDraggable = GameObject.FindWithTag("Mortar").GetComponent<WorldDraggable>();
        if (worldDraggable != null)
            worldDraggable.enabled = false;

        // Make sure the tutorial panel is visible
        tutorialUIPanel.SetActive(true);

        // Hook up button click
        continueButton.onClick.AddListener(CloseTutorial);
    }

    void CloseTutorial()
    {
        // Hide tutorial and enable movement
        tutorialUIPanel.SetActive(false);

        if (worldDraggable != null)
            worldDraggable.enabled = true;

        if (generalUIPanel != null)
            generalUIPanel.SetActive(true);
        if (tasklistPanel != null)
            tasklistPanel.SetActive(true);
    }
}

