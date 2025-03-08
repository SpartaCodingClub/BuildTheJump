using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : UI_Base
{
    #region Open
    private Sequence Frame_Open()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(1.0f, 0.2f).From(0.0f).SetEase(Ease.OutBack));
    }
    #endregion

    private enum Children
    {
        Frame,
        Icon,
        Text_Name,
        Text_Description
    }

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, Frame_Open);

        gameObject.SetActive(false);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void UpdateUI(DataType type, int id, Vector2 screenPosition)
    {
        Open();

        RectTransform frame = Get((int)Children.Frame);
        frame.pivot = GetPivot(screenPosition);
        frame.anchoredPosition = screenPosition;

        BaseData data = Managers.Data.GetData(type, id);
        Get<Image>((int)Children.Icon).sprite = Managers.Resource.GetSprite((SpriteType)type, id.ToString());
        Get<TMP_Text>((int)Children.Text_Name).text = data.Name;
        Get<TMP_Text>((int)Children.Text_Description).text = data.Description;

        gameObject.SetActive(true);
    }

    private Vector2 GetPivot(Vector2 screenPosition)
    {
        float x = screenPosition.x > Screen.width * 0.5f ? 1.0f : 0.0f;
        float y = screenPosition.y > Screen.height * 0.5f ? 1.0f : 0.0f;
        return new(x, y);
    }
}