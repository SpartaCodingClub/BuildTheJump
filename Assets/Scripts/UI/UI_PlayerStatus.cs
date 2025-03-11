using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UI_PlayerStatus : UI_Base
{
    private enum Children
    {
        Fill_HP,
        Text_HP,
        Fill_SP,
        Text_SP
    }

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));
    }

    public void UpdateUI_HP(float hp, int maxHP)
    {
        Get<Image>((int)Children.Fill_HP).DOFillAmount(hp / maxHP, 0.2f);
        Get<TMP_Text>((int)Children.Text_HP).text = $"{(int)hp}/{maxHP}";
    }

    public void UpdateUI_SP(float sp, int maxSP)
    {
        Get<Image>((int)Children.Fill_SP).DOFillAmount(sp / maxSP, 0.2f);
        Get<TMP_Text>((int)Children.Text_SP).text = $"{(int)sp}/{maxSP}";
    }
}