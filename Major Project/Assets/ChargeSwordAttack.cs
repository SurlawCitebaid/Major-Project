using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSwordAttack : MonoBehaviour
{
    [SerializeField] private GameObject slash;
    public GameObject zapEffect, explodeEffect;
    private float radius;
    public void Damage()
    {
        float damage = transform.GetComponentInParent<PlayerAttack>().damage/2;
        radius = transform.localScale.x / 2;
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D enemy in array)
        {
            if(enemy.CompareTag("Enemy"))
            {
                if (PlayerController.Instance.ExplodeOn)
                {
                    Instantiate(explodeEffect, enemy.transform.position, enemy.transform.rotation);
                }
                if (PlayerController.Instance.zapOn)
                {
                    Instantiate(zapEffect, enemy.transform.position, enemy.transform.rotation);
                }
                if (enemy.GetComponent<BossController>() != null)
                {
                    enemy.GetComponent<BossController>().Damage(damage);
                    Instantiate(slash, enemy.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)), enemy.transform);
                }
                else if (enemy.GetComponent<EnemyAiController>() != null)
                {
                    enemy.GetComponent<EnemyAiController>().Damage(damage);
                    Instantiate(slash, enemy.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)), enemy.transform);
                    //EnemyController merged with other AI behaviour

                }
            }
            
        }
    }
   
}
