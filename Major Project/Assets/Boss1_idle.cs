using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_idle : StateMachineBehaviour
{
    BossController boss;
    int[] indexes = { 1,2,4};
    bool ass;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ass = false;
        boss = animator.GetComponent<BossController>();
        animator.SetBool("Reset", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Attack1", false);
        animator.SetBool("Chase", false);
        animator.SetBool("Jump", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
