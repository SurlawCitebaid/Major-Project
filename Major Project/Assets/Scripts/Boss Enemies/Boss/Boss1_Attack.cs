using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Attack : StateMachineBehaviour
{
    Boss1_Logic boss;
    BossController bossControl;
    Rigidbody2D rb;
    float RbXPos;
    bool goOnce;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossControl = animator.GetComponent<BossController>();
        boss = animator.GetComponent<Boss1_Logic>();
        rb = animator.GetComponent<Rigidbody2D>();
        RbXPos = rb.position.x;
        animator.SetBool("Reset", false);
        goOnce = false;
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
                    animator.SetBool("Attack1", true);
                }
                
                break;
            case 4:
                if (!animator.GetBool("Reset"))
                {
                    if (!goOnce)
                    {
                        if (bossControl.AngleDir() > 0)
                        {
                            RbXPos += - 10;
                        }
                        else if (bossControl.AngleDir() < 0)
                        {
                            RbXPos += 10;
                        }
                        goOnce = true;
                    }

                    animator.SetBool("Reset", true);
                }
                Vector2 target4 = new Vector2(RbXPos, rb.position.y);
                Vector2 newPos4 = Vector2.MoveTowards(rb.position, target4, 50f * Time.fixedDeltaTime);
                rb.MovePosition(newPos4);
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
