using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

[RequireComponent(typeof(P_Interaction))]
public class P_InteractionFinder : MonoBehaviour
{
    public static readonly float RADIUS = 2.0f;

    #region Inspector
    [SerializeField, ReadOnly]
    private Transform target;
    #endregion
    #region InputSystem
    private void OnInteraction(InputAction.CallbackContext callbackContext)
    {
        if (interaction.Interaction)
        {
            return;
        }

        if (target == null)
        {
            return;
        }

        transform.LookAt(target);

        interaction.InteractableObject = target.GetComponentInParent<InteractableObject>();
        interaction.InteractionEnter();

        Close_KeyUI();
    }
    #endregion
    #region UI_Key
    private UI_Key keyUI;

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

    private readonly Collider[] monsterColliders = new Collider[8];
    private readonly Collider[] objectColliders = new Collider[8];

    private LayerMask monsterLayer;
    private LayerMask objectLayer;
    private P_Interaction interaction;

    private void Awake()
    {
        monsterLayer = LayerMask.GetMask(Define.LAYER_MONSTER);
        objectLayer = LayerMask.GetMask(Define.LAYER_OBJECT);
        interaction = GetComponent<P_Interaction>();
    }

    private void Start()
    {
        Managers.Input.System.Player.Interaction.started += OnInteraction;
    }

    private void Update()
    {
        if (interaction.Interaction)
        {
            return;
        }

        for (int i = 0; i < monsterColliders.Length; i++) monsterColliders[i] = null;
        if (Physics.OverlapSphereNonAlloc(transform.position, RADIUS, monsterColliders, monsterLayer) > 0)
        {
            this.target = monsterColliders[0].transform;
            return;
        }

        for (int i = 0; i < objectColliders.Length; i++) objectColliders[i] = null;
        if (Physics.OverlapSphereNonAlloc(transform.position, RADIUS, objectColliders, objectLayer) == 0)
        {
            this.target = null;
            Close_KeyUI();
            return;
        }

        var target = GetTarget();
        if (target != this.target)
        {
            Close_KeyUI();
        }

        this.target = target;
        Open_KeyUI();
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

            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                target = collider.transform;
                closestDistance = distance;
            }
        }

        return target;
    }
}