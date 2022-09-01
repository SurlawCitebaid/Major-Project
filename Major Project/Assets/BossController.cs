using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossController : MonoBehaviour
{
    Animator ass;
    GameObject player;
    [SerializeField] int health = 10;
    public bool isInvulnerable = true;
    SpriteRenderer theScale;
    // Start is called before the first frame update
    void Start()
    {
        EnemySpawner.enemiesAlive = true;
        ass = GetComponent<Animator>();
        theScale = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    
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
    public float AngleDir()
    {
        return (transform.position - player.transform.position).normalized.x;
    }
    public void Damage()
    {
        if (isInvulnerable)
            return;
        ass.SetTrigger("Damage");
        health -= 10;
        Debug.Log(health);
        if (health <= 0)
        {
            Die();
            ass.SetTrigger("Death");

        }
    }
    public void Die()
    {
        EnemySpawner.enemiesAlive = false;
        Destroy(gameObject, 4);
    }
}
