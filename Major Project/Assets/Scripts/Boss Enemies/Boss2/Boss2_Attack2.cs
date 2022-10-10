using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Swipe Down
public class Boss2_Attack2 : StateMachineBehaviour
{
    BossController bossControl;
    Boss2_Logic boss;
    int index;
    int[] indexes = { 3, 2 };
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossControl = animator.GetComponent<BossController>();
        boss = animator.GetComponent<Boss2_Logic>();
        index = boss.RandomNum(indexes);
        boss.SetIndex(index);

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        switch (index)
        {
            case 1:
                animator.SetBool("Attack2", false);
                animator.SetBool("Attack1", false);
                bossControl.Flip();
                break;
            case 2:
                animator.SetBool("Attack2", false);
                animator.SetBool("Attack1", false);
                animator.SetBool("Attack3", true);
                break;
            case 3:
                animator.SetBool("Reset", true);
                bossControl.Flip();
                break;
        }
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
