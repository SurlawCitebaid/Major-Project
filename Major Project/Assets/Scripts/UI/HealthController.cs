// Juliet Gobran

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthController : MonoBehaviour
{
    [SerializeField] PlayerController PlayerController;
    private int playerHealth;
    private int maxHearts;
    private int currentActiveHearts;

    //public Color pink = Color.FromArgb(255, 105, 249);

    [SerializeField] public GameObject[] healthHearts;
    private void Start()
    {
        
        currentActiveHearts = healthHearts.Length;
    }
    private void Update()
    {
        if(PlayerController.maxHealth > 10)
        {
            maxHearts = 10;
        } else if(PlayerController.maxHealth < 3)
        {
            maxHearts = 3;
        } else
        {
            maxHearts = PlayerController.maxHealth;
        }
        
        // get Player current health
        UpdateHealthStatus();
        UpdateMaxHealth();

    }
    public void UpdateMaxHealth()
    {
        // to reduce hearts

        if (maxHearts != currentActiveHearts)
        {
            for (int i = 0; i < healthHearts.Length; i++)
            {
                healthHearts[i].SetActive(false);
                currentActiveHearts--;  
            }
            for(int i = 0; i < maxHearts; i++)
            {
                healthHearts[i].SetActive(true);
                currentActiveHearts++;
            }
        }

    }

    public void UpdateHealthStatus()
    {
        playerHealth = PlayerController.Instance.health;
        for (int i = 0; i < healthHearts.Length; i++)
        {
            if (i < playerHealth)
            {
                healthHearts[i].GetComponent<SpriteRenderer>().color = new Color32(224,71,71,255); // RGBA
            }
            else
            {
                healthHearts[i].GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }
}
