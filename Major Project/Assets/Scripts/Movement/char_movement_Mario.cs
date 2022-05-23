using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_movement_Mario : MonoBehaviour
{
    public movement_Mario controller;
    public Animator animator;
    float move_horizontal = 0f;
    bool jump = false;
    bool crouch = false;
    bool dash = false;
    float jumpCounter;


    void Update()
    {
        move_horizontal = Input.GetAxisRaw("Horizontal");
        //animator.SetFloat("Speed", Mathf.Abs(move_horizontal));    // link movement speed to animator speed parameter
        if (Input.GetButtonDown("Jump"))
        {
            controller.addJumpCount();
            jumpCounter = controller.getJumpDuration();
            controller.Jump();
            jump = true;
        }
        if((Input.GetButton("Jump")) && jump)
        {
            if(jumpCounter > 0)
            {
                controller.Jump();
                jumpCounter -= Time.deltaTime;
            } else {
                jump = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            jump = false;
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
