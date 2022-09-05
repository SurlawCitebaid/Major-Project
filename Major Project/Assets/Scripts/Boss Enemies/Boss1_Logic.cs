using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Logic : MonoBehaviour
{
    // Start is called before the first frame update
    private float attackRange = 4;
    SpriteRenderer theScale;
    Collider2D playerCol;
    GameObject player;
    Animator ass;

    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        ass = GetComponent<Animator>();
        theScale = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ass.GetBool("Death"))
        {
            switch (index)
            {
                case 1:

                    float dist = Mathf.Abs(transform.position.x - player.transform.position.x);
                    Debug.Log(dist + " " + attackRange);
                    if (dist <= attackRange)
                    {
                        ass.SetTrigger("Attack");
                    }
                    else
                    {
                        ass.SetTrigger("Chase");
                    }
                    break;
                case 2:
                    dist = Mathf.Abs(transform.position.x - player.transform.position.x);
                    if (dist <= attackRange)
                    {
                        ass.SetTrigger("Attack");
                    }
                    else
                    {
                        ass.SetTrigger("Chase");
                    }
                    break;
                case 4:
                    ass.SetTrigger("Attack");
                    break;
            }
        } 
    }
    public float getAttackRange()
    {
        return 4f;
    }
    public void setAttackRange(float i)
    {
        attackRange = i;
    }
    public int randomNum(int[] ass)
    {
        int enemyNum = Random.Range(0, ass.Length);

        return ass[enemyNum];
    }
    public int getIndex()
    {
        return index;
    }
    public void setIndex(int i)
    {
        index = i;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!ass.GetBool("Idle")) { ass.SetBool("Idle", true); }  //just to switch start state once

        if (ass.GetBool("Fall")) { ass.SetBool("Fall", false); }  //just to switch start state once

        if (col.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(playerCol, col.collider);
        }

    }
}
