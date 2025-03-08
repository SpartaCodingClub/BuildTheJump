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
    public static UI_BuildingBoard BuildingBoardUI { get; private set; }

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
        Text_Name
    }

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
        this.data = data;

        Get<Image>((int)Children.Icon).sprite = Managers.Resource.GetSprite(SpriteType.Building, data.ID.ToString());
        Get<TMP_Text>((int)Children.Text_Name).text = $"{data.Name} 설치";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        UI_Building buildingUI = (Managers.UI.CurrentMenuUI as UI_Building);
        if (buildingUI != null)
        {
            buildingUI.FocusUI.UpdateUI(rectTransform);
        }

        Managers.UI.PopupUI.UpdateUI(DataType.Building, data.ID, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_Building buildingUI = Managers.UI.CurrentMenuUI as UI_Building;
        if (buildingUI == null)
        {
            return;
        }

        buildingUI.FocusUI.UpdateUI(null);
        Managers.UI.PopupUI.Close();
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