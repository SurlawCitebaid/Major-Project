using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Logic : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HitBox;
    Collider2D playerCol;
    
    Animator ass;

    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        ass = GetComponent<Animator>();
        HitBoxInactive();
    }

    // Update is called once per frame
    public void HitBoxActive()
    {
        HitBox.SetActive(true);
    }
    public void HitBoxInactive()
    {
        HitBox.SetActive(false);
    }
    public float GetAttackRange()
    {
        return 4f;
    }
    public int RandomNum(int[] ass)
    {
        int enemyNum = Random.Range(0, ass.Length);

        return ass[enemyNum];
    }
    public int getIndex()
    {
        return index;
    }
    public void SetIndex(int i)
    {
        index = i;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!ass.GetBool("Idle")) { ass.SetBool("Idle", true); }  //just to switch start state once

        if (ass.GetBool("Fall")) { ass.SetBool("Fall", false); }  //just to switch start state once

        if (col.gameObject.CompareTag( "Player"))
        {
            Physics2D.IgnoreCollision(playerCol, col.collider);
        }

    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (ass.GetBool("Fall")) { ass.SetBool("Fall", false); }  //checks if the colliders already colliding
    }
}
