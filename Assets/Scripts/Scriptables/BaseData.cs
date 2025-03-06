using UnityEngine;

[CreateAssetMenu(fileName = "BaseData", menuName = "Scriptable Objects/BaseData")]
public class BaseData : ScriptableObject
{
    public int ID;

    public string Name;
    public string Description;
}