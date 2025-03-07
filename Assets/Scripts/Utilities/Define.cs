using UnityEngine;

public class Define
{
    #region Animations
    public static readonly int ID_ACTION = Animator.StringToHash("Action");
    public static readonly int ID_ACTION_TREE = Animator.StringToHash("Action_Tree");
    public static readonly int ID_ACTION_ROCK = Animator.StringToHash("Action_Rock");
    public static readonly int ID_MOVE = Animator.StringToHash("Move");
    public static readonly int ID_GROUND = Animator.StringToHash("Ground");
    public static readonly int ID_JUMP = Animator.StringToHash("Jump");
    #endregion
    #region Colors
    public static readonly Color BLUE = new Color32(0, 48, 191, 255);
    public static readonly Color RED = new Color32(191, 48, 0, 255);
    #endregion
    #region Effects
    public static readonly string EFFECT_BUILD = "Effect_Build";
    public static readonly string EFFECT_DEATH = "Effect_Death";
    public static readonly string EFFECT_GET = "Effect_Get";
    public static readonly string EFFECT_HIT = "Effect_Hit";
    public static readonly string EFFECT_ITEM = "Effect_Item";
    #endregion
    #region Layers
    public static readonly int LAYER_GROUND = LayerMask.NameToLayer("Ground");
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
    #endregion
    #region Shaders
    public static readonly string EMISSION_COLOR = "_EmissionColor";
    #endregion
    #region Values
    public static readonly float MAX_WEIGHT = 2500.0f;
    #endregion
}