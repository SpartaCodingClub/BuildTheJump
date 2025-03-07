using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public Dictionary<int, Item> Inventory { get; private set; } = new();

    public void AddDropItem(Item dropItem)
    {
        int key = dropItem.Data.ID;
        if (Inventory.TryGetValue(key, out var item))
        {
            item.Count += dropItem.Count;
            return;
        }

        Inventory.Add(key, dropItem);
    }

    public List<Item> GetDropItems(List<DropRow> dropTable)
    {
        List<Item> dropItems = new();
        foreach (var dropRow in dropTable)
        {
            if (dropRow.Percent < Random.Range(0.0f, 100.0f))
            {
                continue;
            }

            int count = Random.Range(1, dropRow.Count);
            Item dropItem = new() { Data = dropRow.Data, Count = count };
            dropItems.Add(dropItem);
        }

        return dropItems;
    }

    public float GetWeight(int key)
    {
        if (Inventory.TryGetValue(key, out var item) == false)
        {
            return 0.0f;
        }

        return item.Data.Weight * item.Count;
    }
}