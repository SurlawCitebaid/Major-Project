// Juliet Gobran
// Script for Health status

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class HealthController : MonoBehaviour
{
    [SerializeField] PlayerController PlayerController;
    public GameObject currencyObject;
    public int currency;
    private int playerHealth, maxHearts, currentActiveHearts;

    public GameObject[] healthHearts; // displayed hearts array 

    private void Start()
    {
        currentActiveHearts = healthHearts.Length;
        currency = PlayerController.currency;
        
    }

    private void Update()
    {
        currencyObject.GetComponent<TMP_Text>().text = "$ "+PlayerController.Instance.getCurrency().ToString();
        if (PlayerController.Instance.maxHealth > 10)
        {
            maxHearts = 10;
        } else if(PlayerController.Instance.maxHealth < 3)
        {
            maxHearts = 3;
        } else
        {
            maxHearts = PlayerController.Instance.maxHealth;
        }
        
        // get Player current health
        UpdateHealthStatus();
        UpdateMaxHealth();

    }


    public void UpdateMaxHealth()
    {
        if (maxHearts != currentActiveHearts)
        {
            // increase max health
            for (int i = 0; i < healthHearts.Length; i++)
            {
                healthHearts[i].SetActive(false);
                currentActiveHearts--;  
            }
            // decrease max health
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

        // update displayed hearts
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
