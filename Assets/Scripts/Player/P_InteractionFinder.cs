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
        // 상호작용 중 상호작용 키를 누르면 무시
        if (interaction.Interaction)
        {
            return;
        }

        // 평캔 중 재호출 시, 타겟이 이미 사망했다면 무시
        if (target == null)
        {
            return;
        }

        // Z축 기울임 방지
        Vector3 lookAtPosition = new(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookAtPosition);

        // 상호작용 시작
        interaction.InteractableObject = target.GetComponentInParent<InteractableObject>();
        interaction.InteractionEnter();

        // 상호작용 UI 숨김
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
        // 상호작용이 버튼을 누르면 상호작용 시작
        Managers.Input.System.Player.Interaction.started += OnInteraction;
    }

    private void Update()
    {
        // 이미 상호작용 중이라면 무시
        if (interaction.Interaction)
        {
            return;
        }

        // 몬스터 우선 탐색
        for (int i = 0; i < monsterColliders.Length; i++) monsterColliders[i] = null;
        if (Physics.OverlapSphereNonAlloc(transform.position, RADIUS, monsterColliders, monsterLayer) > 0)
        {
            this.target = monsterColliders[0].transform;
            return;
        }

        // 자원 탐색
        for (int i = 0; i < objectColliders.Length; i++) objectColliders[i] = null;
        if (Physics.OverlapSphereNonAlloc(transform.position, RADIUS, objectColliders, objectLayer) == 0)
        {
            this.target = null;
            Close_KeyUI();
            return;
        }

        // 가장 가까운 자원 탐색
        var target = GetTarget();
        if (target != this.target)
        {
            Close_KeyUI();
        }

        // 상호작용 UI 표시
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