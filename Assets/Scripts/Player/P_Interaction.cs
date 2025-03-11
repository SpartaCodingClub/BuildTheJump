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

        // 상호작용 중 움직이면, 상호작용 종료
        Managers.Input.System.Player.Move.performed += OnInteraction;
        Managers.Input.System.Player.Jump.performed += OnInteraction;
    }

    public void InteractionEnter()
    {
        if (isPlayer)
        {
            // 플레이어가 공격 중인 대상이 자원 오브젝트라면, 체력바 UI를 표시
            ResourceObject resourceObject = InteractableObject as ResourceObject;
            if (resourceObject != null)
            {
                // SP가 부족하다면 무시
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
            // 상호작용에 필요한 장비를 장착
            equipment.SetActive(true);
        }
        else
        {
            // 장비가 없다면 즉시 상호작용
            InteractableObject.InteractionEnter();
        }

        Interaction = true;

        animationHandler.SetBool(Define.ID_ACTION, true);
        animationHandler.SetTrigger(InteractableObject.Type);

        // 상호작용이 시작되었다면, 플레이어를 일시정지
        OnInteractionEnter?.Invoke();
    }

    private void InteractionExit()
    {
        // 애니메이션 이벤트 종료 시점과 플레이어가 상호작용 중 움직이면, 호출될 수 있음
        if (Interaction == false)
        {
            return;
        }

        // 상호작용 장비를 해제
        GameObject equipment = equipments[(int)InteractableObject.Type];
        if (equipment != null)
        {
            equipment.SetActive(false);
        }

        Interaction = false;
        animationHandler.SetBool(Define.ID_ACTION, false);

        // 체력바 UI를 숨김
        InteractableObject.InteractionExit(isPlayer);
    }
}