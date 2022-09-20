using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Attack : StateMachineBehaviour
{
    Boss1_Logic boss;
    BossController bossControl;
    Rigidbody2D rb;
    float ass;
    Transform player;
    bool bass;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossControl = animator.GetComponent<BossController>();
        boss = animator.GetComponent<Boss1_Logic>();
        rb = animator.GetComponent<Rigidbody2D>();
        ass = rb.position.x;
        bass = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        switch (boss.getIndex())
        {
            case 1:
                animator.SetBool("Reset", true);
                break;
            case 2:
                animator.SetBool("Attack1", true); 
                break;
            case 4:
                if (!animator.GetBool("Reset"))
                {
                    if (!bass)
                    {
                        if (bossControl.AngleDir() > 0)
                        {
                            ass = ass - 20;
                        }
                        else if (bossControl.AngleDir() < 0)
                        {
                            ass = ass + 20;
                        }
                        bass = true;
                    }

                    animator.SetBool("Reset", true);
                }
                Vector2 target4 = new Vector2(ass, rb.position.y);
                Vector2 newPos4 = Vector2.MoveTowards(rb.position, target4, 100f * Time.fixedDeltaTime);
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
