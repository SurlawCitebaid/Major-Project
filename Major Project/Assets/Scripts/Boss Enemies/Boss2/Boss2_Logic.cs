using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Logic : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HitBox;
    Collider2D playerCol;
    public GameObject spear;
    GameObject player;
    Rigidbody2D rb;
    
    Animator ass;

    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCol = player.GetComponent<Collider2D>();
        ass = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
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

    public void PlaySound(string name){
        FindObjectOfType<AudioManager>().Play(name);
    }

    public float GetAttackRange()
    {
        return 3f;
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

    public void SpawnSpear(){
        Vector2 direction = player.GetComponent<Rigidbody2D>().position - rb.position;
        GameObject thrownSpear = Instantiate(spear, rb.transform.position, Quaternion.identity);
        thrownSpear.GetComponent<Rigidbody2D>().velocity = (30f * direction.normalized);
    }
}
