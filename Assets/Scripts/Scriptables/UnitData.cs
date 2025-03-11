using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : BaseData
{
    [Space(20)]
    public string DescriptionUnit;
    public float Duration;

    public float actionSpeed = 1.0f;
    public float moveSpeed = Define.WORKER_MOVE_SPEED;

    public List<Item> Items = new();
}