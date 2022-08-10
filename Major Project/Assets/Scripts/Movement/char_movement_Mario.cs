using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_movement_Mario : MonoBehaviour
{
    public movement_Mario controller;
    public Animator animator;
    float move_horizontal = 0f;
    bool jump = false;
    bool isGrounded, isTouchingWall, canCharge;
    bool crouch = false;
    bool dash = false;
    float jumpDuration;


    void Update()
    {
        move_horizontal = Input.GetAxisRaw("Horizontal");
        isGrounded = controller.getGrounded();
        isTouchingWall = controller.getTouchingWall();
        if (isGrounded)
        {
            canCharge = true;
        }
        //animator.SetFloat("Speed", Mathf.Abs(move_horizontal));    // link movement speed to animator speed parameter
        if (Input.GetButtonDown("Jump"))
        {
            if (!isGrounded && isTouchingWall && move_horizontal != 0)
            {
                controller.WallJump();
            } else {
                controller.addJumpCount();
                jumpDuration = controller.getJumpDuration();
                controller.Jump();
                jump = true;
            }
        }
        // keep adding force midair until time out or key released
        if((Input.GetButton("Jump")) && jump && canCharge)
        {
            if(jumpDuration > 0)
            {
                controller.Jump();
                jumpDuration -= Time.deltaTime;
            } else {
                jump = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            canCharge = false;
        }
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch")) {
            crouch = false;
        }

        if (Input.GetButtonDown("Rush_B"))
        {
            dash = true;
        } else if (Input.GetButtonUp("Rush_B")) {
            dash = false;
        }
    }

    void FixedUpdate()
    {
        controller.Move(move_horizontal * Time.fixedDeltaTime, crouch, dash);
    }
}
