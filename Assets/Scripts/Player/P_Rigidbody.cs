using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

[RequireComponent(typeof(AnimationHandler))]
public class P_Rigidbody : MonoBehaviour
{
    private readonly float GRAVITY = 9.81f;
    private readonly float MOVE_SPEED = 5.0f;
    private readonly float ROTATE_SPEED = 10.0f;
    private readonly float JUMP_FORCE = 15.0f;

    #region Inspector
    [ShowInInspector, ReadOnly] private Vector2 readValue;
    [ShowInInspector, ReadOnly] private Vector3 direction;
    [ShowInInspector, ReadOnly] private Vector3 velocity;
    #endregion
    #region InputSystem
    private void Move(InputAction.CallbackContext callbackContext)
    {
        readValue = callbackContext.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        jumping = callbackContext.phase == InputActionPhase.Started;
    }
    #endregion

    private AnimationHandler animationHandler;
    private CharacterController controller;

    private bool jumping;

    private void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Managers.Input.System.Player.Move.performed += Move;
        Managers.Input.System.Player.Move.canceled += Move;
        Managers.Input.System.Player.Jump.started += Jump;
        Managers.Input.System.Player.Jump.canceled += Jump;
    }

    private void Update()
    {
        Rotate();
        Move();
    }

    private void Move()
    {
        Vector3 targetVelocity = (readValue.magnitude > 0.0f) ? MOVE_SPEED * direction : Vector3.zero;
        velocity = Vector3.Lerp(velocity, targetVelocity, MOVE_SPEED * 2.0f * Time.deltaTime);

        bool isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            if (jumping)
            {
                velocity.y = JUMP_FORCE;
                animationHandler.SetTrigger(Define.ID_JUMP);
            }
            else
            {
                velocity.y = jumping ? JUMP_FORCE : 0.0f;
            }
        }
        else
        {
            velocity.y -= GRAVITY * Time.deltaTime;
        }

        animationHandler.SetBool(Define.ID_GROUND, isGrounded);
        animationHandler.SetBool(Define.ID_MOVE, velocity.magnitude > 1.0f);
        controller.Move(velocity * Time.deltaTime);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ROTATE_SPEED * Time.deltaTime);
    }
}