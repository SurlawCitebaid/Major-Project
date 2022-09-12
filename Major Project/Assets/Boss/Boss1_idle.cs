using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_idle : StateMachineBehaviour
{
    BossController bossControl;
    Boss1_Logic boss;
    int[] indexes = {1,2};
    bool sameIndex;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform ass = animator.transform.GetChild(0);

        sameIndex = true;
        boss = animator.GetComponent<Boss1_Logic>();
        bossControl = animator.GetComponent<BossController>();
        boss.HitBox.SetActive(false);
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

        if (sameIndex)
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

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
