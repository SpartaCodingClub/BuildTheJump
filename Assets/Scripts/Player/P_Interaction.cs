using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;
using Random = UnityEngine.Random;

public class P_Interaction : MonoBehaviour
{
    private readonly int DAMAGE = 20;

    #region Inspector
    [ShowInInspector, ReadOnly]
    public InteractableObject InteractableObject { get; set; }
    #endregion
    #region AnimationEvents
    private void InteractEnter(float height)
    {
        if (InteractableObject == null)
        {
            return;
        }

        if (isPlayer)
        {
            Managers.Camera.Shake();
        }

        Vector3 position = InteractableObject.transform.position;
        position.x += Random.Range(-0.5f, 0.5f);
        position.y += height;
        position.z += Random.Range(-0.5f, 0.5f);
        Managers.Resource.Instantiate(Define.EFFECT_INTERACTION, position, Define.PATH_EFFECT);

        InteractableObject.InteractionEnter(DAMAGE);
    }

    private void InteractExit()
    {
        if (InteractableObject.IsDead == false)
        {
            return;
        }

        InteractionExit();
    }

    private void Attack()
    {
        // TODO: TEMP CODE
    }
    #endregion
    #region InputSystem
    private void OnInteraction(InputAction.CallbackContext callbackContext)
    {
        InteractionExit();
    }
    #endregion

    public event Action OnInteractionEnter;

    public bool Interaction { get; private set; }

    private bool isPlayer;
    private AnimationHandler animationHandler;

    private readonly GameObject[] equipments = new GameObject[(int)ObjectType.Other];

    private void Awake()
    {
        isPlayer = GetComponent<P_Worker>() == null;
        if (isPlayer)
        {
            equipments[(int)ObjectType.Monster] = gameObject.FindComponent<Transform>("Club").gameObject;
        }

        animationHandler = GetComponent<AnimationHandler>();

        equipments[(int)ObjectType.Tree] = gameObject.FindComponent<Transform>("Axe").gameObject;
        equipments[(int)ObjectType.Rock] = gameObject.FindComponent<Transform>("Pickaxe").gameObject;
    }

    private void Start()
    {
        if (isPlayer == false)
        {
            return;
        }

        Managers.Input.System.Player.Move.performed += OnInteraction;
        Managers.Input.System.Player.Jump.performed += OnInteraction;
    }

    public void InteractionEnter()
    {
        if (isPlayer)
        {
            ResourceObject resourceObject = InteractableObject as ResourceObject;
            if (resourceObject != null) resourceObject.Open_ObjectStatusUI();
        }

        GameObject equipment = equipments[(int)InteractableObject.Type];
        if (equipment != null)
        {
            equipment.SetActive(true);
        }
        else
        {
            InteractableObject.InteractionEnter();
        }

        Interaction = true;

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

        GameObject equipment = equipments[(int)InteractableObject.Type];
        if (equipment != null)
        {
            equipment.SetActive(false);
        }

        Interaction = false;
        animationHandler.SetBool(Define.ID_ACTION, false);

        InteractableObject.InteractionExit(isPlayer);
    }
}