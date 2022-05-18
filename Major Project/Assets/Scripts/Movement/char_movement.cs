using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_movement : MonoBehaviour
{
    public movement controller;
    public Animator animator;
    float move_horizontal = 0f;
    float move_speed = 20f;
    bool jump = false;
    bool crouch = false;
    bool run = false;

    void Update()
    {
        move_horizontal = Input.GetAxisRaw("Horizontal") * move_speed;
        animator.SetFloat("Speed", Mathf.Abs(move_horizontal));    // link movement speed to animator speed parameter

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch")) {
            crouch = false;
        }

        if (Input.GetButtonDown("Rush_B"))
        {
            run = true;
        } else if (Input.GetButtonUp("Rush_B")) {
            run = false;
        }
    }

    void FixedUpdate()
    {
        controller.Move(move_horizontal * Time.fixedDeltaTime, crouch, jump, run);
        jump = false;
    }
}
