using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : BaseData
{
    [Space(20)]
    public float Duration;
    public string DescriptionUnit;

    public List<Item> Items = new();
}