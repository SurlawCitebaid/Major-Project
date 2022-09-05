using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_idle : StateMachineBehaviour
{
    BossController bossControl;
    Boss1_Logic boss;
    int[] indexes = { 2};
    bool ass;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ass = false;
        boss = animator.GetComponent<Boss1_Logic>();
        bossControl = animator.GetComponent<BossController>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossControl.flip();
        if (animator.GetBool("Damage"))
        {
            bossControl.isInvulnerable = true;
            animator.SetBool("Damage", false);
        }
        animator.SetBool("Reset", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Attack1", false);
        animator.SetBool("Chase", false);
        animator.SetBool("Jump", false);
        if (!ass)
        {
            int i = boss.randomNum(indexes);
            while (i != boss.getIndex())
            {
                boss.setIndex(i);
                ass = true;
                if(boss.getIndex() == 5)
                {
                    animator.SetBool("Jump", true);
                }
                
                break;
            } 
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
