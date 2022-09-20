using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_idle : StateMachineBehaviour
{
    BossController bossControl;
    Boss1_Logic boss;
    Transform player;
    Rigidbody2D rb;
    float attackRange;
    int[] indexes = {1,2};
    bool sameIndex;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform ass = animator.transform.GetChild(0);

        sameIndex = true;

        boss = animator.GetComponent<Boss1_Logic>();
        bossControl = animator.GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        attackRange = boss.getAttackRange();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossControl.flip();
        animator.SetBool("Reset", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Attack1", false);
        animator.SetBool("Chase", false);
        animator.SetBool("Jump", false);

        float yDistance = Mathf.Abs(rb.position.y - player.position.y);

            if (sameIndex)
            {
                if (yDistance > 5)
                {
                    boss.setIndex(5);
                    animator.SetTrigger("Jump");
                } else
                {
                    int i = boss.randomNum(indexes);
                    while (i != boss.getIndex())
                    {
                        boss.setIndex(i);
                        sameIndex = false;
                        break;
                    }
                }
            
            }
        
        



    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
