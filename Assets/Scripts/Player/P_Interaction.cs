using UnityEngine;

public delegate void Interaction();

public class P_Interaction : MonoBehaviour
{
    #region Inspector
    [SerializeField, Min(0.0f)] private int damage;
    #endregion
    #region AnimationEvents
    private void Attack()
    {
        InteractableObject.OnHit(damage);

        Vector3 position = new(
            InteractableObject.transform.position.x + Random.Range(-0.5f, 0.5f),
            InteractableObject.transform.position.y + 1.5f,
            InteractableObject.transform.position.z + Random.Range(-0.5f, 0.5f));

        Managers.Resource.Instantiate(Define.EFFECT_HIT, position, Define.PATH_EFFECT);
    }
    #endregion

    public event Interaction OnInteractionEnter;
    public event Interaction OnInteractionExit;

    public bool OnInteraction { get; private set; }
    public InteractableObject InteractableObject { get; set; }

    private readonly GameObject[] equipments = new GameObject[(int)ObjectType.Count];

    private AnimationHandler animationHandler;

    private void Awake()
    {
        equipments[(int)ObjectType.Tree] = gameObject.FindComponent<Transform>("Axe").gameObject;
        equipments[(int)ObjectType.Rock] = gameObject.FindComponent<Transform>("Pickaxe").gameObject;

        animationHandler = GetComponent<AnimationHandler>();
    }

    public void InteractionEnter()
    {
        OnInteraction = true;
        InteractableObject.Open_StatusBarUI();
        equipments[(int)InteractableObject.Type].SetActive(true);
        animationHandler.SetTrigger(InteractableObject.Type);

        OnInteractionEnter?.Invoke();
    }

    public void InteractionExit()
    {
        OnInteraction = false;
        InteractableObject.Close_StatusBarUI();
        equipments[(int)InteractableObject.Type].SetActive(false);

        OnInteractionExit?.Invoke();
    }
}