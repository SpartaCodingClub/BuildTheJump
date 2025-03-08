using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventoryItem : UI_Base, IPointerEnterHandler, IPointerExitHandler
{
    private enum Children
    {
        Icon,
        Text_Weight,
        Text_Count
    }

    private Image frame;
    private Image icon;
    private RectTransform rectTransform;
    private Sprite originalSprite;
    private TMP_Text textWeight;
    private TMP_Text textCount;

    private bool activeSelf;
    private int id;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        frame = GetComponent<Image>();
        icon = Get<Image>((int)Children.Icon);
        rectTransform = GetComponent<RectTransform>();
        originalSprite = frame.sprite;
        textWeight = Get<TMP_Text>((int)Children.Text_Weight);
        textCount = Get<TMP_Text>((int)Children.Text_Count);
    }

    public void SetActive(bool value)
    {
        if (value == false)
        {
            frame.sprite = originalSprite;
        }

        icon.gameObject.SetActive(value);
        textWeight.gameObject.SetActive(value);
        textCount.gameObject.SetActive(value);
        activeSelf = value;
    }

    public void UpdateUI(Item item)
    {
        if (item == null)
        {
            SetActive(false);
            return;
        }

        id = item.Data.ID;
        frame.sprite = Managers.Resource.GetSprite(SpriteType.Rarity, item.Data.Rarity.ToString());
        icon.sprite = Managers.Resource.GetSprite(SpriteType.Item, item.Data.ID.ToString());
        textWeight.text = $"{Managers.Item.GetWeights(item.Data.ID):N1}g";
        textCount.text = item.Count.ToString();

        SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (activeSelf == false)
        {
            return;
        }

        UI_Inventory inventoryUI = (Managers.UI.CurrentMenuUI as UI_Inventory);
        if (inventoryUI != null)
        {
            inventoryUI.FocusUI.UpdateUI(rectTransform);
        }

        Managers.UI.PopupUI.UpdateUI(DataType.Item, id, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_Inventory inventoryUI = Managers.UI.CurrentMenuUI as UI_Inventory;
        if (inventoryUI == null)
        {
            return;
        }

        inventoryUI.FocusUI.UpdateUI(null);
        Managers.UI.PopupUI.Close();
    }
}