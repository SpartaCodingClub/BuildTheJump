using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterStatus : UI_Base
{
    #region Open
    private Sequence BuildingStatus_Open()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(1.0f, 0.3f).From(0.0f));
    }

    private Sequence Slider_Open()
    {
        var child = Get((int)Children.Slider);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(1.0f, 0.3f).From(0.0f).SetEase(Ease.OutBack));
    }
    #endregion
    #region Close
    private Sequence BuildingStatus_Close()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(0.0f, 0.3f));
    }

    private Sequence Slider_Close()
    {
        var child = Get((int)Children.Slider);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(0.0f, 0.3f));
    }
    #endregion

    private enum Children
    {
        Slider,
        Text_Name,
        Fill_Darkred,
        Fill_Red
    }

    private Image fillDarkred;
    private Image fillRed;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, BuildingStatus_Open, Slider_Open);
        BindSequences(UIState.Close, BuildingStatus_Close, Slider_Close);

        fillDarkred = Get<Image>((int)Children.Fill_Darkred);
        fillRed = Get<Image>((int)Children.Fill_Red);

        GetComponent<Canvas>().worldCamera = Managers.Camera.Main;
        gameObject.SetActive(false);
    }
}