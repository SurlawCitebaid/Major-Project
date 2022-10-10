using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Swipe Up Attack
public class Boss2_Attack1 : StateMachineBehaviour
{
    Boss2_Logic boss;
    BossController bossControl;
    //Rigidbody2D rb;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossControl = animator.GetComponent<BossController>();
        boss = animator.GetComponent<Boss2_Logic>();
        //rb = animator.GetComponent<Rigidbody2D>();
        animator.SetBool("Reset", false);
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (boss.getIndex())
        {
            case 1:
                animator.SetBool("Reset", true);
                break;
            case 2:
                if(boss.getIndex() != 5)
                {
                    animator.SetBool("Attack2", true);
                }
                
                break;
            case 3:
                animator.SetBool("Attack3", true);
                break;
            case 4://dash move
                if (!animator.GetBool("Reset"))
                {
                    animator.SetBool("Reset", true);
                }
                break;
            case 5:
                animator.SetBool("Fall", true);
                break;
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetBool("Damage"))
        {
            bossControl.isInvulnerable = false;
        }
    }
}
