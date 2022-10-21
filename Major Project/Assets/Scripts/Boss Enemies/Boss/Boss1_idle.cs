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
    int index;
    int i;
    int[] indexes = {1,1,1,1,1,1,2,2,2,4};
    bool sameIndex;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Reset", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Attack1", false);
        animator.SetBool("Chase", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Fall", false);

        sameIndex = true;

        boss = animator.GetComponent<Boss1_Logic>();
        bossControl = animator.GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        attackRange = boss.GetAttackRange();
        index = boss.getIndex();
        i = boss.RandomNum(indexes);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float yDistance = Mathf.Abs(rb.position.y - player.position.y);
        float xDistance = Mathf.Abs(rb.position.x - player.position.x);

        while (sameIndex)
        {
            if (i == index)
            {
                i = boss.RandomNum(indexes);
            }
            else
            {
                boss.SetIndex(i);
                sameIndex = false;
            }
        }
        if (xDistance <= attackRange)
        {
            if (yDistance > 5)
            {
                bossControl.Flip();
                boss.SetIndex(5);
                animator.SetTrigger("Jump");
            }
            else
            {
                bossControl.Flip();
                animator.SetBool("Attack", true);
            }
        } 
        
        else
        {
            bossControl.Flip();
            animator.SetBool("Chase", true);
        } 
    }
}
