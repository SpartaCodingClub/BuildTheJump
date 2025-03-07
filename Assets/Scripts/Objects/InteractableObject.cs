using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    #region Inspector
    [SerializeField] protected ObjectData data;
    #endregion
    #region UI_StatusBar
    public void Open_StatusBarUI()
    {
        statusBarUI = Managers.UI.Open<UI_StatusBar>();
        statusBarUI.UpdateUI(hp, data);
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

    public bool IsDead { get { return hp == 0; } }

    public ObjectType Type;
    private int hp;

    private MeshRenderer meshRenderer;
    private UI_StatusBar statusBarUI;

    private void Start()
    {
        Type = data.Type;
        hp = data.HP;

        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public virtual void OnInteraction(int damage)
    {
        hp = Mathf.Max(hp - damage, 0);
        if (hp == 0)
        {
            GameObject gameObject = Managers.Resource.Instantiate(Define.EFFECT_DEATH, transform.position, Define.PATH_EFFECT);
            gameObject.GetComponent<ParticleHandler>().Play(meshRenderer);

            Managers.Game.Interaction.InteractionExit();
            Managers.Resource.Destroy(this.gameObject);
        }

        if (statusBarUI != null)
        {
            statusBarUI.UpdateUI(hp, data);
        }
    }
}