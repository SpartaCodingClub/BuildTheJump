using DG.Tweening;
using UnityEngine;

public class GameManager
{
    private readonly int HP = 200;
    private readonly int SP = 100;

    public int CurrentHP { get; private set; }
    public int CurrentSP { get; private set; }
    public P_Rigidbody Player { get; private set; }
    public P_Interaction Interaction { get; private set; }

    private UI_PlayerStatus playerStatusUI;

    public void Initialize(P_Rigidbody player, P_Interaction interaction)
    {
        CurrentHP = HP;
        CurrentSP = SP;
        Player = player;
        Interaction = interaction;

        playerStatusUI = Managers.UI.Open<UI_PlayerStatus>();

        Application.targetFrameRate = 60;
        DOTween.SetTweensCapacity(200, 500);
    }

    public void Update()
    {
        // TODO: 자연회복 구현예정
    }

    public void SetHP(int value)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + value, 0, HP);
        playerStatusUI.UpdateUI_HP(CurrentHP, HP);
    }

    public void SetSP(int value)
    {
        CurrentSP = Mathf.Clamp(CurrentSP + value, 0, SP);
        playerStatusUI.UpdateUI_SP(CurrentSP, SP);
    }
}