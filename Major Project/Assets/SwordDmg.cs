using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDmg : MonoBehaviour
{
    Collider2D swordCol;
    public GameObject zapEffect;
    public GameObject explodeEffect;
    public float damage;
    private void Start()
    {
        swordCol = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (PlayerController.Instance.zapOn)
            {
                Instantiate(zapEffect, col.transform.position, col.transform.rotation);
            }
            if (PlayerController.Instance.ExplodeOn)
            {
                Instantiate(explodeEffect, col.transform.position, col.transform.rotation);
            }
            if (col.GetComponent<BossController>() != null)
            {
                col.GetComponent<BossController>().Damage(damage);
            }
            else if (col.GetComponent<EnemyAiController>() != null)
            {
                col.GetComponent<EnemyAiController>().Damage(damage);
                

            }
        }
    }
    public void TurnOffCollider()
    {
        swordCol.enabled = false;
    }
}
