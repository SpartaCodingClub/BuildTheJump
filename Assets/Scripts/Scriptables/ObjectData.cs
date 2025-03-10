using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    // Buildings
    Bonfire,
    Portal,
    Trampoline,

    // Objects
    Tree = 100,
    Rock,
    Monster,

    // Others
    Other = 200
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