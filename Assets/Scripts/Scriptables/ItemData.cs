using System;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Others
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Hero,
    Legendary
}

[Serializable]
public class Item
{
    public ItemData Data;
    public int Count;
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : BaseData
{
    public ItemType Type;
    public ItemRarity Rarity;

    public float Weight;
}