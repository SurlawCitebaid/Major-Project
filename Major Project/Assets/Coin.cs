using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] private Consumable consumable;
    private int value;

    private void Start() {
        this.gameObject.GetComponent<SpriteRenderer>().color = consumable.colour;
        value = consumable.value;
    }
}
