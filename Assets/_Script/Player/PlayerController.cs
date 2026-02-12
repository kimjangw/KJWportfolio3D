using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    CharacterController cc;
    public GimmickController gimmickController;

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    [Header("Physics & Jump")]
    public float gravity = -19.62f;
    public float jumpHeight = 1.5f;
    private Vector3 velocity;
    public bool isGrounded;

    [Header("Ground Check (Tag-Based)")]
    public string groundTag = "Ground";
    public float extraRayLength = 0.3f;

    [Header("Camera Zone Settings")]
    public bool isInCameraZone = false;
    private Vector3 lockedForward;
    private Vector3 lockedRight;

    [Header("Gimmick Settings")]
    public PhaseGimmick phaseGimmick;
    public DesolveGimmick desolveGimmick;
    public float ToggleDurationSeconds = 0.35f;

    int hashMoveX, hashMoveY, hashJump;
    int hashMouseLeft, hashMouseRight;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        hashMoveX = Animator.StringToHash("MoveX");
        hashMoveY = Animator.StringToHash("MoveY");
        hashJump = Animator.StringToHash("Jump");
        hashMouseLeft = Animator.StringToHash("MouseLeft");
        hashMouseRight = Animator.StringToHash("MouseRight");
    }

    private void OnEnable()
    {
        // 1회성 입력 액션들 등록
        InputManager.OnJump += HandleJump;
        InputManager.OnLeftClick += MouseLeftClick;
        InputManager.OnRightClick += MouseRightClick;
    }

    private void OnDisable()
    {
        InputManager.OnJump -= HandleJump;
        InputManager.OnLeftClick -= MouseLeftClick;
        InputManager.OnRightClick -= MouseRightClick;
    }

    // 좌클릭 기믹: Phase 토글
    private void MouseLeftClick()
    {
        Debug.Log("좌클릭 기믹 발동");
        if (phaseGimmick != null)
        {
            phaseGimmick.PhaseToggle(ToggleDurationSeconds);
            if (gimmickController != null) gimmickController.TogglePhaseAB();
        }
    }

    // 우클릭 기믹: Desolve 토글
    private void MouseRightClick()
    {
        Debug.Log("우클릭 기믹 발동");
        if (desolveGimmick != null)
        {
            desolveGimmick.DesolveToggle(ToggleDurationSeconds);
            if (gimmickController != null) gimmickController.ToggleDesolveSolid();
        }
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger(hashJump);
            isGrounded = false;
        }
    }

    void Update()
    {
        isGrounded = CheckGroundedWithTag();

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        PlayerMove(InputManager.Input, InputManager.IsSprint);

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    public void SetCameraZoneMode(bool active)
    {
        isInCameraZone = active;
        if (active)
        {
            lockedForward = Camera.main.transform.forward;
            lockedForward.y = 0;
            lockedForward.Normalize();

            lockedRight = Camera.main.transform.right;
            lockedRight.y = 0;
            lockedRight.Normalize();
        }
    }

    void PlayerMove(Vector2 input, bool isLeftShiftPressed)
    {
        if (input.magnitude > 0.1f)
        {
            Vector3 moveDir;

            if (isInCameraZone)
            {
                moveDir = (lockedForward * input.y + lockedRight * input.x).normalized;
            }
            else
            {
                Vector3 camForward = Camera.main.transform.forward;
                Vector3 camRight = Camera.main.transform.right;
                camForward.y = 0; camRight.y = 0;
                moveDir = (camForward.normalized * input.y + camRight.normalized * input.x).normalized;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * 10f);
            float curSpeed = isLeftShiftPressed ? runSpeed : walkSpeed;
            cc.Move(moveDir * curSpeed * Time.deltaTime);

            float animSpeed = isLeftShiftPressed ? 2f : 1f;
            anim.SetFloat(hashMoveX, input.x);
            anim.SetFloat(hashMoveY, input.y * animSpeed);
        }
        else
        {
            anim.SetFloat(hashMoveX, 0f);
            anim.SetFloat(hashMoveY, 0f);
        }
    }

    bool CheckGroundedWithTag()
    {
        float rayDistance = (cc.height / 2f) + extraRayLength;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider.CompareTag(groundTag)) return true;
        }
        return cc.isGrounded;
    }
}