using UnityEngine;

public class Define
{
    #region Animations
    public static readonly int ID_ACTION = Animator.StringToHash("Action");
    public static readonly int ID_ACTION_SPEED = Animator.StringToHash("ActionSpeed");
    public static readonly int ID_ACTION_BONFIRE = Animator.StringToHash("Action_Bonfire");
    public static readonly int ID_ACTION_TREE = Animator.StringToHash("Action_Tree");
    public static readonly int ID_ACTION_ROCK = Animator.StringToHash("Action_Rock");
    public static readonly int ID_ATTACK = Animator.StringToHash("Attack");
    public static readonly int ID_MOVE = Animator.StringToHash("Move");
    public static readonly int ID_MOVE_SPEED = Animator.StringToHash("MoveSpeed");
    public static readonly int ID_GROUND = Animator.StringToHash("Ground");
    public static readonly int ID_JUMP = Animator.StringToHash("Jump");
    #endregion
    #region Colors
    public static readonly Color BLUE = new Color32(0, 48, 191, 255);
    public static readonly Color RED = new Color32(191, 48, 0, 255);
    public static readonly Color SKYBLUE = new Color32(128, 255, 255, 255);
    #endregion
    #region Effects
    public static readonly string EFFECT = "Effect";
    public static readonly string EFFECT_BUILD = "Effect_Build";
    public static readonly string EFFECT_DEATH = "Effect_Death";
    public static readonly string EFFECT_INTERACTION = "Effect_Interaction";
    public static readonly string EFFECT_ITEM = "Effect_Item";
    public static readonly string EFFECT_ITEM_GET = "Effect_ItemGet";
    #endregion
    #region Layers
    public static readonly string LAYER_DEFAULT = "Default";
    public static readonly string LAYER_GROUND = "Ground";
    public static readonly string LAYER_MONSTER = "Monster";
    public static readonly string LAYER_OBJECT = "Object";
    #endregion
    #region Path
    public const string PATH_ATLAS = "Atlas";
    public const string PATH_BUILDING = "Buildings";
    public const string PATH_EFFECT = "Effects";
    public const string PATH_ITEM = "Items";
    public const string PATH_MATERIAL = "Materials";
    public const string PATH_OBJECT = "Objects";
    public const string PATH_UI = "UI";
    public const string PATH_UNIT = "Units";
    #endregion
    #region Shaders
    public static readonly string EMISSION_COLOR = "_EmissionColor";
    #endregion
    #region Values
    public static readonly float MAX_WEIGHT = 2500.0f;
    public static readonly float PLAYER_MOVE_SPEED = 5.0f;
    public static readonly float WORKER_MOVE_SPEED = 3.5f;
    #endregion
}