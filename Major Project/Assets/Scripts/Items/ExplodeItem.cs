using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeItem : MonoBehaviour
{
    [SerializeField]
    GameObject willOWispParticles;
    public float radius;
    [SerializeField]
    public float explosionChance = 40;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0f, 100f) < explosionChance)
        {
            FindObjectOfType<AudioManager>().Play("WillOWisp");
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, PlayerController.Instance.explodeRadius);
            if (enemies != null)
            {
                foreach (Collider2D enemy in enemies)
                {
                    if (enemy.CompareTag("Enemy"))
                    {
                        Instantiate(willOWispParticles, enemy.transform.position, Quaternion.identity);
                        if (enemy.gameObject.GetComponent<EnemyAiController>() != null)
                        {
                            enemy.gameObject.GetComponent<EnemyAiController>().Damage(PlayerController.Instance.baseDamage);
                        }
                        else
                        {
                            enemy.gameObject.GetComponent<BossController>().Damage(PlayerController.Instance.baseDamage);
                        }
                    }
                }
            }
        }
        Destroy(gameObject, 2f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
