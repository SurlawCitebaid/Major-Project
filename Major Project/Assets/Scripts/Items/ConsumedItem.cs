using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumedItem : MonoBehaviour
{
    [SerializeField] private int id;
    bool inside = false;
    float force = 6f;
    float timeStamp;
    Transform magnet;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            
            PickUpItem(id);
            Destroy(gameObject);
        } else if (other.gameObject.CompareTag("Magnet")){
            timeStamp = Time.time;
            magnet = other.gameObject.transform;
            inside = true;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    private void Update() {
        if (inside) {
            Vector2 magnetDir = (magnet.position - transform.position);
            GetComponent<Rigidbody2D>().velocity = magnetDir.normalized * force * (Time.time + timeStamp)/timeStamp;
        }
    }

    void PickUpItem(int id){
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        switch (id){
                case 0:
                    int value = Random.Range(1, 10);
                    player.GetComponent<PlayerController>().AddCurrency(value);//add currency to player
                    FindObjectOfType<AudioManager>().Play("CoinCollect");
                    //play collect particles
                    break;
                case 1:
                    player.GetComponent<PlayerController>().RegenHealth(); //heal
                    FindObjectOfType<AudioManager>().Play("Heal");
                    //play collect particles
                    break;
                default:
                break;
            }
    }
}
