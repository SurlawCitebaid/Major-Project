using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSwordAttack : MonoBehaviour
{
    [SerializeField] private GameObject slash;
    private float radius;
    public void Damage()
    {
        float damage = transform.GetComponentInParent<PlayerAttack>().damage;
        radius = transform.localScale.x / 2;
        Debug.Log(radius);
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D enemy in array)
        {
            if (enemy.GetComponent<BossController>() != null)
            {
                enemy.GetComponent<BossController>().Damage(damage);
                Instantiate(slash, enemy.transform.position, Quaternion.Euler(0,0,Random.Range(0,360)), enemy.transform);
            }
            else if (enemy.GetComponent<EnemyAiController>() != null)
            {
                enemy.GetComponent<EnemyAiController>().Damage(damage, 5, 0);
                Instantiate(slash, enemy.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)), enemy.transform);
                //EnemyController merged with other AI behaviour

            }
        }
    }
   
}
