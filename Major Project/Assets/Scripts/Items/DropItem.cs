using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] GameObject moneyDrop;
    [SerializeField] GameObject[] possibleDrops;
    [SerializeField][Range(0f, 1f)] float dropChance = 0.2f; //20% at base to drop random consumable
    
    public void Drop(GameObject fromObject){
        int coinsToDrop = Random.Range(1, 4);
        for(int i = 0; i < coinsToDrop; i++)
            SpawnDrop(moneyDrop, fromObject);
        
        float chance = Random.Range(0f, 1f);
        if (chance < dropChance){
            int toDrop = Random.Range(0, possibleDrops.Length);
            SpawnDrop(possibleDrops[toDrop], fromObject);
        }

        FindObjectOfType<AudioManager>().Play("CoinJingle");
    }

    public void DropBoss(GameObject fromObject){
        for(int i = 0; i < 20; i++)
            SpawnDrop(moneyDrop, fromObject);

        FindObjectOfType<AudioManager>().Play("CoinJingle");
    }

    void SpawnDrop(GameObject drop, GameObject from){ //instantiate item
        GameObject spawned = Instantiate(drop, from.transform.position, drop.transform.rotation);
        float xShift = Random.Range(-1.5f, 1.5f); //how much the object moves sideways when spawned
        float yShift = Random.Range(2.5f, 4.5f);
        spawned.GetComponent<Rigidbody2D>().velocity = new Vector2(xShift, yShift);
    }
}
