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
  
## Character Controller
Character Controller로 캐릭터를 제어하고 있습니다.

캐릭터의 물리 연산은 P_Rigidbody에서 제어합니다.

캐릭터가 움직이는 방향대로 부드러운 회전을 하기 위해 LookRotation을 Slerp 함수를 통해 회전을 구현했습니다.

캐릭터는 회전한 방향을 기준으로 W, A, S, D 키로 움직일 수 있으며, 역시 Lerp 함수를 통해 가속도를 구현했습니다.

캐릭터는 항상 중력의 영향을 받으며, isGrounded 상태에서 점프 키를 누르면 P_Rigidbody의 velocity.y 값을 조절하여 포물선 운동을 구현했습니다.

### P_Interaction, P_InteractionFinder
모든 키는 New Input SYstem으로 제어하고 있습니다.

Start 문에서 New Input System의 제너레이트 함수를 통해 이벤트를 연결해주고 있습니다.

P_Interaction 스크립트는 델리게이트를 활용하여 상호작용의 시작, 중, 종료 상태를 관리해주고 있습니다.

P_InteractionFinder 스크립트는 OverlapSphereNonAlloc 함수를 이용해, 주변 반경에서 InteractableObject(상호작용 가능한 오브젝트)를 찾습니다.

또한 미리 만들어진 배열을 이용해 GC 호출을 최소화 합니다.

탐색한 배열에서 가장 가까운 대상을 탐색하여 상호작용 UI를 노출시킵니다.
</details>



<details>
<summary>상호작용 로직</summary>

## 인터렉티브 키
주변에 상호작용 가능한 오브젝트가 있다면, 상호작용 UI가 노출됩니다.

UI는 대상의 WorldPosition을 ScreenPosition으로 변환하여 상호작용 가능한 오브젝트의 위치에서 표시될 수 있도록 위치를 갱신합니다.
</details>



<details>
<summary>건물 로직</summary>

## 건물
건물은 장애물이 없는 지형에만 설치가 가능하며, 다양한 종류의 건물을 건설할 수 있습니다.
</details>



<details>
<summary>일꾼 로직</summary>

## 일꾼
일꾼은 '포탈' 건물에서 소환할 수 있으며, 일꾼은 가장 가까운 자원 오브젝트를 찾아 자원을 채집합니다.
</details>



<details>
<summary>미니맵 로직</summary>

## 미니맵
미니맵은 팰월드에서 영감을 받아 제작되었으며, 건물과 일꾼의 방향과 위치를 표시합니다.
단, 10m 이내의 오브젝트는 표시하지 않습니다.
</details>



<details>
<summary>3D 사운드 로직</summary>

## 3D 사운드
기본적으로 유니티 웹 빌드는 3D 사운드를 지원하지 않습니다.
오브젝트와 플레이어(Audio Listner)와의 거리를 계산하여 볼륨을 줄이는 방식으로 사용했습니다.
</details>



<details>
<summary>UI 로직</summary>
## 팝업
건물이나 아이템에 커서를 올리면, 해당 아이템의 설명 팝업이 노출됩니다.

## 네비게이션
아이템 획득, 건물 건설, 일꾼 소환 등 중요한 알림을 화면의 좌측 하단에서 확인하실 수 있습니다.
</details>





