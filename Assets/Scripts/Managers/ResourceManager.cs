using System;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

public enum SpriteType
{
    Item,
    Building,
    Rarity,
    Unit,
    Count
}

public class ResourceManager
{
    private readonly SpriteAtlas[] atlas = new SpriteAtlas[(int)SpriteType.Count];

    public Sprite GetSprite(SpriteType type, string name) => atlas[(int)type].GetSprite(name);

    public void Initialize()
    {
        string[] names = Enum.GetNames(typeof(SpriteType));
        for (int i = 0; i < atlas.Length; i++)
        {
            SpriteAtlas atlas = Resources.Load<SpriteAtlas>($"{Define.PATH_ATLAS}/Atlas_{names[i]}");
            this.atlas[i] = atlas;
        }
    }

    public GameObject Instantiate(string key, Vector3 position, string pathType = Define.PATH_OBJECT)
    {
        GameObject gameObject = Managers.Pool.TryPop(key);
        if (gameObject == null)
        {
            string path = $"{pathType}/{key}";
            GameObject original = Resources.Load<GameObject>(path);
            if (original == null)
            {
                Debug.LogWarning($"Failed to Load<GameObject>({path})");
                return null;
            }

            gameObject = Instantiate(original);
        }

        gameObject.transform.position = position;
        return gameObject;
    }

    public GameObject Instantiate(GameObject original)
    {
        GameObject gameObject = Object.Instantiate(original);
        gameObject.name = original.name;
        return gameObject;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Poolable>(out var poolable))
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(gameObject);
    }
}