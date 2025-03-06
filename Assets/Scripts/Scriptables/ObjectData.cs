using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Tree,
    Rock,
    Count
}

[System.Serializable]
public class DropRow
{
    public ItemData Data;

    [Range(0.0f, 100.0f)]
    public float Percent;

    [Range(0, 100)]
    public int MaxCount;
}

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class ObjectData : ScriptableObject
{
    public ObjectType Type;

    public string Name;
    public string Description;

    public int HP;

    public List<DropRow> DropTable = new();
}