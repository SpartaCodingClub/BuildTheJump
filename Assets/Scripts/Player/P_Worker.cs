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
    private readonly WaitForSeconds INTERVAL = new(0.5f);

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
                Interaction();
                break;
            case WorkerState.Interaction:
                if (target == null) SetState(WorkerState.Idle);
                break;
        }
    }

    public void SetDestination(Vector3 target, Action onComplete)
    {
        if (navMeshAgent.enabled == false)
        {
            navMeshAgent.enabled = true;
        }

        animationHandler.SetBool(Define.ID_MOVE, true);
        navMeshAgent.SetDestination(target);

        StartCoroutine(Moving(onComplete));
    }

    public void SetState(WorkerState state)
    {
        this.state = state;
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
    }

    private Transform GetTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var collider in objectColliders)
        {
            if (collider == null)
            {
                continue; ;
            }

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
        if (target == null || state != WorkerState.Interact)
        {
            SetState(WorkerState.Idle);
            return;
        }

        transform.LookAt(target);

        interaction.InteractableObject = target.GetComponentInParent<InteractableObject>();
        interaction.InteractionEnter();

        SetState(WorkerState.Interaction);
    }

    private IEnumerator Moving(Action onComplete)
    {
        do
        {
            yield return null;
        }
        while (navMeshAgent.remainingDistance > P_InteractionFinder.RADIUS);

        onComplete();
    }

    private IEnumerator Targeting()
    {
        animationHandler.SetBool(Define.ID_MOVE, false);
        yield return DELAY;

        while (target == null)
        {
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