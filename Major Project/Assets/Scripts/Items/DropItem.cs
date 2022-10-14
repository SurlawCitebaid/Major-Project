using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] GameObject[] possibleDrops;
    [SerializeField][Range(0f, 1f)] float dropChance = 0.2f; //20%
    float coinDropChance = 0.8f;
    
    public void Drop(GameObject fromObject){
        float chance = Random.Range(0f, 1f);
        if (chance < dropChance){
            //drop item
            Debug.Log("Item Dropped at: " + chance);
            int toDrop = Random.Range(0, possibleDrops.Length);
            //instantiate item
            GameObject drop = Instantiate(possibleDrops[toDrop], fromObject.transform.position, possibleDrops[toDrop].transform.rotation);
            drop.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 3f);
        } else {
            //Item not dropped
        }
    }
}
