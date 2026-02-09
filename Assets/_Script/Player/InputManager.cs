using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.DirectorControlPlayable;

[RequireComponent(typeof(PlayerInput))]

public class InputManager : MonoBehaviour
{

    private PlayerInput playerInput;

    public static Vector2 Input {  get; private set; }
    public static bool IsSprint { get; private set; }

    public static event Action OnJump;
    public static event Action OnLeftClick;
    public static event Action OnRightClick;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction mouseAction;


    private Action<InputAction.CallbackContext> onMovePerformed;
    private Action<InputAction.CallbackContext> onMoveCanceled;
    private Action<InputAction.CallbackContext> onSprintPerformed;
    private Action<InputAction.CallbackContext> onSprintCanceled;
    private Action<InputAction.CallbackContext> onJumpPerformed;
    private Action<InputAction.CallbackContext> onMousePerformed;


    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.defaultActionMap = "Player";
        playerInput.defaultControlScheme = "Keyboard&Mouse"; //"Default"
        playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

        // 각각의 액션들 찾기
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        sprintAction = playerInput.actions.FindAction("Sprint");
        mouseAction = playerInput.actions.FindAction("Mouse");

        // Move Action 콜백등록
        if (moveAction != null)
        {
            onMovePerformed = ctx => Input = ctx.ReadValue<Vector2>();
            onMoveCanceled = ctx => Input = Vector2.zero;
            moveAction.performed += onMovePerformed;
            moveAction.canceled += onMoveCanceled;
        }

        // Sprint Action 콜백등록
        if (sprintAction != null)
        {
            onSprintPerformed = ctx => IsSprint = true;
            onSprintCanceled = ctx => IsSprint = false;
            sprintAction.performed += onSprintPerformed;
            sprintAction.canceled += onSprintCanceled;
        }

        // Jump Action 콜백등록
        if (jumpAction != null)
        {
            onJumpPerformed = ctx => OnJump?.Invoke();
            jumpAction.performed += onJumpPerformed;
        }

        // Mouse Action 콜백등록
        if (mouseAction != null)
        {
            onMousePerformed = ctx =>
            {
                // 어떤 컨트롤(왼/오른 버튼)이 눌렸는지 확인
                string controlPath = ctx.control != null ? ctx.control.path : string.Empty;

                if (controlPath.Contains("leftButton"))
                    OnLeftClick?.Invoke();

                if (controlPath.Contains("rightButton"))
                    OnRightClick?.Invoke();
            };

            mouseAction.performed += onMousePerformed;
        }
    }


    private void OnDisable()
    {
        if (moveAction != null)
        {
            if (onMovePerformed != null)
            {
                moveAction.performed -= onMovePerformed;
                onMovePerformed = null;
            }

            if (onMoveCanceled != null)
            {
                moveAction.canceled -= onMoveCanceled;
                onMoveCanceled = null;
            }
        }

        // Sprint Action 콜백해제
        if (sprintAction != null)
        {
            if (onSprintPerformed != null)
            {
                sprintAction.performed -= onSprintPerformed;
                onSprintPerformed = null;
            }

            if (onSprintCanceled != null)
            {
                sprintAction.canceled -= onSprintCanceled;
                onSprintCanceled = null;
            }
        }

        if (jumpAction != null && onJumpPerformed != null)
        {
            jumpAction.performed -= onJumpPerformed;
            onJumpPerformed = null;
        }

        if (mouseAction != null && onMousePerformed != null)
        {
            mouseAction.performed -= onMousePerformed;
            onMousePerformed = null;
        }
    }
}
