using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;
using Random = UnityEngine.Random;

public class P_Interaction : MonoBehaviour
{
    private readonly int DAMAGE = 20;
    private readonly int SP = 10;

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
            Managers.Game.SetSP(-SP);
        }

        Vector3 position = transform.position + transform.forward * P_InteractionFinder.RADIUS;
        position.x += Random.Range(-0.5f, 0.5f);
        position.y += height;
        position.z += Random.Range(-0.5f, 0.5f);
        Managers.Resource.Instantiate(Define.EFFECT_INTERACTION, position, Define.PATH_EFFECT);

        InteractableObject.InteractionEnter(DAMAGE);
    }

    private void InteractExit()
    {
        if (InteractableObject.IsDead)
        {
            InteractionExit();
        }

        if (isPlayer && Managers.Game.CurrentSP < SP)
        {
            InteractionExit();
        }
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

        // ��ȣ�ۿ� �� �����̸�, ��ȣ�ۿ� ����
        Managers.Input.System.Player.Move.performed += OnInteraction;
        Managers.Input.System.Player.Jump.performed += OnInteraction;
    }

    public void InteractionEnter()
    {
        if (isPlayer)
        {
            // �÷��̾ ���� ���� ����� �ڿ� ������Ʈ���, ü�¹� UI�� ǥ��
            ResourceObject resourceObject = InteractableObject as ResourceObject;
            if (resourceObject != null)
            {
                // SP�� �����ϴٸ� ����
                if (Managers.Game.CurrentSP < SP)
                {
                    return;
                }

                resourceObject.Open_ObjectStatusUI();
            }
        }

        GameObject equipment = equipments[(int)InteractableObject.Type];
        if (equipment != null)
        {
            // ��ȣ�ۿ뿡 �ʿ��� ��� ����
            equipment.SetActive(true);
        }
        else
        {
            // ��� ���ٸ� ��� ��ȣ�ۿ�
            InteractableObject.InteractionEnter();
        }

        Interaction = true;

        animationHandler.SetBool(Define.ID_ACTION, true);
        animationHandler.SetTrigger(InteractableObject.Type);

        // ��ȣ�ۿ��� ���۵Ǿ��ٸ�, �÷��̾ �Ͻ�����
        OnInteractionEnter?.Invoke();
    }

    private void InteractionExit()
    {
        // �ִϸ��̼� �̺�Ʈ ���� ������ �÷��̾ ��ȣ�ۿ� �� �����̸�, ȣ��� �� ����
        if (Interaction == false)
        {
            return;
        }

        // ��ȣ�ۿ� ��� ����
        GameObject equipment = equipments[(int)InteractableObject.Type];
        if (equipment != null)
        {
            equipment.SetActive(false);
        }

        Interaction = false;
        animationHandler.SetBool(Define.ID_ACTION, false);

        // ü�¹� UI�� ����
        InteractableObject.InteractionExit(isPlayer);
    }
}