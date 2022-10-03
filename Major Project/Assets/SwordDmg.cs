using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDmg : MonoBehaviour
{
    Collider2D swordCol;
    private void Start()
    {
        swordCol = GetComponent<Collider2D>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (col.GetComponent<BossController>() != null)
            {
                col.GetComponent<BossController>().Damage(1);
            }
            else if (col.GetComponent<EnemyAiController>() != null)
            {
                col.GetComponent<EnemyAiController>().Damage(1, 5, 0);

            }
        }
    }
    public void TurnOffCollider()
    {
        swordCol.enabled = false;
    }
}
