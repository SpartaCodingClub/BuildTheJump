using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Scriptable Objects/BuildingData")]
public class BuildingData : BaseData
{
    [Space(20)]
    public string NameBuilding;
    public float Duration;

    public List<Item> Items = new();
}