using DG.Tweening;
using UnityEngine;

public class UI_Key : UI_Base
{
    #region Open
    private Sequence Key_Open()
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
    private Sequence Key_Close()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(0.0f, 0.3f));
    }

    private Sequence Frame_Close()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(0.0f, 0.3f));
    }
    #endregion

    private enum Children
    {
        Frame,
        Text_Description
    }

    private RectTransform frame;
    private RectTransform description;
    private float descriptionOffsetY;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, Key_Open, Frame_Open);
        BindSequences(UIState.Close, Key_Close, Frame_Close);

        frame = Get<RectTransform>((int)Children.Frame);
        description = Get<RectTransform>((int)Children.Text_Description);
        descriptionOffsetY = description.position.y - frame.position.y;
    }

    public void UpdateUI(Transform target)
    {
        Vector3 worldPosition = new(target.position.x, target.position.y + 1.5f, target.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        frame.position = screenPosition;

        screenPosition.y += descriptionOffsetY;
        description.position = screenPosition;
    }
}