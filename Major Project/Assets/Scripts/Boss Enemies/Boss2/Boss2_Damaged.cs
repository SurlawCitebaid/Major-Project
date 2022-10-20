using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Damaged : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Reset", false);
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        animator.SetBool("Chase", false);
        animator.SetBool("Jump", false);
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!animator.GetBool("Death"))
        {
            animator.SetBool("Damage", false);
        }
        
    }
}
