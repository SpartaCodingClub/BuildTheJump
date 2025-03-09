using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using VInspector;

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

    private enum WorkerState
    {
        Idle,
        Move,
        Interact,
        Interaction,
    }

    private readonly Collider[] colliders = new Collider[8];

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
    }

    private void Start()
    {
        SetState(WorkerState.Idle);
    }

    private void Update()
    {
        switch (state)
        {
            case WorkerState.Move:
                if (navMeshAgent.remainingDistance <= P_InteractionFinder.RADIUS) SetState(WorkerState.Interact);
                break;
            case WorkerState.Interact:
                Interaction();
                break;
            case WorkerState.Interaction:
                if (target == null) SetState(WorkerState.Idle);
                break;
        }
    }

    private void SetState(WorkerState state)
    {
        this.state = state;
        switch (state)
        {
            case WorkerState.Idle:
                //navMeshAgent.isStopped = false;
                StartCoroutine(Targeting());
                break;
            case WorkerState.Move:
                animationHandler.SetBool(Define.ID_MOVE, true);
                navMeshAgent.SetDestination(target.position);
                break;
            case WorkerState.Interact:
                animationHandler.SetBool(Define.ID_MOVE, false);
                //navMeshAgent.isStopped = true;
                break;
        }
    }

    private IEnumerator Targeting()
    {
        yield return DELAY;

        while (target == null)
        {
            if (Physics.OverlapSphereNonAlloc(transform.position, radius += 10.0f, colliders, targetLayer) == 0)
            {
                continue;
            }

            target = GetTarget();
            yield return INTERVAL;
        }

        SetState(WorkerState.Move);
    }

    private Transform GetTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;
        foreach (var collider in colliders)
        {
            if (collider == null)
            {
                continue;
            }

            if (collider.TryGetComponent<ResourceObject>(out var resourceObject) == false)
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

        radius = 0.0f;
        return target;
    }

    private void Interaction()
    {
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
}