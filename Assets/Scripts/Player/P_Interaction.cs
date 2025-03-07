using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AnimationHandler))]
public class P_Interaction : MonoBehaviour
{
    private readonly int DAMAGE = 20;

    #region AnimationEvents
    private void InteractEnter(float height)
    {
        Vector3 position = InteractableObject.transform.position;
        position.x += Random.Range(-0.5f, 0.5f);
        position.y += height;
        position.z += Random.Range(-0.5f, 0.5f);
        Managers.Resource.Instantiate(Define.EFFECT_INTERACTION, position, Define.PATH_EFFECT);

        InteractableObject.OnInteraction(DAMAGE);
    }

    private void InteractExit()
    {
        if (InteractableObject.IsDead == false)
        {
            return;
        }

        InteractionExit();
    }
    #endregion
    #region InputSystem
    private void OnInteraction(InputAction.CallbackContext callbackContext)
    {
        InteractionExit();
    }
    #endregion

    public event Action OnInteractionEnter;
    public event Action OnInteractionExit;

    public bool Interaction { get; private set; }
    public InteractableObject InteractableObject { get; set; }

    private readonly GameObject[] equipments = new GameObject[(int)ObjectType.Count];

    private AnimationHandler animationHandler;

    private void Awake()
    {
        equipments[(int)ObjectType.Tree] = gameObject.FindComponent<Transform>("Axe").gameObject;
        equipments[(int)ObjectType.Rock] = gameObject.FindComponent<Transform>("Pickaxe").gameObject;

        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        Managers.Input.System.Player.Move.performed += OnInteraction;
        Managers.Input.System.Player.Jump.performed += OnInteraction;
    }

    public void InteractionEnter()
    {
        Interaction = true;
        InteractableObject.Open_StatusBarUI();
        equipments[(int)InteractableObject.Type].SetActive(true);
        animationHandler.SetBool(Define.ID_ACTION, true);
        animationHandler.SetTrigger(InteractableObject.Type);

        OnInteractionEnter?.Invoke();
    }

    private void InteractionExit()
    {
        if (Interaction == false)
        {
            return;
        }

        Interaction = false;
        InteractableObject.Close_StatusBarUI();
        equipments[(int)InteractableObject.Type].SetActive(false);
        animationHandler.SetBool(Define.ID_ACTION, false);

        OnInteractionExit?.Invoke();
    }
}