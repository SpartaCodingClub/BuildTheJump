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
    private Sprite originalSprite;

    private bool activeSelf;
    private int id;

    protected override void Initialize()
    {
        base.Initialize();
        BindChildren(typeof(Children));

        frame = GetComponent<Image>();
        originalSprite = frame.sprite;
    }

    public void SetActive(bool value)
    {
        if (value == false)
        {
            frame.sprite = originalSprite;
        }

        Get((int)Children.Icon).gameObject.SetActive(value);
        Get((int)Children.Text_Weight).gameObject.SetActive(value);
        Get((int)Children.Text_Count).gameObject.SetActive(value);

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

        Get<Image>((int)Children.Icon).sprite = Managers.Resource.GetSprite(SpriteType.Item, item.Data.ID.ToString());
        Get<TMP_Text>((int)Children.Text_Weight).text = $"{Managers.Item.GetWeights(item.Data.ID):N1}g";
        Get<TMP_Text>((int)Children.Text_Count).text = item.Count.ToString();

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
            inventoryUI.FocusUI.UpdateUI(transform);
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