using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    #region Open
    private Sequence Background_Open()
    {
        var graphic = Get<Graphic>((int)Children.Background);

        return Utility.RecyclableSequence()
            .Append(graphic.DOFade(0.8f, 0.3f).From(0.0f));
    }

    private Sequence Frame_Open()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOAnchorPosX(1080.0f, 0.3f).From().SetEase(Ease.OutBack));
    }
    #endregion
    #region Close
    private Sequence Background_Close()
    {
        var graphic = Get<Graphic>((int)Children.Background);

        return Utility.RecyclableSequence()
            .Append(graphic.DOFade(0.0f, 0.3f));
    }

    private Sequence Frame_Close()
    {
        var child = Get((int)Children.Frame);

        return Utility.RecyclableSequence()
            .Append(child.DOAnchorPosX(1080.0f, 0.3f).SetEase(Ease.OutExpo));
    }
    #endregion

    private enum Children
    {
        Background,
        Frame,
        Content,
        Fill,
        Text_Weights
    }

    public UI_Focus FocusUI { get; private set; }

    private UI_InventoryItem[] content;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, Background_Open, Frame_Open);
        BindSequences(UIState.Close, Background_Close, Frame_Close);

        FocusUI = gameObject.FindComponent<UI_Focus>();
        content = Get((int)Children.Content).GetComponentsInChildren<UI_InventoryItem>();
    }

    public override void Open()
    {
        base.Open();
        UpdateUI();
    }

    public override void Close()
    {
        base.Close();

        FocusUI.UpdateUI(null);
        Managers.UI.PopupUI.Close();
    }

    public void UpdateUI()
    {
        foreach (var inventoryItem in content)
        {
            inventoryItem.UpdateUI(null);
        }

        int index = 0;
        float weight = 0.0f;
        foreach (var item in Managers.Item.Inventory)
        {
            content[index++].UpdateUI(item.Value);
            weight += Managers.Item.GetWeights(item.Key);
        }

        float fillAmount = weight / Define.MAX_WEIGHT;
        Get<Image>((int)Children.Fill).fillAmount = fillAmount;
        Get<TMP_Text>((int)Children.Text_Weights).text = $"무게 <size=30>({weight:N1}g / {Define.MAX_WEIGHT:N1}g)</size>";
    }
}