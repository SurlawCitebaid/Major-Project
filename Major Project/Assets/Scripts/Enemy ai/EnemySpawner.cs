using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject[] enemies; //list of enemies that can spawn here
    [SerializeField] float timeBetweenSpawns = 5f; //fixed time between spawns
    float countdown;//current between spawns

    void Start() {
        countdown = timeBetweenSpawns;
    }

    void Update() {
        if (countdown < 0)
        {
            //SpawnEnemy();
            countdown = timeBetweenSpawns;
        }

        countdown -= Time.deltaTime;
    }

    //continuously spawn enemies or
    //spawn a set amount in the room
    void SpawnEnemy(){
        
    }

}
