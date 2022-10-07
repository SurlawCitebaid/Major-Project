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
    //Shake effect
    Vector2 originalPosition;
    bool startEffect = false;
    float bossShakeSpeed = 10f;
    Vector2 spawnLocation;
    float currentTime;
    bool inTrigger = false;


    private void Start()
    {
        EnemySpawner.enemiesAlive = false;
        transform.localScale = new Vector3(6f,6f,1f);
        originalPosition = transform.position;
        spawnLocation = new Vector2(transform.position.x, transform.position.y + 4);
    }

    private void Update()
    {

        //Start effect
        if (startEffect)
        {
            //Shakes
            //Vector2 translation = new Vector2(Random.Range(-1f, 1f), 0) * bossShakeSpeed * Time.deltaTime;
            //transform.Translate(translation);
            
            //Go up to spawn location
            currentTime += Time.deltaTime;
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(originalPosition.y, originalPosition.y + 4, currentTime / 6.5f));
        }

        //Press E to start boss
        if (inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            //Start the stone boss effect
            startEffect = true;
            if (!spawnedOnce)
            {
                //Turns on the particles
                transform.Find("Stone Shatter Effect").gameObject.SetActive(true);
                FindObjectOfType<AudioManager>().PlayMusic("MusicBossBattleLoop", "MusicBossBattleIntro");
                StartCoroutine(delayBossSpawn());
            }
            spawnedOnce = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Interactable Notfication
            collision.transform.Find("interact").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            inTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Interactable Notfication
            collision.transform.Find("interact").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            inTrigger = false;
        }
    }

    

    IEnumerator delayBossSpawn()
    {
        //Delayed to match intro
        yield return new WaitForSeconds(6.5f);

        
        GameObject.Instantiate(bossStartParticles, spawnLocation, Quaternion.identity);
        GameObject.Instantiate(boss, spawnLocation, Quaternion.identity);

        GameObject.Instantiate(levelChangeDoor, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
        

        Destroy(gameObject);
    }

}
