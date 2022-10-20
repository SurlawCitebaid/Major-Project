using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_movement_Mario : MonoBehaviour
{
    public movement_Mario m_movementController;
    public GrappleController m_grappleController;
    public PlayerAttack m_playerAttack;
    public Animator animator;
    float move_horizontal = 0f;
    bool jump = false;
    public bool isGrounded, isTouchingWall, isWallGrabing, isfalling, isDashing, isAttacking, canCharge, isGrappling;
    bool canSwap = true;
    bool crouch = false;
    bool dash = false;
    float jumpDuration;
    string weapon;
    int comboCount = 0;


    void Update()
    {
        move_horizontal = Input.GetAxis("Horizontal");
        isGrounded = m_movementController.getGrounded();
        isTouchingWall = m_movementController.getTouchingWall();
        isWallGrabing = m_movementController.getWallGrabing();
        isfalling = m_movementController.getFalling();
        isDashing = m_movementController.getDashing();
        isAttacking = m_playerAttack.GetAttacking();
        isGrappling = m_grappleController.GetGrappling();

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
            weapon = m_playerAttack.weapon.ToString();
            comboCount = m_playerAttack.GetComboCount();
            if (weapon.Equals("FIST"))
            {
                animator.SetBool("isUsingFist", true);
                if (comboCount == 0)
                {
                    animator.SetInteger("ComboCount", 0);
                } else if (comboCount == 1)
                {
                    animator.SetInteger("ComboCount", 1);
                } else if (comboCount == 2)
                {
                    animator.SetInteger("ComboCount", 2);
                }
            } else {
                animator.SetBool("isUsingFist", false);
            }
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
        if (isGrappling)
        {
            animator.SetBool("isGrappling", true);
        } else {
            animator.SetBool("isGrappling", false);
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

        if (Input.GetButtonDown("Swap"))
        {
            if (canSwap)
            {
                canSwap = false;
                StartCoroutine(cycle());
            }
        }
    }

    private void FixedUpdate()
    {
        m_movementController.Move(move_horizontal * Time.fixedDeltaTime, crouch, dash);
    }

    IEnumerator cycle()
    {
      yield return new WaitUntil(() => isAttacking == false);
      m_playerAttack.cycleWeapon();
      yield return new WaitForSeconds(1);
      canSwap = true;
    }
}
