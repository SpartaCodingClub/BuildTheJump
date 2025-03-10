using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    #region Inspector
    [SerializeField] protected BaseData data;
    #endregion

    public ObjectType Type { get; private set; }
    public bool IsDead { get { return currentHP == 0; } }

    protected int currentHP;
    protected MeshRenderer meshRenderer;

    private void Awake()
    {
        Type = data.Type;
        currentHP = data.HP;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public virtual void OnInteraction(int damage = 0)
    {
        if (data.HP == 0)
        {
            return;
        }

        currentHP = Mathf.Max(currentHP - damage, 0);

        if (IsDead)
        {
            Managers.Resource.Instantiate(Define.EFFECT_DEATH, transform.position, Define.PATH_EFFECT).GetComponent<ParticleHandler>().Play(meshRenderer);
            Managers.Resource.Destroy(gameObject);
        }
    }
}