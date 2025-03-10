using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AnimationHandler))]
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
    private void OnInteractionExit(InputAction.CallbackContext callbackContext)
    {
        InteractionExit();
    }
    #endregion

    public event Action OnInteractionEnter;

    public bool Interaction { get; private set; }

    private readonly GameObject[] equipments = new GameObject[(int)ObjectType.Other];

    private bool isPlayer;
    private AnimationHandler animationHandler;

    private void Awake()
    {
        equipments[(int)ObjectType.Tree] = gameObject.FindComponent<Transform>("Axe").gameObject;
        equipments[(int)ObjectType.Rock] = gameObject.FindComponent<Transform>("Pickaxe").gameObject;

        isPlayer = GetComponent<P_Worker>() == null;
        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        if (isPlayer == false)
        {
            return;
        }

        Managers.Input.System.Player.Move.performed += OnInteractionExit;
        Managers.Input.System.Player.Jump.performed += OnInteractionExit;
    }

    public void InteractionEnter()
    {
        Interaction = true;
        animationHandler.SetBool(Define.ID_ACTION, true);
        animationHandler.SetTrigger(InteractableObject.Type);

        if (isPlayer)
        {
            InteractableObject.Open_ObjectStatusUI();
        }

        GameObject equipment = equipments[(int)InteractableObject.Type];
        if (equipment != null)
        {
            equipment.SetActive(true);
        }
        else
        {
            InteractableObject.OnInteraction();
        }

        OnInteractionEnter?.Invoke();
    }

    private void InteractionExit()
    {
        if (Interaction == false)
        {
            return;
        }

        Interaction = false;
        InteractableObject.Close_ObjectStatusUI();
        animationHandler.SetBool(Define.ID_ACTION, false);

        GameObject equipment = equipments[(int)InteractableObject.Type];
        if (equipment != null)
        {
            equipment.SetActive(false);
        }
    }
}