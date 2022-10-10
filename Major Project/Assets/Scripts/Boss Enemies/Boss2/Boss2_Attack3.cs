using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Swipe Down
public class Boss2_Attack3 : StateMachineBehaviour
{
    BossController bossControl;
    Boss2_Logic boss;
    int index;
    
    int[] indexes = { 1, 2};
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
                case 1://throw again
                    animator.SetBool("Reset", true);
                    break;
                case 2:
                    animator.SetBool("Attack3", false);
                    animator.SetBool("Reset", true);
                    break;
            }

            
        
        
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    

}
