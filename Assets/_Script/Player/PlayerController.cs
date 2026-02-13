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

    int hashMoveX, hashMoveY, hashJump, hashPickUp;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        hashMoveX = Animator.StringToHash("MoveX");
        hashMoveY = Animator.StringToHash("MoveY");
        hashJump = Animator.StringToHash("Jump");
        hashPickUp = Animator.StringToHash("PickUp");
    }

    private void OnEnable()
    {
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

    private void MouseLeftClick()
    {
        if (phaseGimmick != null)
        {
            anim.SetTrigger(hashPickUp);
            phaseGimmick.PhaseToggle(ToggleDurationSeconds);
            if (gimmickController != null) gimmickController.TogglePhaseAB();
        }
    }

    private void MouseRightClick()
    {
        if (desolveGimmick != null)
        {
            anim.SetTrigger(hashPickUp);
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
        if (isGrounded && velocity.y < 0) velocity.y = -5f;

        PlayerMove(InputManager.Input, InputManager.IsSprint);

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    // CameraZoneTrigger에서 호출 (ClearShot 활성화 시 방향 고정)
    public void SetCameraZoneMode(bool active)
    {
        isInCameraZone = active;
        if (active)
        {
            lockedForward = Camera.main.transform.forward;
            lockedForward.y = 0; lockedForward.Normalize();
            lockedRight = Camera.main.transform.right;
            lockedRight.y = 0; lockedRight.Normalize();
        }
    }

    void PlayerMove(Vector2 input, bool isLeftShiftPressed)
    {
        if (input.magnitude > 0.1f)
        {
            Vector3 moveDir;
            Vector3 lookDir;

            // 1. 이동 방향 및 시선 방향 계산 (ClearShot 구역 대응)
            if (isInCameraZone)
            {
                moveDir = (lockedForward * input.y + lockedRight * input.x).normalized;
                lookDir = lockedForward; // 구역 진입 시점의 정면 고정
            }
            else
            {
                Vector3 camForward = Camera.main.transform.forward;
                Vector3 camRight = Camera.main.transform.right;
                camForward.y = 0; camRight.y = 0;
                moveDir = (camForward.normalized * input.y + camRight.normalized * input.x).normalized;
                lookDir = camForward.normalized; // 현재 카메라가 보는 정면
            }

            // 2. 캐릭터 회전: 이동 방향이 아닌 '카메라 정면'을 바라보게 함
            // 이렇게 해야 S를 누를 때 몸을 돌리지 않고 뒤로 걷는 애니메이션이 나옵니다.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 10f);

            // 3. 실제 이동 적용
            float curSpeed = isLeftShiftPressed ? runSpeed : walkSpeed;
            cc.Move(moveDir * curSpeed * Time.deltaTime);

            // 4. 애니메이터 전달: 블렌드 트리의 좌/우/후진 파라미터 활용
            float animSpeed = isLeftShiftPressed ? 2f : 1f;
            anim.SetFloat(hashMoveX, input.x * animSpeed);
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