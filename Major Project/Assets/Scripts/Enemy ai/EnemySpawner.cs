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
    float timeFactor, timeFactor1;
    float constantFactor = 1.15f;
    public float spawnCoeff;
    public float heathScaler = 1;

    void Start() {
        countdown = timeBetweenSpawns;
        timeFactor = 0.0506f * difficultyValue * 1 * 0.2f;
        timeFactor1 = 0.0506f * difficultyValue/10 * 1 * 0.2f;
        maxSpawns = startSpawns;
        unlockDoorsOnce = true;
    }

    void Update() {
        //Updates coeficient for spawning
        spawnCoeff = (playerFactor + PlayerController.Instance.timeAlive / 60 * timeFactor) * constantFactor;
        scalingMaxSpawns = startSpawns * spawnCoeff;

        //Scales health
        spawnCoeff = (playerFactor + PlayerController.Instance.timeAlive / 60 * timeFactor1) * constantFactor;
        heathScaler = 1 * spawnCoeff;
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
        //enemies alive and current room isn't visted
        else if(enemiesAlive && !Door.isVisted(GenerateLevel.currentPlayerRoom))
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
            GameObject enemySpawned = Instantiate(enemies[enemyNum], transform.position, transform.rotation);
            StartCoroutine(delayScalingHealth(enemySpawned));
            currentSpawned++;
            totalEnemiesAlive++;
        }
        
    }

    IEnumerator delayScalingHealth(GameObject enemySpawned)
    {
        yield return new WaitForSeconds(0.1f);
        float newHealth = (enemySpawned.GetComponent<EnemyAiController>().Health * heathScaler);
        enemySpawned.GetComponent<EnemyAiController>().Health = (int)newHealth;
        Debug.Log("New Heath " + newHealth);
    }

    
}
