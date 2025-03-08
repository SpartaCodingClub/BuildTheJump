using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    // Buildings
    Bonfire,
    Portal,

    // Objects
    Tree,
    Rock,

    // Others
    Other
}

[System.Serializable]
public class DropRow
{
    public ItemData Data;

    [Range(0, 10)]
    public int Count;

    [Range(0.0f, 100.0f)]
    public float Percent;
}

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class ObjectData : BaseData
{
    [Space(20)]
    public List<DropRow> DropTable = new();
}