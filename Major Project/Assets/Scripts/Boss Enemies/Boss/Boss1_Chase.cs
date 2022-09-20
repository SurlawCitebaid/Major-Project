using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Chase : StateMachineBehaviour
{
    Boss1_Logic boss;
    BossController bossControl;
    float attackRange;
    Transform player;
    Rigidbody2D rb;
    int[] indexes = { 1, 2, 3, 4 };
    int index;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossControl = animator.GetComponent<BossController>();
        boss = animator.GetComponent<Boss1_Logic>();
        attackRange = boss.getAttackRange();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        index = boss.randomNum(indexes);

    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        float distance = Mathf.Abs(rb.position.x - player.position.x);
        float yDistance = Mathf.Abs(rb.position.y - player.position.y);

        if (index != 4)
        {
            if (distance > attackRange)
            {
                bossControl.flip();
                Vector2 target = new Vector2(player.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, 7f * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            } else if (distance <= attackRange)
            {
                bossControl.flip();
                if (yDistance > 5)
                {
                    boss.setIndex(5);
                    animator.SetTrigger("Jump");
                } else
                {
                    animator.SetBool("Chase", false);
                }
            }
        } else
        {
            boss.setIndex(4);
            animator.SetBool("Chase", false);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
