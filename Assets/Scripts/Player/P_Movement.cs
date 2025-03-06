using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

public class P_Movement : MonoBehaviour
{
    private readonly float GRAVITY = 9.81f;
    private readonly float MOVE_SPEED = 5.0f;
    private readonly float ROTATE_SPEED = 10.0f;

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
    #endregion


    private AnimationHandler animationHandler;
    private CharacterController controller;
    private P_Interaction interaction;

    private void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
        controller = GetComponent<CharacterController>();
        interaction = GetComponent<P_Interaction>();
    }

    private void Start()
    {
        Managers.Input.System.Player.Move.performed += Move;
        Managers.Input.System.Player.Move.canceled += Move;
        //interaction.OnInteractionEnter += () =>
        //{
        //    animationHandler.SetBool(Define.HASH_ACTION, true);
        //    animationHandler.SetFloat(Define.HASH_SPEED, 0.0f);
        //};

        //interaction.OnInteractionExit += () => animationHandler.SetBool(Define.HASH_ACTION, false);
    }

    private void Update()
    {
        Rotate();
        Move();
    }

    private void Move()
    {
        Vector3 targetVelocity = (readValue.magnitude > 0.0f) ? MOVE_SPEED * direction : Vector3.zero;
        Vector3 horizontalVelocity = Vector3.Lerp(velocity, targetVelocity, MOVE_SPEED * Time.deltaTime);
        if (controller.isGrounded)
        {
            velocity.y = 0.0f;
        }
        else
        {
            velocity.y -= GRAVITY * Time.deltaTime;
        }

        velocity = horizontalVelocity + velocity.y * Vector3.up;
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