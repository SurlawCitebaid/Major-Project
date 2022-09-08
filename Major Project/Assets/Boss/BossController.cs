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
    Vector3 leftFacing;
    Vector3 rightFacing;

    [SerializeField]
    GameObject deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        EnemySpawner.enemiesAlive = true;
        Door.lockDoors();
        ass = GetComponent<Animator>();
        theScale = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        leftFacing = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        rightFacing = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); ;
    }

    // Update is called once per frame
    
    public void flip()
    {
        
        if (AngleDir() > 0)
        {
            transform.localScale = leftFacing;
        }
        else if (AngleDir() < 0)
        {
            transform.localScale = rightFacing;
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
        StartCoroutine(dieEffect());
    }

    public IEnumerator dieEffect()
    {
        //Wait 4 seconds then die
        yield return new WaitForSeconds(4f);
        //Die effect
        if(gameObject != null)
        {
            GameObject.Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    } 
}
