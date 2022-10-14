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
                    other.gameObject.GetComponent<PlayerController>().AddCurrency(10);//add currency to player
                    //play sound effect and collect particles
                    break;
                case 1:
                    other.gameObject.GetComponent<PlayerController>().RegenHealth(); //heal
                    //play sound effect and collect particles
                    break;
                default:
                break;
            }
            
            Destroy(gameObject);
        }
    }
}
