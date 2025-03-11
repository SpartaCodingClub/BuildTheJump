using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    #region Inspector
    [SerializeField] protected BaseData baseData;
    #endregion

    public ObjectType Type { get; private set; }
    public bool IsDead { get { return currentHP == 0; } }

    protected int currentHP;
    protected MeshRenderer meshRenderer;

    private void Awake()
    {
        Type = baseData.Type;

        currentHP = baseData.HP;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    // ���� �ִϸ��̼� �̺�Ʈ �������� ȣ��˴ϴ�.
    public virtual void InteractionEnter(int damage = 0)
    {
        // ü���� ����(����) ������Ʈ���, ������Ʈ�� ���� ����
        if (baseData.HP == 0)
        {
            return;
        }

        // ������Ʈ ��� ����Ʈ
        currentHP = Mathf.Max(currentHP - damage, 0);
        if (IsDead)
        {
            Managers.Resource.Instantiate(Define.EFFECT_DEATH, transform.position, Define.PATH_EFFECT).GetComponent<ParticleHandler>().Play(meshRenderer);
            Managers.Resource.Destroy(gameObject);
        }
    }

    // ���� �ִϸ��̼� �̺�Ʈ ���� �������� ȣ��˴ϴ�.
    public virtual void InteractionExit(bool isPlayer) { }
}