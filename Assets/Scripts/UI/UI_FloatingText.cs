using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_FloatingText : UI_Base
{
    #region Open
    private Sequence Open_InteractionText()
    {
        var child = Get((int)Children.Text_Value);

        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(1.0f, 0.3f).From(0.0f))
            .Join(child.DOScale(1.0f, 0.3f).From(3.0f))
            .Append(child.DOAnchorPosY(200.0f, 1.3f).SetRelative(true))
            .JoinCallback(Close);
    }
    #endregion
    #region Close
    private Sequence Close_InteractionText()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(0.0f, 0.3f).SetDelay(1.0f));
    }
    #endregion

    private enum Children
    {
        Text_Value
    }

    private Sequence open;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Close, Close_InteractionText);

        open = Open_InteractionText();
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
        open.Kill();
    }

    public void UpdateUI(string value, Vector3 position, Color color)
    {
        transform.position = position;

        RectTransform child = Get((int)Children.Text_Value);
        child.anchoredPosition = new(Random.Range(-50.0f, 50.0f), Random.Range(200.0f, 250.0f));

        TMP_Text textValue = child.GetComponent<TMP_Text>();
        textValue.color = color;
        textValue.text = value;

        open.Restart();
    }
}