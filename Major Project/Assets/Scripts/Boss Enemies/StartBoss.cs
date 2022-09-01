using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    [SerializeField]
    GameObject boss;
    bool spawnedOnce = false;

    private void Start()
    {
        EnemySpawner.enemiesAlive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!spawnedOnce)
            {
                Vector2 spawnLocation = new Vector2(transform.position.x, transform.position.y + 4);
                GameObject.Instantiate(boss, spawnLocation, Quaternion.identity);
            }
            
            spawnedOnce = true;
        }
    }
}
