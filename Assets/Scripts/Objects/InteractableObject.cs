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

    // 공격 애니메이션 이벤트 시점에서 호출됩니다.
    public virtual void InteractionEnter(int damage = 0)
    {
        // 체력이 없는(무적) 오브젝트라면, 오브젝트가 죽지 않음
        if (baseData.HP == 0)
        {
            return;
        }

        // 데미지 텍스트
        int amount = currentHP - Mathf.Max(currentHP - damage, 0);
        currentHP -= amount;
        Managers.UI.Open<UI_FloatingText>().UpdateUI(amount.ToString(), transform.position + Vector3.right * 3.0f, Color.red);

        // 오브젝트 사망 이펙트
        if (IsDead)
        {
            Managers.Resource.Instantiate(Define.EFFECT_DEATH, transform.position, Define.PATH_EFFECT).GetComponent<ParticleHandler>().Play(meshRenderer);
            Managers.Resource.Destroy(gameObject);
        }
    }

    // 공격 애니메이션 이벤트 종료 시점에서 호출됩니다.
    public virtual void InteractionExit(bool isPlayer) { }
}