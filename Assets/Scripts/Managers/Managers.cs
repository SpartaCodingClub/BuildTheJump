using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance { get; private set; }

    public static readonly BuildingManager Building = new();
    public static readonly CameraManager Camera = new();
    public static readonly DataManager Data = new();
    public static readonly GameManager Game = new();
    public static readonly InputManager Input = new();
    public static readonly ItemManager Item = new();
    public static readonly PoolManager Pool = new();
    public static readonly ResourceManager Resource = new();
    public static readonly UIManager UI = new();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        P_Rigidbody player = FindAnyObjectByType<P_Rigidbody>();
        P_Interaction interaction = FindAnyObjectByType<P_Interaction>();
        Game.Initialize(player, interaction);

        Building.Initialize();
        Camera.Initialize();
        Data.Initialize();
        Input.Initialize();
        Pool.Initialize();
        Resource.Initialize();
        UI.Initialize();
    }

    private void Update()
    {
        Building.Update();
        UI.Update();
    }

    private void LateUpdate()
    {
        Camera.LateUpdate();
    }
}