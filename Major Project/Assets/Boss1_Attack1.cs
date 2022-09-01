using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Attack1 : StateMachineBehaviour
{
    Boss1_Logic boss;
    int index;
    int[] indexes = { 1, 3 };
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss1_Logic>();
        index = boss.randomNum(indexes);
        boss.setIndex(index);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        switch (index)
        {
            case 1:
                animator.SetBool("Attack1", false);
                animator.SetBool("Attack", false);
                break;
            case 3:
                animator.SetTrigger("Reset");
                break;
        }
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
