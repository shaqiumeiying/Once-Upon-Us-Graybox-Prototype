using UnityEngine;
using System.Collections;

public class TaskChecklistUI : MonoBehaviour
{
    [Header("Checkmark Icons (GameObjects)")]
    public GameObject checkLeafInMortar;
    public GameObject checkGrindLeaves;
    public GameObject checkHeatLeaves;
    public GameObject checkLeafInPot;
    public GameObject checkWaterInPot;
    public GameObject checkBoilTea;
    public GameObject checkPourTea;

    [Header("End UI")]
    public GameObject congratsUI;

    private TeaBrewManager game;

    // --- Permanent completion flags ---
    private bool doneLeafInMortar;
    private bool doneGrindLeaves;
    private bool doneHeatLeaves;
    private bool doneLeafInPot;
    private bool doneWaterInPot;
    private bool doneBoilTea;
    private bool donePourTea;

    private bool congratsTriggered = false;

    void Start()
    {
        game = TeaBrewManager.Instance;
        HideAll();
        if (congratsUI != null)
            congratsUI.SetActive(false);
    }

    void Update()
    {
        if (game == null) return;
        UpdateChecklist();
    }

    void UpdateChecklist()
    {
        // Once a flag becomes true, record it forever
        if (game.mortarHasLeaves) doneLeafInMortar = true;
        if (game.mortarGrinded) doneGrindLeaves = true;
        if (game.mortarBaked) doneHeatLeaves = true;
        if (game.teapotHasLeaves) doneLeafInPot = true;
        if (game.teapotHasWater) doneWaterInPot = true;
        if (game.teapotBoiled) doneBoilTea = true;
        if (game.cupHasTea) donePourTea = true;
        
        // Update visuals
        checkLeafInMortar.SetActive(doneLeafInMortar);
        checkGrindLeaves.SetActive(doneGrindLeaves);
        checkHeatLeaves.SetActive(doneHeatLeaves);
        checkLeafInPot.SetActive(doneLeafInPot);
        checkWaterInPot.SetActive(doneWaterInPot);
        checkBoilTea.SetActive(doneBoilTea);
        checkPourTea.SetActive(donePourTea);

        // trigger congrats after 2 seconds delay
        if (donePourTea && !congratsTriggered)
        {
            congratsTriggered = true;
            StartCoroutine(ShowCongratsAfterDelay(4f));
        }
    }

    IEnumerator ShowCongratsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (congratsUI != null)
        {
            congratsUI.SetActive(true);
            Debug.Log("Congrats! Tea brewed successfully!");
        }
    }

    void HideAll()
    {
        checkLeafInMortar.SetActive(false);
        checkGrindLeaves.SetActive(false);
        checkHeatLeaves.SetActive(false);
        checkLeafInPot.SetActive(false);
        checkWaterInPot.SetActive(false);
        checkBoilTea.SetActive(false);
        checkPourTea.SetActive(false);
    }
}
