using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumedItem : MonoBehaviour
{
    [SerializeField] private int id;
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (id){
                case 0:
                    int value = Random.Range(1, 10);
                    other.gameObject.GetComponent<PlayerController>().AddCurrency(value);//add currency to player
                    FindObjectOfType<AudioManager>().Play("CoinCollect");
                    //play collect particles
                    break;
                case 1:
                    other.gameObject.GetComponent<PlayerController>().RegenHealth(); //heal
                    FindObjectOfType<AudioManager>().Play("Heal");
                    //play collect particles
                    break;
                default:
                break;
            }
            
            Destroy(gameObject);
        }
    }
}
