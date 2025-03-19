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

        int currnetHP = (int)CurrentHP;
        if (currnetHP != previousHP)
        {
            previousHP = currnetHP;
            playerStatusUI.UpdateUI_HP(CurrentHP, HP);
        }

        if (CurrentSP < SP)
        {
            CurrentSP += Time.deltaTime * 2.0f;
        }

        int currentSP = (int)CurrentSP;
        if (currentSP != previousSP)
        {
            previousSP = currentSP;
            playerStatusUI.UpdateUI_SP(CurrentSP, SP);
        }
    }

    public void SetHP(int value)
    {
        float amount = Mathf.Clamp(CurrentHP + value, 0, HP) - CurrentHP;
        if (Mathf.Approximately(amount, 0.0f))
        {
            return;
        }

        CurrentHP += amount;
        Managers.UI.Open<UI_FloatingText>().UpdateUI(amount.ToString("0"), Managers.Game.Player.transform.position, Color.green);
    }

    public void SetSP(int value)
    {
        float amount = Mathf.Clamp(CurrentSP + value, 0, SP) - CurrentSP;
        if (amount > 0.0f)
        {
            Managers.UI.Open<UI_FloatingText>().UpdateUI(amount.ToString("0"), Managers.Game.Player.transform.position, Color.yellow);
        }

        CurrentSP += amount;
    }
}