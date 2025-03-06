using UnityEngine;
using UnityEngine.Pool;

public class Pool
{
    private readonly Transform transform;
    private readonly GameObject original;
    private readonly IObjectPool<GameObject> objectPool;

    public Pool(Poolable poolable, Transform parent)
    {
        transform = new GameObject(poolable.name).transform;
        transform.SetParent(parent);

        string pathType = Define.PATH_OBJECT;
        switch (poolable.name)
        {
            case var name when name.Contains("Effect"):
                pathType = Define.PATH_EFFECT;
                break;
            case var name when name.Contains("UI"):
                pathType = Define.PATH_UI;
                break;
        }

        original = Resources.Load<GameObject>($"{pathType}/{poolable.name}");
        objectPool = new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    public void Push(Poolable poolable)
    {
        objectPool.Release(poolable.gameObject);
    }

    public GameObject Pop()
    {
        return objectPool.Get();
    }

    private GameObject CreateFunc()
    {
        return Managers.Resource.Instantiate(original);
    }

    private void ActionOnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    private void ActionOnRelease(GameObject gameObject)
    {
        gameObject.transform.SetParent(transform);
        gameObject.SetActive(false);
    }

    private void ActionOnDestroy(GameObject gameObject)
    {
        Object.Destroy(gameObject);
    }
}