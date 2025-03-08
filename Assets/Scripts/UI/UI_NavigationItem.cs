using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_NavigationItem : UI_Base
{
    #region Open
    private Sequence Mask_Open()
    {
        var child = Get((int)Children.Mask);
        var graphic = child.GetComponent<Graphic>();

        return Utility.RecyclableSequence()
            .Append(child.DOScale(1.5f, 0.3f))
            .Append(graphic.DOFade(0.0f, 0.3f))
            .AppendInterval(1.5f)
            .AppendCallback(Close);
    }
    #endregion
    #region Close
    private Sequence NavigationItem_Close()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(0.0f, 0.5f));
    }
    #endregion

    private enum Children
    {
        Frame,
        Icon,
        Mask,
        Text_Description
    }

    private Sprite originalSprite;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, Mask_Open);
        BindSequences(UIState.Close, NavigationItem_Close);

        originalSprite = Get<Image>((int)Children.Frame).sprite;
    }

    public override void Open()
    {
        base.Open();
        canvasGroup.alpha = 1.0f;
    }

    public void UpdateUI(Item dropItem)
    {
        Get<Image>((int)Children.Frame).sprite = Managers.Resource.GetSprite(SpriteType.Rarity, dropItem.Data.Rarity.ToString());
        Get<Image>((int)Children.Icon).sprite = Managers.Resource.GetSprite(SpriteType.Item, dropItem.Data.ID.ToString());

        TMP_Text textDescription = Get<TMP_Text>((int)Children.Text_Description);
        textDescription.color = Color.white;
        textDescription.text = $"{dropItem.Data.Name} x{dropItem.Count}";
    }

    public void UpdateUI(string id, string description)
    {
        Get<Image>((int)Children.Frame).sprite = originalSprite;
        Get<Image>((int)Children.Icon).sprite = Managers.Resource.GetSprite(SpriteType.Building, id.ToString());

        TMP_Text textDescription = Get<TMP_Text>((int)Children.Text_Description);
        textDescription.color = Define.SKYBLUE;
        textDescription.text = description;
    }
}