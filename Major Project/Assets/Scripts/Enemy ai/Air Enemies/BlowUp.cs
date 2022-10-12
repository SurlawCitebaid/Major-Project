using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject parent;
    private EnemyAiController enemyAI;
    private AirEnemy_3 control;


    private void Damage()
    {
        FindObjectOfType<AudioManager>().Play("Boom");

        enemyAI = parent.GetComponent<EnemyAiController>();
        control = parent.GetComponent<AirEnemy_3>();
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, enemyAI.enemy.attack.range / 2);
        foreach (Collider2D coll in array)
        {
            PlayerController ass = coll.gameObject.GetComponent<PlayerController>();
            if (ass != null)
            {
                ass.damage(1);
            }
        }
        control.KillSelf();
    }

    public void PlaySound(string name){
        FindObjectOfType<AudioManager>().Play(name);
    }
}
