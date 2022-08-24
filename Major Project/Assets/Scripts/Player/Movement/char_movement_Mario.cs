using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_movement_Mario : MonoBehaviour
{
    public movement_Mario controller;
    public Animator animator;
    float move_horizontal = 0f;
    bool jump = false;
    bool isGrounded, isTouchingWall, isWallGrabing, isfalling, canCharge;
    bool crouch = false;
    bool dash = false;
    float jumpDuration;


    void Update()
    {
        move_horizontal = Input.GetAxis("Horizontal");
        isGrounded = controller.getGrounded();
        isTouchingWall = controller.getTouchingWall();
        isWallGrabing = controller.getWallGrabing();
        isfalling = controller.getFalling();

        // animation part
        if (move_horizontal != 0 && isGrounded)
        {
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }
        if (!isGrounded && isfalling)
        {
            animator.SetBool("isFalling", true);
        } else {
            animator.SetBool("isFalling", false);
        }
        if (isGrounded)
        {
            canCharge = true;
            animator.SetBool("isJumping", false);
            animator.SetBool("isGrounded", true);
        } else {
            animator.SetBool("isGrounded", false);
        }
        if(isWallGrabing)
        {
            animator.SetBool("isGrabing", true);
        } else {
            animator.SetBool("isGrabing", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (!isGrounded && isTouchingWall && move_horizontal != 0)
            {
                controller.WallJump();
                animator.SetBool("isJumping", true);
            } else {
                controller.addJumpCount();
                jumpDuration = controller.getJumpDuration();
                controller.Jump();
                animator.SetBool("isJumping", true);
                jump = true;
            }
        }
        // keep adding force midair until time out or key released
        if((Input.GetButton("Jump")) && jump && canCharge)
        {
            if(jumpDuration > 0)
            {
                controller.Jump();
                animator.SetBool("isJumping", true);
                jumpDuration -= Time.deltaTime;
            } else {
                jump = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            canCharge = false;
        }

        if (Input.GetButtonDown("Rush_B"))
        {
            dash = true;
        } else if (Input.GetButtonUp("Rush_B")) {
            dash = false;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(move_horizontal * Time.fixedDeltaTime, crouch, dash);
    }

}
