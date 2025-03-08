using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Subitem : UI_Base
{
    private enum Children
    {
        Icon,
        Text_Value
    }

    public bool UpdateUI(Item item)
    {
        BindChildren(typeof(Children));

        int id = item.Data.ID;
        Get<Image>((int)Children.Icon).sprite = Managers.Resource.GetSprite(SpriteType.Item, id.ToString());

        int count = Managers.Item.Inventory.TryGetValue(id, out var invetoryItem) ? invetoryItem.Count : 0;
        TMP_Text textValue = Get<TMP_Text>((int)Children.Text_Value);
        textValue.color = count >= item.Count ? Color.green : Color.red;
        textValue.text = $"{count} / {item.Count}";

        return count >= item.Count;
    }
}