using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] float timeBetweenSpawns = 5f; //fixed time between spawns
    [SerializeField] int startSpawns = 1;
    [SerializeField] float difficultyValue = 30;//increasing this will increase the spawn rate faster

    public static int maxSpawns;
    public static int currentSpawned = 0;
    public static int totalEnemiesAlive;
    public static bool enemiesAlive = true;
    public static bool unlockDoorsOnce = true;
    float countdown;//current time till spawn

    //Spawning Scaling
    float scalingMaxSpawns = 0;
    float playerFactor = 1f + 0.3f * (1 - 1);
    float timeFactor;
    float constantFactor = 1.15f;
    float spawnCoeff;

    void Start() {
        countdown = timeBetweenSpawns;
        timeFactor = 0.0506f * difficultyValue * 1 * 0.2f;
        maxSpawns = startSpawns;
    }

    void Update() {
        //Updates coeficient for spawning
        spawnCoeff = (playerFactor + Time.realtimeSinceStartup / 60 * timeFactor) * constantFactor;
        scalingMaxSpawns = startSpawns * spawnCoeff;

        if(!enemiesAlive)
        {
            if (unlockDoorsOnce)
            {
                unlockDoorsOnce = false;
                Door.unlockDoors();
            }
            
            currentSpawned = 0;
            totalEnemiesAlive = 0;
            maxSpawns = (int)scalingMaxSpawns;
        }
        //enemies alive
        else if(enemiesAlive)
        {
            //Spawn until the max is reached
            if(currentSpawned < maxSpawns)
            {
                if (countdown < 0)
                {
                    SpawnEnemy();
                    countdown = timeBetweenSpawns;
                }
                countdown -= Time.deltaTime;
            }
        } 
    }

    //continuously spawn enemies or
    //spawn a set amount in the room
    void SpawnEnemy(){
        int enemyNum = Random.Range(0, enemies.Length);

        if (Room.enemyLocationValid(transform.position))
        {
            Instantiate(enemies[enemyNum], transform.position, transform.rotation);

            currentSpawned++;
            totalEnemiesAlive++;
        }
        
    }

}
