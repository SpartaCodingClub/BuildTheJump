using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using VInspector;

public enum WorkerState
{
    Idle,
    Move,
    Interact,
    Interaction,
}

public class P_Worker : MonoBehaviour
{
    private readonly WaitForSeconds DELAY = new(1.0f);
    private readonly WaitForSeconds INTERVAL = new(0.2f);

    #region Inspector
    [SerializeField, ReadOnly]
    private WorkerState state;

    [SerializeField, ReadOnly]
    private float radius;

    [SerializeField, ReadOnly]
    private Transform target;
    #endregion

    private readonly Collider[] objectColliders = new Collider[8];

    private LayerMask targetLayer;
    private AnimationHandler animationHandler;
    private NavMeshAgent navMeshAgent;
    private P_Interaction interaction;

    private void Awake()
    {
        targetLayer = LayerMask.GetMask(Define.LAYER_OBJECT);
        animationHandler = GetComponent<AnimationHandler>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        interaction = GetComponent<P_Interaction>();

        navMeshAgent.stoppingDistance = P_InteractionFinder.RADIUS;
    }

    private void Update()
    {
        switch (state)
        {
            case WorkerState.Interact:
                navMeshAgent.enabled = false;
                Interaction();
                break;
            case WorkerState.Interaction:
                if (target == null) SetState(WorkerState.Idle);
                break;
        }
    }

    public void SetDestination(Vector3 target, Action onComplete)
    {
        // navMeshAgent가 미리 켜져있으면, 길찾기에 오류가 있음
        // 따라서, 필요한 시점에 활성화 시작
        if (navMeshAgent.enabled == false)
        {
            navMeshAgent.enabled = true;
        }

        // 목적지 설정
        animationHandler.SetBool(Define.ID_MOVE, true);
        navMeshAgent.SetDestination(target);

        // 도착 여부 확인 후 콜백 실행
        StartCoroutine(Moving(onComplete));
    }

    public void SetState(WorkerState state)
    {
        switch (state)
        {
            case WorkerState.Idle:
                StartCoroutine(Targeting());
                break;
            case WorkerState.Move:
                SetDestination(target.position, () => SetState(WorkerState.Interact));
                break;
            case WorkerState.Interact:
                animationHandler.SetBool(Define.ID_MOVE, false);
                break;
        }

        this.state = state;
    }

    public void SetStats(UnitData unitData)
    {
        animationHandler.Animator.SetFloat(Define.ID_ACTION_SPEED, unitData.actionSpeed);
        animationHandler.Animator.SetFloat(Define.ID_MOVE_SPEED, unitData.moveSpeed / 3.5f);

        navMeshAgent.speed = unitData.moveSpeed;
    }

    private Transform GetTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;
        foreach (var collider in objectColliders)
        {
            if (collider == null)
            {
                continue;
            }

            // 자원 오브젝트가 아닐 수도 있음 (예, 건물 오브젝트)
            if (collider.GetComponent<ResourceObject>() == null)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                target = collider.transform;
                closestDistance = distance;
            }
        }

        return target;
    }

    private void Interaction()
    {
        // 이동 중 타겟이 이미 사망했다면 재탐색
        if (target == null)
        {
            SetState(WorkerState.Idle);
            return;
        }

        transform.LookAt(target);

        interaction.InteractableObject = target.GetComponentInParent<InteractableObject>();
        interaction.InteractionEnter();

        SetState(WorkerState.Interaction);
    }

    private IEnumerator Moving(Action onComplete = null)
    {
        do yield return INTERVAL;
        while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > P_InteractionFinder.RADIUS);

        onComplete?.Invoke();
    }

    private IEnumerator Targeting()
    {
        animationHandler.SetBool(Define.ID_MOVE, false);
        yield return DELAY;

        // 타겟이 반경 내에 없다면 반경 확대 후 재탐색
        while (target == null)
        {
            for (int i = 0; i < objectColliders.Length; i++) objectColliders[i] = null;
            if (Physics.OverlapSphereNonAlloc(transform.position, radius += 10.0f, objectColliders, targetLayer) == 0)
            {
                continue;
            }

            target = GetTarget();
            yield return INTERVAL;
        }

        radius = 0.0f;
        SetState(WorkerState.Move);
    }
}