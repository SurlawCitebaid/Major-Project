using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Chase : StateMachineBehaviour
{
    Boss2_Logic boss;
    BossController bossControl;
    float attackRange;
    Transform player;
    Rigidbody2D rb;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossControl = animator.GetComponent<BossController>();
        boss = animator.GetComponent<Boss2_Logic>();
        attackRange = boss.GetAttackRange();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();

    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        float distance = Mathf.Abs(rb.position.x - player.position.x);
        float yDistance = Mathf.Abs(rb.position.y - player.position.y);

        
        if (distance > attackRange)
        {
            bossControl.Flip();
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, 7f * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        } else //in attackrange
        {
            bossControl.Flip();
            if (yDistance > 4)//Jump
            {
                //bossControl.Flip();
                //animator.SetBool("Attack3", true);
                animator.SetBool("Chase", false);

            } else
            {
                animator.SetBool("Chase", false);
            }
        }
        
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
