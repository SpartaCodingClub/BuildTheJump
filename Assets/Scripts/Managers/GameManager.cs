using DG.Tweening;
using UnityEngine;

public class GameManager
{
    private readonly int HP = 100;
    private readonly int SP = 200;

    public float CurrentHP { get; private set; }
    public float CurrentSP { get; private set; }
    public P_Rigidbody Player { get; private set; }
    public P_Interaction Interaction { get; private set; }

    private UI_PlayerStatus playerStatusUI;

    private int previousHP;
    private int previousSP;

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
        if (CurrentHP < HP)
        {
            CurrentHP += Time.deltaTime;
        }

        if (CurrentHP != previousHP)
        {
            previousHP = (int)CurrentHP;
            playerStatusUI.UpdateUI_HP(CurrentHP, HP);
        }

        if (CurrentSP < SP)
        {
            CurrentSP += Time.deltaTime * 2.0f;
        }

        if (CurrentSP != previousSP)
        {
            previousSP = (int)CurrentSP;
            playerStatusUI.UpdateUI_SP(CurrentSP, SP);
        }
    }

    public void SetHP(int value)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + value, 0, HP);
    }

    public void SetSP(int value)
    {
        CurrentSP = Mathf.Clamp(CurrentSP + value, 0, SP);
    }
}