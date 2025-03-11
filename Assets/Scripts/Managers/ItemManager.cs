using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public Dictionary<int, Item> Inventory { get; private set; } = new();

    public void AddItem(Item item)
    {
        int key = item.Data.ID;
        if (Inventory.TryGetValue(key, out var inventoryItem))
        {
            inventoryItem.Count += item.Count;
            return;
        }

        Inventory.Add(key, item);
        SetMoveSpeed();
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

            int count = Random.Range(1, dropRow.Count + 1);
            Item dropItem = new() { Data = dropRow.Data, Count = count };
            dropItems.Add(dropItem);
        }

        return dropItems;
    }

    public float GetWeights(int key)
    {
        if (Inventory.TryGetValue(key, out var item) == false)
        {
            return 0.0f;
        }

        return item.Data.Weight * item.Count;
    }

    private void SetMoveSpeed()
    {
        float totalWeights = 0.0f;
        foreach (var item in Inventory)
        {
            totalWeights += GetWeights(item.Key);
        }

        Managers.Game.Player.SetMoveSpeed(totalWeights >= Define.MAX_WEIGHT ? Define.WORKER_MOVE_SPEED : Define.PLAYER_MOVE_SPEED);
    }
}