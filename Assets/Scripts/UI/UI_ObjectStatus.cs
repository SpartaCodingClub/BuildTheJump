using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ObjectStatus : UI_Base
{
    #region Open
    private Sequence StatusBar_Open()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(1.0f, 0.3f).From(0.0f));
    }

    private Sequence Frame_Open()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(1.0f, 0.3f).From(0.0f).SetEase(Ease.OutBack));
    }
    #endregion
    #region Close
    private Sequence StatusBar_Close()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(0.0f, 0.3f));
    }

    private Sequence Frame_Close()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(0.0f, 0.3f))
            .AppendCallback(StopAllCoroutines);
    }
    #endregion

    private enum Children
    {
        Frame,
        Fill_White,
        Fill_Red,
        Text_Name,
        Text_Description
    }

    private Image fillWhite;
    private Image fillRed;

    private float previousHP;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, StatusBar_Open, Frame_Open);
        BindSequences(UIState.Close, StatusBar_Close, Frame_Close);

        fillWhite = Get<Image>((int)Children.Fill_White);
        fillRed = Get<Image>((int)Children.Fill_Red);
    }

    public override void Open()
    {
        base.Open();
        previousHP = 0.0f;
    }

    public void UpdateUI(float hp, BaseData objectData)
    {
        float fillAmount = hp / objectData.HP;
        if (previousHP == 0.0f)
        {
            fillWhite.fillAmount = fillAmount;
            previousHP = hp;
        }

        Get<TMP_Text>((int)Children.Text_Name).text = objectData.Name;
        Get<TMP_Text>((int)Children.Text_Description).text = objectData.Description;

        fillRed.fillAmount = fillAmount;
        StartCoroutine(Filling());
    }

    private IEnumerator Filling()
    {
        while (fillWhite.fillAmount > fillRed.fillAmount)
        {
            fillWhite.fillAmount = Mathf.Lerp(fillWhite.fillAmount, fillRed.fillAmount, 5.0f * Time.deltaTime);
            yield return null;
        }
    }
}