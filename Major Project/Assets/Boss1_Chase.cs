using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Chase : StateMachineBehaviour
{
    BossController boss;
    float attackRange;
    Transform player;
    Rigidbody2D rb;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<BossController>();
        attackRange = animator.GetComponent<BossController>().getAttackRange();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        float distance = Mathf.Abs(rb.position.x - player.position.x);
        float yDistance = Mathf.Abs(rb.position.y - player.position.y);
        if (distance > attackRange)
        {
            animator.GetComponent<BossController>().flip();
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, 7f * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        } else if (distance <= attackRange)
        {
            if(yDistance > 5)
            {
                boss.setIndex(5);
                animator.SetTrigger("Jump");
                animator.SetBool("Chase", false);
                animator.SetBool("Attack", false);
            } else
            {
                animator.SetBool("Chase", false);
                animator.SetBool("Attack", true);
            }
        }
        
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
