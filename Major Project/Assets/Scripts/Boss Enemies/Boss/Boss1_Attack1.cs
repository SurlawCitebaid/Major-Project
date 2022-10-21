using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Attack1 : StateMachineBehaviour
{
    BossController bossControl;
    Boss1_Logic boss;
    int index;
    int[] indexes = { 3, 3, 3, 3, 1, 1 };
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossControl = animator.GetComponent<BossController>();
        boss = animator.GetComponent<Boss1_Logic>();
        index = boss.RandomNum(indexes);
        boss.SetIndex(index);

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        switch (index)
        {
            case 1:
                animator.SetBool("Attack1", false);
                animator.SetBool("Attack", false);
                bossControl.Flip();
                break;
            case 3:
                animator.SetBool("Reset", true);
                bossControl.Flip();
                break;
        }
        
    }
}
