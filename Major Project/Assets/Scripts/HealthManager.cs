using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    public int maxHealth { get; private set; }
    public int health { get; private set; }


    private void Awake() {
        maxHealth = 6;
        health = maxHealth;
        //heartDisplay = GameObject.FindObjectOfType<HeartDisplay>();
    }

    public void Damage() {
        health--;
    }
}
