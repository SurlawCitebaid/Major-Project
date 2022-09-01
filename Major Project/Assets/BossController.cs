using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossController : MonoBehaviour
{
    [SerializeField] private float attackRange = 5;
    SpriteRenderer theScale;
    Rigidbody2D rb;
    Collider2D playerCol;
    GameObject player;
    Animator ass;

    int index = 1;
    // Start is called before the first frame update
    void Start()
    {
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent <Rigidbody2D>();
        ass = GetComponent<Animator>();
        theScale = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (!ass.GetBool("Damage"))
            {
                ass.SetTrigger("Damage");
                ass.SetBool("Damage", false);
            }
        }
        switch (index)
        {
            case 1:
                float dist = Vector3.Distance(transform.position, player.transform.position);
                if(dist > attackRange)
                {
                    ass.SetTrigger("Chase");
                } else
                {
                    ass.SetTrigger("Attack");
                } 
                break;
            case 2:
                dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist > attackRange)
                {
                    ass.SetTrigger("Chase");
                }
                else
                {
                    ass.SetTrigger("Attack");
                }
                break;
            case 4:
                ass.SetTrigger("Attack");
                break;
        }
    }
    public float getAttackRange()
    {
        return 6f;
    }
    public float getLongAttackRange()
    {
        return 30f;
    }
    public void setAttackRange(float i)
    {
        attackRange = i;
    }
    public void flip()
    {
        
        if (AngleDir() > 0)
        {
            theScale.flipX = true;
        }
        else if (AngleDir() < 0)
        {
            theScale.flipX = false;
        }
    }
    public int randomNum(int[] ass)
    {
        int enemyNum = Random.Range(0,ass.Length);

        return ass[enemyNum];
    }
    public float AngleDir()
    {
        return (transform.position - player.transform.position).normalized.x;
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
