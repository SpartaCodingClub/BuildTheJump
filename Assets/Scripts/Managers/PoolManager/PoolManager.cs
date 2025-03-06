using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private Transform transform;

    private readonly Dictionary<string, Pool> pools = new();

    public void Initialize()
    {
        transform = new GameObject(nameof(PoolManager)).transform;
        transform.SetParent(Managers.Instance.transform);
    }

    public void Push(Poolable poolable)
    {
        string key = poolable.name;
        if (pools.TryGetValue(key, out var pool) == false)
        {
            pool = new(poolable, transform);
            pools.Add(key, pool);
        }

        pool.Push(poolable);
    }

    public GameObject TryPop(string key)
    {
        if (pools.TryGetValue(key, out var pool) == false)
        {
            return null;
        }

        return pool.Pop();
    }
}