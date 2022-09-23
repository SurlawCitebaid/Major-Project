using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Logic : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HitBox;
    private float attackRange = 4;
    Collider2D playerCol;
    GameObject player;
    
    Animator ass;
    bool attack;

    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        ass = GetComponent<Animator>();
        hitBoxInactive();
    }

    // Update is called once per frame
    void Update()
    {
        
         
    }
    public void hitBoxActive()
    {
        HitBox.SetActive(true);
    }
    public void hitBoxInactive()
    {
        HitBox.SetActive(false);
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
    void OnCollisionStay2D(Collision2D col)
    {
        if (ass.GetBool("Fall")) { ass.SetBool("Fall", false); }  //checks if the colliders already colliding
    }
}
