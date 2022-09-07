using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_movement_Mario : MonoBehaviour
{
    public movement_Mario m_movementController;
    public PlayerAttack m_playerAttack;
    public Animator animator;
    float move_horizontal = 0f;
    bool jump = false;
    bool isGrounded, isTouchingWall, isWallGrabing, isfalling, isDashing, isAttacking, canCharge;
    bool crouch = false;
    bool dash = false;
    float jumpDuration;


    void Update()
    {
        move_horizontal = Input.GetAxis("Horizontal");
        isGrounded = m_movementController.getGrounded();
        isTouchingWall = m_movementController.getTouchingWall();
        isWallGrabing = m_movementController.getWallGrabing();
        isfalling = m_movementController.getFalling();
        isDashing = m_movementController.getDashing();
        isAttacking = m_playerAttack.getAttacking();

        //////////////animation part//////////////
        if (isDashing)                                              // invincible when dashing
        {
            animator.SetBool("isDashing", true);
        } else {
            animator.SetBool("isDashing", false);
        }
        if (isAttacking)                                              // invincible when dashing
        {
            animator.SetBool("isAttacking", true);
        } else {
            animator.SetBool("isAttacking", false);
        }
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
        //////////////////////////////////////////

        if (Input.GetButtonDown("Jump"))
        {
            if (!isGrounded && isTouchingWall && move_horizontal != 0)
            {
                m_movementController.WallJump();
                animator.SetBool("isJumping", true);
            } else {
                m_movementController.addJumpCount();
                jumpDuration = m_movementController.getJumpDuration();
                m_movementController.Jump();
                animator.SetBool("isJumping", true);
                jump = true;
            }
        }
        // keep adding force midair until time out or key released
        if((Input.GetButton("Jump")) && jump && canCharge)
        {
            if(jumpDuration > 0)
            {
                m_movementController.Jump();
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
        m_movementController.Move(move_horizontal * Time.fixedDeltaTime, crouch, dash);
    }

}
