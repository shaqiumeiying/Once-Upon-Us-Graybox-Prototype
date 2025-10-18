using UnityEngine;

public class InteractableState : MonoBehaviour
{
    [Header("Contents")]
    public bool hasTeaLeaves = false;
    public bool hasWater = false;

    [Header("Progress")]
    public bool isBaking = false;
    public bool isBoiling = false;

    [Header("Results (TODO)")]
    public bool bakedDone = false;
    public bool boiledDone = false;
}
