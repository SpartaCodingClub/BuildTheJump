using UnityEngine;

public class P_Finder : MonoBehaviour
{
    #region Inspector
    [SerializeField] private LayerMask targetLayer;
    [SerializeField, Min(0.0f)] private float radius;
    #endregion
    #region UI_Key
    private void Open_KeyUI()
    {
        if (keyUI == null)
        {
            keyUI = Managers.UI.Open<UI_Key>();
        }

        keyUI.UpdateUI(target);
    }

    private void Close_KeyUI()
    {
        if (keyUI == null)
        {
            return;
        }

        keyUI.Close();
        keyUI = null;
    }
    #endregion

    private readonly Collider[] colliders = new Collider[8];

    private P_Interaction interaction;
    private Transform target;
    private UI_Key keyUI;

    private void Awake()
    {
        interaction = GetComponent<P_Interaction>();
    }

    private void Start()
    {
        interaction.OnInteractionEnter += () =>
        {
            transform.LookAt(target);
            Close_KeyUI();
        };
    }

    private void Update()
    {
        if (interaction.OnInteraction)
        {
            return;
        }

        if (Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, targetLayer) == 0)
        {
            Close_KeyUI();
            return;
        }

        target = GetTarget();
        Open_KeyUI();
        GetInteraction();
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

            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                target = collider.transform;
                closestDistance = distance;
            }
        }

        return target;
    }

    private void GetInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F) == false)
        {
            return;
        }

        interaction.InteractableObject = target.GetComponent<InteractableObject>();
        interaction.InteractionEnter();
    }
}