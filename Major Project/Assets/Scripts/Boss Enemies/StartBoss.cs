using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    [SerializeField]
    GameObject boss;
    bool spawnedOnce = false;
    [SerializeField]
    GameObject bossStartParticles;
    [SerializeField]
    GameObject levelChangeDoor;


    private void Start()
    {
        EnemySpawner.enemiesAlive = false;
        transform.localScale = new Vector3(6f,6f,1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!spawnedOnce)
            {
                Vector2 spawnLocation = new Vector2(transform.position.x, transform.position.y + 4);
                GameObject.Instantiate(bossStartParticles, spawnLocation, Quaternion.identity);
                GameObject.Instantiate(boss, spawnLocation, Quaternion.identity);
            }
            GameObject.Instantiate(levelChangeDoor, new Vector3(transform.position.x, transform.position.y+1f, transform.position.z), Quaternion.identity);
            spawnedOnce = true;

            FindObjectOfType<AudioManager>().PlayMusic("MusicBossBattleLoop", "MusicBossBattleIntro");
            Destroy(gameObject);
        }
    }
}
