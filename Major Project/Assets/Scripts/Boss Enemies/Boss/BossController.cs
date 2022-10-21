using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossController : MonoBehaviour
{
    Animator anim;
    GameObject player;
    [SerializeField] int health = 25;
    public bool isInvulnerable = false;
    SpriteRenderer theScale;
    Vector3 leftFacing;
    Vector3 rightFacing;

    [SerializeField]
    GameObject deathParticles;
    //Camera Shake
    CameraShake cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        health = 25;
        cameraShake = GameObject.FindGameObjectWithTag("Camera Shaker").GetComponent<CameraShake>();
        anim = GetComponent<Animator>();
        theScale = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        leftFacing = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        rightFacing = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        health *= PlayerController.Instance.stages * 2;
        Debug.Log("Boss Health " + health);
    }

    // Update is called once per frame
    
    public void Flip()
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
    public void Damage(float damage)
    {
        if (isInvulnerable)
            return;

        health -= (int) damage;
        StartCoroutine(Invulnerable());

        if (health <= 0)
        {
            Die();
            anim.SetTrigger("Death");

        } else {
            FindObjectOfType<AudioManager>().Play("EnemyHit");
        }
    }
    public IEnumerator Invulnerable()
    {
        isInvulnerable = true;
        theScale.color = Color.grey;
        yield return new WaitForSeconds(2);
        isInvulnerable = false;
        theScale.color = Color.white;
    }
    public void Die()
    {
        Door.bossStart = false;
        EnemySpawner.enemiesAlive = false;
        StartCoroutine(DieEffect());
    }

    public IEnumerator DieEffect()
    {
        //Wait 4 seconds then die
        yield return new WaitForSeconds(4f);
        FindObjectOfType<AudioManager>().ScenePlayMusic(true);//restarts scene music
        FindObjectOfType<DropItem>().DropBoss(gameObject);
        //Die effect
        if(gameObject != null)
        {
            cameraShake.CamShake();
            GameObject.Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    } 
}
