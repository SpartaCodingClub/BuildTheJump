using System.Collections.Generic;
using UnityEngine;

public enum DataType
{
    Item,
    Building,
    Count
}

public class DataManager
{
    private readonly Dictionary<int, BaseData>[] datas = new Dictionary<int, BaseData>[(int)DataType.Count];

    public BaseData GetData(DataType type, int id) => datas[(int)type][id];
    public T GetData<T>(DataType type, int id) where T : BaseData => GetData(type, id) as T;
    public IEnumerable<BaseData> GetDatas(DataType type) => datas[(int)type].Values;

    public void Initialize()
    {
        for (int i = 0; i < datas.Length; i++)
        {
            datas[i] = new();
            LoadData((DataType)i);
        }
    }

    private void LoadData(DataType type)
    {
        switch (type)
        {
            case DataType.Item:
                foreach (var data in Resources.LoadAll<BaseData>(Define.PATH_ITEM)) datas[(int)type].Add(data.ID, data);
                break;
            case DataType.Building:
                foreach (var data in Resources.LoadAll<BaseData>(Define.PATH_BUILDING)) datas[(int)type].Add(data.ID, data);
                break;
        }
    }
}