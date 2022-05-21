using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] float timeBetweenSpawns = 5f; //fixed time between spawns
    [SerializeField] int maxSpawns = 10; //total number of enemy spawns from this spawner
    int currentSpawns = 0;
    float countdown;//current time till spawn

    void Start() {
        countdown = timeBetweenSpawns;
    }

    void Update() {
        if (countdown < 0)
        {
            SpawnEnemy();
            countdown = timeBetweenSpawns;
        }

        countdown -= Time.deltaTime;
    }

    //continuously spawn enemies or
    //spawn a set amount in the room
    void SpawnEnemy(){
        int enemyNum = Random.Range(0, enemies.Length);

        Debug.Log("Spawning " + enemies[enemyNum].name);
        Instantiate(enemies[enemyNum], transform.position, transform.rotation);

        currentSpawns++;
        
        
        if (currentSpawns >= maxSpawns)
        {
            Debug.Log("Max Spawns Reached");
            Destroy(gameObject, 1f);
        }
    }

}
