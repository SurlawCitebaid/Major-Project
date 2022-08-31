using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Attack : StateMachineBehaviour
{
    BossController boss;
    Rigidbody2D rb;
    float ass;
    Transform player;
    bool bass;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        boss = animator.GetComponent<BossController>();
        rb = animator.GetComponent<Rigidbody2D>();
        ass = rb.position.x;
        bass = false;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (boss.getIndex())
        {
            case 1:
                if (!bass)
                {
                    boss.flip();
                }
                animator.SetBool("Reset", true);
                break;
            case 2:
                if (!bass)
                {
                    boss.flip();
                }
                animator.SetBool("Attack1", true); 
                break;
            case 4:
                if (!animator.GetBool("Reset"))
                {
                    if (!bass)
                    {
                        boss.flip();
                        boss.setAttackRange(boss.getAttackRange());
                        if (boss.AngleDir(player.transform.position, rb.position) < 0)
                        {
                            ass = ass - 10;
                        }
                        else if (boss.AngleDir(player.transform.position, rb.position) > 0)
                        {
                            ass = ass + 10;
                        }
                        bass = true;
                    }
                    Vector2 target4 = new Vector2(ass, rb.position.y);
                    Vector2 newPos4 = Vector2.MoveTowards(rb.position, target4, 100f * Time.fixedDeltaTime);
                    rb.MovePosition(newPos4);

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

    }
}
