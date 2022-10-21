using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchController : MonoBehaviour
{
    public float damage, time;
    public GameObject zapEffect, explodeEffect;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(selfDestruct(time));
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
                }
                else if (enemy.GetComponent<EnemyAiController>() != null)
                {
                    enemy.GetComponent<EnemyAiController>().Damage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
    IEnumerator selfDestruct(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
