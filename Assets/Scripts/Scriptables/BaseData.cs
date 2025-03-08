using UnityEngine;

public abstract class BaseData : ScriptableObject
{
    public ObjectType Type;
    public int ID;

    public string Name;
    public string Description;

    public int HP;
}