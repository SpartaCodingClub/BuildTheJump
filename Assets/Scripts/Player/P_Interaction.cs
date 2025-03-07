using UnityEngine;

public delegate void Interaction();

[RequireComponent(typeof(AnimationHandler))]
public class P_Interaction : MonoBehaviour
{
    private readonly int DAMAGE = 20;

    #region AnimationEvents
    private void Interact(float height)
    {
        Vector3 position = InteractableObject.transform.position;
        position.x += Random.Range(-0.5f, 0.5f);
        position.y += height;
        position.z += Random.Range(-0.5f, 0.5f);
        Managers.Resource.Instantiate(Define.EFFECT_HIT, position, Define.PATH_EFFECT);

        InteractableObject.OnInteraction(DAMAGE);
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
        animationHandler.SetBool(Define.ID_ACTION, true);
        animationHandler.SetTrigger(InteractableObject.Type);

        OnInteractionEnter?.Invoke();
    }

    public void InteractionExit()
    {
        OnInteraction = false;
        InteractableObject.Close_StatusBarUI();
        equipments[(int)InteractableObject.Type].SetActive(false);
        animationHandler.SetBool(Define.ID_ACTION, false);

        OnInteractionExit?.Invoke();
    }
}