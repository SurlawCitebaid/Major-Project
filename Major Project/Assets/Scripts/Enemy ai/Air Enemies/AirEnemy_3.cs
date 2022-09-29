using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy_3 : MonoBehaviour
{
    private EnemyAiController states;
    private GameObject player, BlastRange;
    private bool enraged = false;
    
    // Start is called before the first frame update
    void Start()
    {
        states = this.transform.GetComponent<EnemyAiController>();
        player = GameObject.FindGameObjectWithTag("Player");
        BlastRange = transform.Find("BlastRange").gameObject;
        BlastRange.transform.localScale = new Vector2(states.enemy.attack.range, states.enemy.attack.range);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            switch (states.CurrentState())
            {
                case EnemyAiController.State.MOVING:
                    float dist = Vector3.Distance(this.transform.position, player.transform.position);
                    if (dist < states.enemy.attack.range)
                    {
                        enraged = true;
                    }
                    if (enraged)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * states.enemy.moveSpeed);
                        if (dist < (states.enemy.attack.range /2))
                        {
                            BlastRange.SetActive(true);
                        }
                    }
                    break;
            }
        
    }
}
