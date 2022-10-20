using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeItem : MonoBehaviour
{
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position,radius);
        foreach(Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.gameObject.GetComponent<EnemyAiController>().Damage(PlayerController.Instance.baseDamage);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
