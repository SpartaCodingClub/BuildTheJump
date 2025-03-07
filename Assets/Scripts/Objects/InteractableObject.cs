using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    #region Inspector
    [SerializeField] protected ObjectData data;
    #endregion
    #region UI_StatusBar
    private UI_StatusBar statusBarUI;

    public void Open_StatusBarUI()
    {
        if (statusBarUI == null)
        {
            statusBarUI = Managers.UI.Open<UI_StatusBar>();
        }

        statusBarUI.UpdateUI(currentHP, data);
    }

    public void Close_StatusBarUI()
    {
        if (statusBarUI == null)
        {
            return;
        }

        statusBarUI.Close();
        statusBarUI = null;
    }
    #endregion

    public ObjectType Type { get; private set; }
    public bool IsDead { get { return currentHP == 0; } }

    private int currentHP;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        Type = data.Type;
        currentHP = data.HP;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public virtual void OnInteraction(int damage)
    {
        currentHP = Mathf.Max(currentHP - damage, 0);

        if (IsDead)
        {
            Managers.Resource.Instantiate(Define.EFFECT_DEATH, transform.position, Define.PATH_EFFECT).GetComponent<ParticleHandler>().Play(meshRenderer);
            Managers.Resource.Destroy(gameObject);
        }

        if (statusBarUI != null)
        {
            statusBarUI.UpdateUI(currentHP, data);
        }
    }
}