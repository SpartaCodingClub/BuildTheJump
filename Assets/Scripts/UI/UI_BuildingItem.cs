using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BuildingItem : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    #region Open
    private Sequence BuildingItem_Open()
    {
        return Utility.RecyclableSequence()
            .Append(canvasGroup.DOFade(1.0f, 0.3f).From(0.0f).SetDelay(0.3f));
    }

    private Sequence Icon_Open()
    {
        var child = Get((int)Children.Icon);

        return Utility.RecyclableSequence()
            .Append(child.DOScale(1.0f, 0.3f).From(0.0f).SetEase(Ease.OutBack));
    }
    #endregion
    #region UI_BuildingBoard
    private void Close_BuildingBoardUI()
    {
        if (BuildingBoardUI == null)
        {
            return;
        }

        BuildingBoardUI.Close();
        BuildingBoardUI = null;
    }
    #endregion

    private enum Children
    {
        Icon,
        Name
    }

    public static UI_BuildingBoard BuildingBoardUI { get; private set; }

    private BaseData data;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        BindSequences(UIState.Open, BuildingItem_Open, Icon_Open);
    }

    public override void Close()
    {
        Close_BuildingBoardUI();
    }

    public void UpdateUI(BaseData data)
    {
        Get<Image>((int)Children.Icon).sprite = Managers.Resource.GetSprite(SpriteType.Building, data.ID.ToString());
        Get<TMP_Text>((int)Children.Name).text = $"{data.Name} 설치";

        this.data = data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        (Managers.UI.CurrentMenuUI as UI_Building).FocusUI.UpdateUI(rectTransform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_Building buildingUI = Managers.UI.CurrentMenuUI as UI_Building;
        if (buildingUI == null)
        {
            return;
        }

        buildingUI.FocusUI.UpdateUI(null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (BuildingBoardUI != null)
        {
            if (BuildingBoardUI.ID == data.ID)
            {
                return;
            }

            Close_BuildingBoardUI();
        }

        BuildingBoardUI = Managers.UI.Open<UI_BuildingBoard>();
        BuildingBoardUI.UpdateUI(data);
    }
}