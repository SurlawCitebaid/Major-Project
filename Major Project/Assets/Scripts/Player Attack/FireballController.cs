using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour {

    public GameObject fireBall, zapEffect, explodeEffect;
    public float damage;
    Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * 10f;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x/2);
        foreach (Collider2D enemy in array)
        {
            if (enemy.CompareTag("Enemy"))
            {
                if (PlayerController.Instance.zapOn)
                {
                    Instantiate(zapEffect, enemy.transform.position, enemy.transform.rotation);
                }
                if (PlayerController.Instance.ExplodeOn)
                {
                    Instantiate(explodeEffect, enemy.transform.position, enemy.transform.rotation);
                }
                if (enemy.GetComponent<BossController>() != null)
                {
                    enemy.GetComponent<BossController>().Damage(damage);
                    Destroy(gameObject);
                    Instantiate(fireBall, this.transform.position, transform.rotation);
                }
                else if (enemy.GetComponent<EnemyAiController>() != null)
                {
                    enemy.GetComponent<EnemyAiController>().Damage(damage);
                    Destroy(gameObject);
                    Instantiate(fireBall, this.transform.position, transform.rotation);
                    //EnemyController merged with other AI behaviour

                }
            }
        }
        if(other.CompareTag( "Wall"))
        {
            Destroy(gameObject);
            Instantiate(fireBall, this.transform.position, transform.rotation);
        }

    }
    
    
}
