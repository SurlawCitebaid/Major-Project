using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemZapEffect : MonoBehaviour
{
    public GameObject Ball;
    Collider2D[] array;
    List<GameObject> Enemies, enemyPositions;

    Vector2 currentPos;
    GameObject closestEnemy;
    public int maxjumps = 0;
    int index = 0, loops = 0, totalEnemies = 0;
    int closestEnemyIndex;
    float minDist = Mathf.Infinity;
    int counter = 0, jumps;
    // Start is called before the first frame update
    void Start()
    {
        Enemies = new List<GameObject>();
        enemyPositions = new List<GameObject>();
        array = Physics2D.OverlapCircleAll(this.transform.position, 1000);
        convertEnemy(array);
        currentPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.Instance.zaps != maxjumps)                              //updates when maxjumps is changed via item pickup
        {
            maxjumps = PlayerController.Instance.zaps;
            jumps = maxjumps;
        }
        if(totalEnemies < maxjumps)                                                 //if the total enemies is less than the amount of enemies present
        {
            jumps = totalEnemies;                   
        }
        
        if (loops >= jumps)                                                         //runs when position of all enemies information processed reaches max enemies present or jump limit
        {
            if (enemyPositions.Count == 0)
            {
                Destroy(gameObject);
            }else if (enemyPositions[index] == null)                               //checks if it was destroyed before reaching
            {
                Destroy(gameObject);
            } else if (Vector2.Distance(Ball.transform.position, enemyPositions[index].transform.position) < 0.01f)
            {
                if(index != enemyPositions.Count -1)
                {
                    enemyPositions[index].GetComponent<EnemyAiController>().Damage(PlayerController.Instance.baseDamage/2);
                    index = (index + 1);
                } else
                {
                    Destroy(gameObject);            //reaches last position
                }
            }
            else
            {                                                           // moves the 
                Ball.transform.position = Vector2.MoveTowards(
                Ball.transform.position,
                enemyPositions[index].transform.position,
                36f * Time.deltaTime);
            }

        } else
        {
            foreach (GameObject Enemy in Enemies)
            {
                if (Enemy != null)                          //checks if they are destroyed before reaching
                {
                    float dist = Vector2.Distance(Enemy.transform.position, currentPos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestEnemy = Enemy.gameObject;
                        closestEnemyIndex = counter;
                    }
                    counter++;
                } else
                {
                    Destroy(gameObject);                        
                }
            }
            if(Enemies.Count == 0)                      //If There are no enemies
            {
                Destroy(gameObject);
            } else
            {
                if(loops != maxjumps)
                {
                    currentPos = closestEnemy.transform.position;
                    counter = 0;
                    loops++;
                    minDist = Mathf.Infinity;

                    enemyPositions.Add(closestEnemy);
                    Enemies.RemoveAt(closestEnemyIndex);
                }


            }
        }
    }
    void convertEnemy(Collider2D[] EnemiesArray)
    {
        foreach (Collider2D Enemy in EnemiesArray)
        {
            if (Enemy.gameObject.CompareTag("Enemy"))                           //filter out player
            {
                Enemies.Add(Enemy.gameObject);
                totalEnemies++;
            }
        }

    }


}
