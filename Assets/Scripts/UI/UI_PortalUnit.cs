using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PortalUnit : UI_Base, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private enum Children
    {
        Icon,
        Text_Name,
        Text_Description
    }

    private UnitData data;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));
    }

    public void UpdateUI(BaseData unitData)
    {
        data = unitData as UnitData;

        Get<Image>((int)Children.Icon).sprite = Managers.Resource.GetSprite(SpriteType.Unit, unitData.ID.ToString());
        Get<TMP_Text>((int)Children.Text_Name).text = unitData.Name;
        Get<TMP_Text>((int)Children.Text_Description).text = unitData.Description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.1f, 0.2f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UI_Portal portalUI = Managers.UI.CurrentMenuUI as UI_Portal;
        if (portalUI == null)
        {
            return;
        }

        if (portalUI.ID == data.ID)
        {
            return;
        }

        portalUI.UpdateUI(data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1.0f, 0.2f);
    }
}