# 🔨 Build The Jump!

* <b>프로젝트 설명</b>

  본 프로젝트는 유니티 6를 체험해 보기 위한 3D 개인 프로젝트입니다.
  
  생존 게임 장르로 자원을 획득하여 건물을 건설하고, 포탈 건물에서 일꾼을 생성하여 자원 채집 효율을 늘릴 수 있습니다.
  
  코드 재사용성을 높이고 확장성과 유지보수가 용이한 방향으로 작업을 목표했습니다.
  
* 개발 기간: 2025년 3월 4일 ~ 2025년 3월 11일
* 개발 엔진: Unity 6000.0.23f1

### 플레이 영상
(추후 첨부 예정)

### 게임 플레이
[WebGL: Build The Jump!](https://play.unity.com/en/games/1bbb1716-136c-47b4-800f-d44c4d4bd98f/build-the-jump)

# 🛠️ 핵심 기능

<details>
<summary>캐릭터 로직</summary>

### 물리 연산 [ 🔗 P_Rigidbody ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/Player/P_Rigidbody.cs)
Character Controller로 캐릭터를 제어하고 있으며, 물리 연산은 P_Rigidbody에서 제어하고 있습니다.

물리 연산을 위해 FixedUpdate 생명 주기에서 함수를 호출하고 있습니다.

```csharp
private void FixedUpdate()
{
    Rotate();  // 캐릭터의 방향 갱신
    Move();    // 물리 연산
}
```

캐릭터가 움직이는 방향대로 부드러운 회전을 하기 위해 LookRotation을 Slerp 함수를 통해 회전을 구현했습니다.
```csharp
private void Rotate()
{
    if (readValue.magnitude == 0.0f)
    {
        return;
    }

    Vector3 forward = Managers.Camera.Main.transform.forward;
    forward.y = 0.0f;
    forward.Normalize();

    Vector3 right = Managers.Camera.Main.transform.right;
    right.y = 0.0f;
    right.Normalize();

    direction = right * readValue.x + forward * readValue.y;
    Quaternion targetRotation = Quaternion.LookRotation(direction);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ROTATE_SPEED * Time.fixedDeltaTime);
}
```

Lerp 함수를 통해 가속도 및 감속도를 구현했습니다.

캐릭터는 항상 중력의 영향을 받으며, isGrounded 상태에서 점프 키를 누르면 P_Rigidbody의 velocity.y 값을 조절하여 포물선 운동을 구현했습니다.
```csharp
private void Move()
{
    float verticalVelocity = velocity.y;
    Vector3 targetVelocity = (readValue.magnitude > 0.0f) ? moveSpeed * direction : Vector3.zero;
    velocity = Vector3.Lerp(velocity, targetVelocity, moveSpeed * 2.0f * Time.fixedDeltaTime);
    velocity.y = verticalVelocity;

    if (isGrounded)
    {
        if (jumping)
        {
            velocity.y = JUMP_FORCE;
            animationHandler.SetTrigger(Define.ID_JUMP);
        }
        else
        {
            velocity.y = 0.0f;
        }
    }
    else
    {
        velocity.y -= GRAVITY * Time.fixedDeltaTime;
    }

    animationHandler.SetBool(Define.ID_GROUND, isGrounded);
    animationHandler.SetBool(Define.ID_MOVE, velocity.magnitude > 1.0f);

    controller.Move(velocity * Time.fixedDeltaTime);
    isGrounded = controller.isGrounded;
}
```
</details>

<details>
<summary>상호작용 로직</summary>

![Interaction](https://github.com/user-attachments/assets/abed9a28-0d4a-4dba-9b54-c51a4ad32591)

### 상호작용 [ 🔗 P_Interaction ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/Player/P_Interaction.cs)

모든 키 이벤트는 New Input SYstem으로 제어하고 있으며, Start 문에서 New Input System의 제너레이트 함수를 통해 이벤트를 연결했습니다.
```csharp
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
```

P_Interaction 스크립트는 델리게이트를 활용하여 상호작용에 진입함을 관리합니다.
```csharp
public event Action OnInteractionEnter;
```

상호작용 중인 오브젝트가 자원 오브젝트라면, 체력 UI를 표시하고, SP (스태미너)가 부족하면 행동을 중단합니다.

또한 상호작용에 필요한 도구가 있다면 장비하고, AnimationHandler를 통해 애니메이션 상태를 관리하고, 위에서 선언한 델리게이트를 통해 플레이어의 움직임을 제어합니다.

```csharp
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
                Managers.UI.Open<UI_FloatingText>().UpdateUI("스태미너가 부족합니다.", transform.position + Vector3.right * 3.0f, Color.red);
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
```

### 상호작용 찾기 [ 🔗 P_InteractionFinder ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/Player/P_InteractionFinder.cs)

P_InteractionFinder 스크립트는 OverlapSphereNonAlloc 함수를 이용해, 주변 반경에서 InteractableObject(상호작용 가능한 오브젝트)를 찾습니다.

NonAlloc 함수의 가장 큰 차이점은, 미리 만들어진 배열을 이용해 GC 호출을 최소화 합니다.

```csharp
for (int i = 0; i < objectColliders.Length; i++) objectColliders[i] = null;
if (Physics.OverlapSphereNonAlloc(transform.position, RADIUS, objectColliders, objectLayer) == 0)
{
    this.target = null;
    Close_KeyUI();
    return;
}
```

탐색한 배열에서 가장 가까운 대상을 탐색하여 상호작용 UI를 노출시킵니다.

UI는 대상의 WorldPosition을 ScreenPosition으로 변환하여 상호작용 가능한 오브젝트의 위치에서 표시될 수 있도록 위치를 갱신합니다.
```csharp
private void Update()
{
    // 이미 상호작용 중이라면 무시
    if (interaction.Interaction)
    {
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
```

상호작용이 시작될 때, 대상 오브젝트를 바라봅니다.

다만 대상 오브젝트의 위에 있을 때, Z 축 회전 방지를 위해 아래 코드를 추가했습니다.
```csharp
// Z축 기울임 방지
Vector3 lookAtPosition = new(target.position.x, transform.position.y, target.position.z);
transform.LookAt(lookAtPosition);
```

</details>

<details>
<summary>건물 로직</summary>

![Build](https://github.com/user-attachments/assets/18415d6c-8fbc-47d9-9504-34f417ea4d9f)

## 건물

### 빌드 관리 [ 🔗 BuildManager ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/Managers/BuildingManager.cs)

BuildManager에서 NewInputSystem 이벤트를 활용해 건설 위치를 결정할 때(OnBuild), 건설이 시작되었을 때(OnConfirm), 취소됬을 때를(OnCancel) 관리하고 있습니다.
```csharp
public void Initialize()
{
    layerMask = LayerMask.GetMask(Define.LAYER_GROUND);

    Managers.Input.System.UI_Building.Build.performed += OnBuild;
    Managers.Input.System.UI_Building.Confirm.canceled += OnConfirm;
    Managers.Input.System.UI_Building.Cancel.started += OnCancel;
}
```

건설이 시작되면, 현재 건설 중인 건물의 정보를 관리하고, 다른 UI 창은 활성화할 수 없도록 기존 UI InputSystem은 비활성화하고, ESC 키를 눌렀을 때 건설 취소가 가능하도록 UI_Building InputSystme을 활성화 시켜줍니다.
```csharp
public void Build(BuildingData buildingData)
{
    buildingObject = Managers.Resource.Instantiate(buildingData.name, Vector3.zero, Define.PATH_BUILDING).GetComponent<BuildingObject>();

    Managers.Input.System.UI.Disable();
    Managers.Input.System.UI_Building.Enable();
}
```

### 건물 [ 🔗 BuildingObject ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/Objects/BuildingObject.cs)
모든 건물은 BudilngObject를 상속받아 관리하고 있습니다.

BuildingObject가 confirm 전의 상태(건설 중)라면, 설치할 layer가 ground인지 확인합니다.

groundLayer가 아니라면, Material의 색상을 붉은 계열로 변경하여 사용자에게 건설이 불가능한 위치임을 알립니다.

```csharp
layer = LayerMask.NameToLayer(Define.LAYER_GROUND);

private void OnTriggerStay(Collider other)
{
    if (confirm)
    {
        return;
    }

    if (other.gameObject.layer == layer)
    {
        return;
    }

    CanBuild = false;
    meshRenderer.material.SetColor(Define.EMISSION_COLOR, Define.RED);
}
```

건설이 가능한 위치라면 Material의 색상을 푸른 계열로 변경합니다.
```csharp
private void OnTriggerExit(Collider other)
{
    if (confirm)
    {
        return;
    }

    meshRenderer.material.SetColor(Define.EMISSION_COLOR, Define.BLUE);
    CanBuild = true;
}
```

건설을 확정하면(건설을 시작하면) 충돌 처리를 위해 isTrigger를 비활성화 하고, 건설 상태를 확인할 수 있는 UI를 표시합니다.
```csharp
public void Confirm()
{
    meshCollider.isTrigger = false;
    confirm = true;

    buildingStatusUI.UpdateUI_Build(baseData as BuildingData);
    Managers.Resource.Instantiate(Define.EFFECT_BUILD, transform.position, Define.PATH_EFFECT);
}
```

### 이펙트 [ 🔗 ParticleHandler ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/Handlers/ParticleHandler.cs)
위 Confirm 함수에서 건설 시작과 동시에 아래 코드를 통해, 파티클을 활용한 이펙트를 생성하며, 모든 파티클은 재사용(풀링)을 위해 ParticleHandler에서 관리합니다.
```csharp
public void Confirm()
{
    Managers.Resource.Instantiate(Define.EFFECT_BUILD, transform.position, Define.PATH_EFFECT);
}
```

파티클 시스템 종료 시점을 감지하기 위해 ParticleSystem의 stopAction에 콜백을 연결했습니다.

OnParticleSystemStopped 이벤트 함수를 통해 오브젝트 풀링을 위한 ResourceManager의 Destroy 함수를 통해 객체를 관리(파괴)했습니다.

```csharp
private void Awake()
{
    _particleSystem = GetComponent<ParticleSystem>();

    var main = _particleSystem.main;
    main.stopAction = ParticleSystemStopAction.Callback;
}

private void OnParticleSystemStopped()
{
    Managers.Resource.Destroy(gameObject);
}
```

</details>


<details>
<summary>리소스 및 오브젝트 동적 생성</summary>

### 리소스 관리 [ 🔗 Resource Manager ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/Managers/ResourceManager.cs)

각 아이콘은 Atals로 압축하여 게임 시작 후 캐싱하고 있습니다.

캐싱된 아이콘은 GetSprite 함수로 id 값을 통해 아이콘을 불러올 수 있도록 했습니다.

```csharp
public enum SpriteType
{
    Item,
    Building,
    Rarity,
    Unit,
    Count
}
  
private readonly SpriteAtlas[] atlas = new SpriteAtlas[(int)SpriteType.Count];

public Sprite GetSprite(SpriteType type, string name) => atlas[(int)type].GetSprite(name);

public void Initialize()
{
    string[] names = Enum.GetNames(typeof(SpriteType));
    for (int i = 0; i < atlas.Length; i++)
    {
        SpriteAtlas atlas = Resources.Load<SpriteAtlas>($"{Define.PATH_ATLAS}/Atlas_{names[i]}");
        this.atlas[i] = atlas;
    }
}
```

ResourceManager는 Instatiate 함수를 통해 Resource 폴더에 있는 오브젝트를 동적으로 생성할 수 있고, 풀링된 오브젝트가 있다면 풀링된 오브젝트를 우선하여 생성합니다.
```csharp
public GameObject Instantiate(string key, Vector3 position, string pathType = Define.PATH_OBJECT)
{
    GameObject gameObject = Managers.Pool.TryPop(key);
    if (gameObject == null)
    {
        string path = $"{pathType}/{key}";
        GameObject original = Resources.Load<GameObject>(path);
        if (original == null)
        {
            Debug.LogWarning($"Failed to Load<GameObject>({path})");
            return null;
        }

        gameObject = Instantiate(original);
    }

    gameObject.transform.position = position;
    return gameObject;
}
```

ResourceManager의 Destroy 함수 역시 풀링이 가능한 오브젝트인지 확인한 후, 풀링이 가능한 오브젝트라면 PoolManager에게 오브젝트 제어권을 넘깁니다.
```csharp
public void Destroy(GameObject gameObject)
{
    if (gameObject.TryGetComponent<Poolable>(out var poolable))
    {
        Managers.Pool.Push(poolable);
        return;
    }

    Object.Destroy(gameObject);
}
```

### 오브젝트 풀링 [ 🔗 PoolManager ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/Managers/PoolManager/Pool.cs)

유니티에서 제공하는 IObjectPool을 활용하여 오브젝트 풀링을 적용했습니다.
```csharp
private readonly IObjectPool<GameObject> objectPool;

objectPool = new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
```

</details>



<details>
<summary>일꾼 로직</summary>

![Worker](https://github.com/user-attachments/assets/b4c5361e-a494-4b0a-8fbe-08bc456473ba)

### 일꾼 [ 🔗 P_Worker ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/Player/P_Worker.cs)

일꾼은 '포탈' 건물에서 소환할 수 있으며, 일꾼은 가장 가까운 자원 오브젝트를 찾아 자원을 채집합니다.

일꾼은 NavMeshAgent를 통한 길찾기와, 상태 패턴으로 일꾼의 상태를 관리하고 있습니다.

```csharp
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
```

일꾼의 공격 속도 및 이동 속도에 따라 애니메이션 속도를 변경합니다.
```csharp
public void SetStats(UnitData unitData)
{
    animationHandler.Animator.SetFloat(Define.ID_ACTION_SPEED, unitData.actionSpeed);
    animationHandler.Animator.SetFloat(Define.ID_MOVE_SPEED, unitData.moveSpeed / Define.WORKER_MOVE_SPEED);

    navMeshAgent.speed = unitData.moveSpeed;
}
```

일꾼이 navMeshAgent를 이용해 타겟에게 이동하고, 이동이 완료되었는지 특정 간격(INTERVAL)으로 코루틴을 통해 확인합니다.

이동이 완료되면 onComplete 콜백을 통해 다음 행동을 지정할 수 있습니다.

```csharp
private IEnumerator Moving(Action onComplete = null)
{
    do yield return INTERVAL;
    while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > P_InteractionFinder.RADIUS);

    onComplete?.Invoke();
}
```
</details>



<details>
<summary>미니맵 로직</summary>

### 미니맵 [ 🔗 UI_Minimap ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/UI/UI_Minimap.cs)
미니맵은 팰월드에서 영감을 받아 제작되었으며, 건물과 일꾼의 방향과 위치를 표시합니다.

단, 10m 이내의 오브젝트는 표시하지 않습니다.

플레이어의 현재 방향을 계산하고, SetPositionX 함수에서 삼각 함수를 통해 각 방향(동, 서, 남, 북)을 미니맵에 표시합니다.
```csharp
private void UpdateContent()
{
    float eulerAngle = (player.eulerAngles.y - offsetY + 360.0f) % 360.0f;
    SetPositionX(textSouth, eulerAngle, 0.0f);
    SetPositionX(textWest, eulerAngle, 90.0f);
    SetPositionX(textNorth, eulerAngle, 180.0f);
    SetPositionX(textEast, eulerAngle, 270.0f);
}
```

플레이어 위치로부터 건물과 일꾼의 방향을 구한 후, UI_MinimapItem의 정보를 갱신합니다.
```csharp
Vector3 direction = minimapItem.Target.position - player.position;
float targetAngle = Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg;
float normalizedAngle = (player.eulerAngles.y - targetAngle + 360.0f) % 360.0f / 360.0f;
float x = Mathf.Lerp(-sliderWidth, sliderWidth, normalizedAngle);
minimapItem.UpdateUI(x, direction.magnitude);
```


### 미니맵 오브젝트 [ 🔗 UI_MinimapItem ](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/UI/UI_MinimapItem.cs)
건물과 일꾼이 생성될때 아래 코드를 통해 MinimapUI에 새 MinimapItem을 추가합니다.
```csharp
Managers.UI.MinimapUI.AddItem(buildingObject.transform, SpriteType.Building, id);
```

MinimapUI에서 호출된 UpdateUI 함수를 통해 현재 오브젝트까지 남은 거리를 갱신해주고, 거리가 가까워진다면(10m 이내 거리) 해당 MinimapItemUI를 숨깁니다.
```csharp
public void UpdateUI(float x, float distance)
{
    bool isClose = distance < MIN_DISTANCE;
    if (isClose != this.isClose)
    {
        this.isClose = isClose;
        if (this.isClose)
        {
            open.Pause();
            close.Restart();
        }
        else
        {
            close.Pause();
            open.Restart();
        }
    }

    rectTransform.anchoredPosition = new(x, rectTransform.anchoredPosition.y);
    textDistance.text = $"{distance:F1}m";
}
```
</details>



<details>
<summary>3D 사운드 로직</summary>

### 3D 사운드
기본적으로 유니티 웹 빌드는 3D 사운드를 지원하지 않습니다.

웹 빌드에서 3D 사운드를 연출하기 위해 오브젝트와 플레이어(Audio Listner)와의 거리를 계산하여 볼륨을 줄이는 방식으로 사용했습니다.
```csharp
public class AudioSourceHandler : MonoBehaviour
{
    private float originalVolume;
    private AudioSource audioSource;
    private Transform player;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        originalVolume = audioSource.volume;
        player = Managers.Game.Player.transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        audioSource.volume = Mathf.Clamp01(1 - (distance / 20.0f)) * originalVolume;
        audioSource.panStereo = Mathf.Clamp((player.position.x - transform.position.x) * 0.1f, -1f, 1f);
    }
}
```
</details>

<details>
<summary>UI 로직</summary>

UI 애니메이션을 관리하기 위해 DoTween 플러그인을 사용했고, DoTween의 Sequence 관리를 위한 SequenceHandler를 만들었습니다.

SequenceHandler는 Sequence 할당, 해제, 바인딩을 관리해 줍니다.

```csharp
public class SequenceHandler
{
    public Sequence Open { get; private set; }
    public Sequence Close { get; private set; }

    public void Initialize()
    {
        Open = Utility.RecyclableSequence();
        Close = Utility.RecyclableSequence();
    }

    public void Deinitialize()
    {
        Open.Kill();
        Close.Kill();
    }

    public void Bind(UIState type, params Func<Sequence>[] sequences)
    {
        Sequence sequence = sequences[0]();
        for (int i = 1; i < sequences.Length; i++)
        {
            sequence.Join(sequences[i]());
        }

        switch (type)
        {
            case UIState.Open:
                Open.Append(sequence);
                break;
            case UIState.Close:
                Close.Append(sequence);
                break;
        }
    }
}
```

### [UI_Base](https://github.com/SpartaCodingClub/BuildTheJump/blob/main/Assets/Scripts/UI/UI_Base.cs)

모든 UI는 UI_Base를 상속받아 만들어 지며, UI가 열리는 중이거나, 닫히는 중일때는 상호작용되지 않도록 처리했습니다.

또한, 해당 상태와 바인딩되어 있는 sequence(애니메이션)를 재생해 줍니다.

```csharp
public virtual void Open()
{
    canvasGroup.interactable = true;
    canvasGroup.blocksRaycasts = true;

    sequenceHandler.Open.Restart();
}

public virtual void Close()
{
    canvasGroup.interactable = false;
    canvasGroup.blocksRaycasts = false;

    sequenceHandler.Close.Restart();
}
```

자식 객체에는 자식에 정의된 enum으로 편하게 접근할 수 있도록 BindChildren 함수를 만들었습니다.

```csharp
protected void BindChildren(Type enumType)
{
    var names = Enum.GetNames(enumType);
    foreach (var name in names)
    {
        RectTransform child = gameObject.FindComponent<RectTransform>(name);
        children.Add(child);
    }
}
```


</details>
