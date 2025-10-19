using UnityEngine;
using UnityEngine.UI;

public class BookmarkUITabs : MonoBehaviour
{
    [Header("Tab Buttons")]
    public Button alliesButton;
    public Button abilitiesButton;
    public Button artifactsButton;

    [Header("Tab Panels")]
    public GameObject alliesPanel;
    public GameObject abilitiesPanel;
    public GameObject artifactsPanel;

    void Start()
    {
        // Hook up buttons
        alliesButton.onClick.AddListener(() => ShowPanel("Allies"));
        abilitiesButton.onClick.AddListener(() => ShowPanel("Abilities"));
        artifactsButton.onClick.AddListener(() => ShowPanel("Artifacts"));

        // Start default tab
        ShowPanel("Allies");
    }

    void ShowPanel(string tab)
    {
        alliesPanel.SetActive(tab == "Allies");
        abilitiesPanel.SetActive(tab == "Abilities");
        artifactsPanel.SetActive(tab == "Artifacts");
    }
}
