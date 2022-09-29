using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject parent;
    private EnemyAiController control;
    private void Damage()
    {
        control = parent.GetComponent<EnemyAiController>();
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, control.enemy.attack.range / 2);
        foreach (Collider2D coll in array)
        {
            PlayerController ass = coll.gameObject.GetComponent<PlayerController>();
            if (ass != null)
            {
                ass.damage(1);
            }

            control.Die();
            
               
            
        }
    }
}
