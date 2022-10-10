using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Jump : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    float yPos;
    bool jumping;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        jumping = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!jumping)
        {
            yPos = player.position.y + 2;
            jumping = true;
        }
        Vector2 target = new Vector2(rb.position.x, yPos);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, 30 * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        animator.SetBool("Attack1", true);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
