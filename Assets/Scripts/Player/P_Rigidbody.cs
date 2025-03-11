using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

public class P_Rigidbody : MonoBehaviour
{
    private readonly float GRAVITY = 9.81f;
    private readonly float ROTATE_SPEED = 10.0f;
    private readonly float JUMP_FORCE = 8.0f;

    #region Inspector
    [ShowInInspector, ReadOnly] private Vector2 readValue;
    [ShowInInspector, ReadOnly] private Vector3 direction;
    [ShowInInspector, ReadOnly] private Vector3 velocity;
    #endregion
    #region InputSystem
    private void OnMove(InputAction.CallbackContext callbackContext)
    {
        readValue = callbackContext.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext callbackContext)
    {
        jumping = callbackContext.phase == InputActionPhase.Started;
    }
    #endregion

    private AnimationHandler animationHandler;
    private CharacterController controller;

    private bool isGrounded;
    private bool jumping;
    private float moveSpeed;

    private void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Managers.Game.Interaction.OnInteractionEnter += () =>
        {
            readValue = Vector2.zero;
            jumping = false;
        };

        Managers.Input.System.Player.Move.performed += OnMove;
        Managers.Input.System.Player.Move.canceled += OnMove;
        Managers.Input.System.Player.Jump.started += OnJump;
        Managers.Input.System.Player.Jump.canceled += OnJump;

        SetMoveSpeed(Define.PLAYER_MOVE_SPEED);
    }

    private void FixedUpdate()
    {
        Rotate();  // 캐릭터의 방향 갱신
        Move();    // 물리 연산
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        animationHandler.Animator.SetFloat(Define.ID_MOVE_SPEED, moveSpeed / Define.WORKER_MOVE_SPEED);
        this.moveSpeed = moveSpeed;
    }

    public void Jump(float jumpForce)
    {
        velocity.y = jumpForce;
        isGrounded = false;
    }

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
}