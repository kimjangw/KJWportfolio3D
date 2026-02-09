using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    CharacterController cc;

    float walkSpeed = 3f;
    float runSpeed = 6f;


    int hashMoveX;
    int hashMoveY;
    int hashJump;
    int hashMouseLeft;
    int hashMouseRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        //InputManager 이벤트 등록
        //1회성 입력되는 액션들만 등록한다
        InputManager.OnJump += HandleJump;
        InputManager.OnLeftClick += MouseLeftClick;
        InputManager.OnRightClick += MouseRightClick;
    }

    
    private void OnDisable()
    {
        //InputManager 이벤트 등록
        //1회성 입력되는 액션들만 등록한다
        InputManager.OnJump -= HandleJump;
        InputManager.OnLeftClick -= MouseLeftClick;
        InputManager.OnRightClick -= MouseRightClick;
    }


    private void MouseLeftClick()
    {
        Debug.Log("좌클릭");
        anim.SetTrigger(hashMouseLeft);
    }

    private void MouseRightClick()
    {
        Debug.Log("우클릭");
        anim.SetTrigger(hashMouseRight); // 우클릭도 같은 트리거면 그대로
    }
    private void  HandleJump()
    {
        print("점프");
        anim.SetTrigger(hashJump);
    }

    void Update()
    {
        //플레이어 이동
        PlayerMove(InputManager.Input, InputManager.IsSprint);

    }

    void PlayerMove(Vector2 input, bool isLeftShiftPressed)
    {
        if (input.magnitude > 0.1f)
        {
            //이동 처리
            Vector3 dir = new Vector3(input.x, 0f, input.y);
            dir.Normalize();
            float curSpeed = isLeftShiftPressed ? runSpeed : walkSpeed;
            cc.Move(dir * curSpeed * Time.deltaTime);

            //이동 애니메이션
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
}
