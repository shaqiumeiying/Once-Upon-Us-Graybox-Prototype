using UnityEngine;

public class TeaGameManager : MonoBehaviour
{
    public static TeaGameManager Instance;

    [Header("Object States")]
    public bool mortarHasLeaves = false;
    public bool teapotHasWater = false;
    public bool teapotHasLeaves = false;
    public bool teapotBoiled = false;
    public bool cupHasTea = false;

    [Header("Inventory Reference")]
    public InventoryManager inventory;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ResetGame()
    {
        mortarHasLeaves = false;
        teapotHasWater = false;
        teapotHasLeaves = false;
        teapotBoiled = false;
        cupHasTea = false;
    }
}
