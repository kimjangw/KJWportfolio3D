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

    [Header("Hit Penalty Settings")]
    public float penaltyDuration = 1.0f;
    public float penaltySpeed = 0f;
    private bool isPenalized = false;
    public UnityEngine.Rendering.Volume hitVolume;

    [Header("Phase Collision")]
    public bool isPhaseA = true;
    private int playerLayer;
    private int ballALayer;
    private int ballBLayer;

    int hashMoveX, hashMoveY, hashJump, hashPickUp;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        hashMoveX = Animator.StringToHash("MoveX");
        hashMoveY = Animator.StringToHash("MoveY");
        hashJump = Animator.StringToHash("Jump");
        hashPickUp = Animator.StringToHash("PickUp");

        playerLayer = LayerMask.NameToLayer("Player");
        ballALayer = LayerMask.NameToLayer("Ball_A");
        ballBLayer = LayerMask.NameToLayer("Ball_B");

        UpdateBallCollision();
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

            isPhaseA = !isPhaseA;
            UpdateBallCollision();
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

            if (isInCameraZone)
            {
                moveDir = (lockedForward * input.y + lockedRight * input.x).normalized;
                lookDir = lockedForward;
            }
            else
            {
                Vector3 camForward = Camera.main.transform.forward;
                Vector3 camRight = Camera.main.transform.right;
                camForward.y = 0; camRight.y = 0;
                moveDir = (camForward.normalized * input.y + camRight.normalized * input.x).normalized;
                lookDir = camForward.normalized;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 10f);

            float curSpeed = isPenalized ? penaltySpeed : (isLeftShiftPressed ? runSpeed : walkSpeed);
            cc.Move(moveDir * curSpeed * Time.deltaTime);

            float animSpeed = isPenalized ? 0f : (isLeftShiftPressed ? 2f : 1f);
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

    public void TakeHit()
    {
        StopCoroutine("HitPenaltyRoutine");
        StartCoroutine("HitPenaltyRoutine");
    }

    private System.Collections.IEnumerator HitPenaltyRoutine()
    {
        isPenalized = true;
        if (hitVolume != null) hitVolume.weight = 1f;

        float timer = 0f;
        while (timer < penaltyDuration)
        {
            timer += Time.deltaTime;
            if (hitVolume != null)
                hitVolume.weight = Mathf.Lerp(1f, 0f, timer / penaltyDuration);

            yield return null;
        }

        isPenalized = false;
        if (hitVolume != null) hitVolume.weight = 0f;
    }

    private void UpdateBallCollision()
    {
        if (isPhaseA)
        {
            Physics.IgnoreLayerCollision(playerLayer, ballALayer, true);
            Physics.IgnoreLayerCollision(playerLayer, ballBLayer, false);
        }
        else
        {
            Physics.IgnoreLayerCollision(playerLayer, ballALayer, false);
            Physics.IgnoreLayerCollision(playerLayer, ballBLayer, true);
        }
    }
}