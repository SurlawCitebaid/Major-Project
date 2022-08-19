using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_movement : MonoBehaviour
{
    public movement controller;
    public Animator animator;
    float move_horizontal = 0f;
    float jump_charge = 1f;
    float jump_charge_max = 2f;
    bool jump = false;
    bool crouch = false;
    bool dash = false;

    void Update()
    {
        move_horizontal = Input.GetAxisRaw("Horizontal");
        //animator.SetFloat("Speed", Mathf.Abs(move_horizontal));    // link movement speed to animator speed parameter
        bool grounded = controller.GetGrounded();
        if (grounded == true)
        {
            if (Input.GetButton("Jump"))
            {
                crouch = true;
                if (jump_charge < jump_charge_max)
                {
                    jump_charge += Time.deltaTime * 1.2f;
                } else {
                    jump_charge = jump_charge_max;
                }
            }
            if (Input.GetButtonUp("Jump"))
            {
                crouch = false;
                jump = true;
            }
        } else {
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
            jump_charge = 1;
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
        controller.Move(move_horizontal * Time.fixedDeltaTime, crouch, jump, jump_charge, dash);
        jump = false;
    }
}
