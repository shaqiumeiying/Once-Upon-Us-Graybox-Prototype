using UnityEngine;

public class TeaBrewManager : MonoBehaviour
{
    // === Singleton ===
    public static TeaBrewManager Instance;

    private void Awake()
    {
        // instances
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // cross scene for future references
    }

    // === Global State Flags ===
    [Header("Mortar")]
    public bool mortarHasLeaves = false;   // dragged from ui: add tea to mortar
    public bool mortarGrinded = false;      // from grind minigame: grinding done
    public bool mortarBaked = false;       // from stove trigger: bakeing done

    [Header("Teapot")]
    public bool teapotHasWater = false;    // dragged from ui: add water to pot
    public bool teapotHasLeaves = false;   // dragged from ui: add driedtea to pot
    public bool teapotBoiled = false;      // from stove trigger: boiling done

    [Header("Cup")]
    public bool cupHasTea = false;         // pour

    // === Helper Flags ===
    [Header("Process Flags")]
    public bool isBaking = false;
    public bool isBoiling = false;

    // === Inventory / Managers references ===
    [HideInInspector] public InventoryManagerTBM inventory;

    private void Start()
    {
        inventory = FindObjectOfType<InventoryManagerTBM>();
    }

    // === Example helpers for logic checks ===
    public bool CanBake()
    {
        return mortarGrinded && !mortarBaked;
    }

    public bool CanBoil()
    {
        return teapotHasWater && teapotHasLeaves && !teapotBoiled;
    }

    public void ResetProcess()
    {
        mortarHasLeaves = false;
        mortarGrinded = false;
        mortarBaked = false;
        teapotHasWater = false;
        teapotHasLeaves = false;
        teapotBoiled = false;
        cupHasTea = false;
        isBaking = false;
        isBoiling = false;
    }
}
