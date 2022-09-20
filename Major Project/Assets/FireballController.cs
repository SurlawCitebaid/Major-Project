using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour {

    private ParticleSystem ps;
    Rigidbody2D rb;

    private void Start() {
        ps = this.gameObject.GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * 10f;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, .5f);
        foreach (Collider2D enemy in array)
        {
            if (enemy.GetComponent<BossController>() != null)
            {
                enemy.GetComponent<BossController>().Damage();
                Destroy(gameObject);
            }
            else if (enemy.GetComponent<EnemyAiController>() != null)
            {
                enemy.GetComponent<EnemyAiController>().Damage(1, 5, 0);
                Destroy(gameObject);
                //EnemyController merged with other AI behaviour

            }
            //enemy.GetComponent<EnemyController>().Damage(attackDamage, 5, attackDir);//Dans code works on this
        }

    }
    
    
}
