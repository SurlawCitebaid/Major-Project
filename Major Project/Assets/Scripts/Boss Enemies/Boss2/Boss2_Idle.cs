using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Idle : StateMachineBehaviour
{
    BossController bossControl;
    Boss2_Logic boss;
    Transform player;
    Rigidbody2D rb;
    float attackRange;
    int bossCurrentIndex;//currentBossIndex
    int i;
    int[] probabilities = {1,2};//probability
    bool sameIndex;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Reset", false);
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        animator.SetBool("Chase", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Fall", false);

        sameIndex = true;

        boss = animator.GetComponent<Boss2_Logic>();
        bossControl = animator.GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        attackRange = boss.GetAttackRange();
        bossCurrentIndex = boss.getIndex();
        i = boss.RandomNum(probabilities);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float yDistance = Mathf.Abs(rb.position.y - player.position.y);
        float xDistance = Mathf.Abs(rb.position.x - player.position.x);

        while (sameIndex)
        {
            if (i == bossCurrentIndex)
            {
                i = boss.RandomNum(probabilities);
            }
            else
            {
                boss.SetIndex(i);
                sameIndex = false;
            }
        }
        if (xDistance <= attackRange)//boss2 attack range is smaller
        {
            if (yDistance > 4)
            {
                bossControl.Flip();
                boss.SetIndex(2);//index 5 = jump
                bossControl.Flip();
                animator.SetBool("Attack3", true);
            }
            else
            {
                bossControl.Flip();
                animator.SetBool("Attack1", true);
            }
        } 
        else
        {

            if (boss.getIndex() == 1)
            {
                bossControl.Flip();
                animator.SetBool("Chase", true);
            } else if (boss.getIndex() == 2) {
                bossControl.Flip();
                animator.SetBool("Attack3", true);
            }
            
        }
        
        
    }
}
