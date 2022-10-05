// Juliet Gobran

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;

public class HealthController : MonoBehaviour
{
    [SerializeField] PlayerSO playerScriptableObject;
    public int playerHealth;
    private int activeHearts;

    //public Color pink = Color.FromArgb(255, 105, 249);

    [SerializeField] public Image[] healthHearts;

    private void Start()
    {
        UpdateHealthStatus();
        UpdateMaxHealth();
        activeHearts = playerScriptableObject.maxHealth;
    }

    public void UpdateMaxHealth()
    {
        // to reduce hearts
        if (playerScriptableObject.maxHealth < activeHearts)
        {
            for (int i = 0; i < healthHearts.Length; i++)
            {
                if (i > playerScriptableObject.maxHealth)
                {
                    healthHearts[i].gameObject.SetActive(false);
                    activeHearts--;
                }
            }
        }

        // to increase hearts
        if (playerScriptableObject.maxHealth > activeHearts)
        {
            for (int i = 0; i < healthHearts.Length; i++)
            {
                if (i < playerScriptableObject.maxHealth)
                {
                    healthHearts[i].gameObject.SetActive(true);
                    activeHearts++;
                }
            }
        }

        UpdateHealthStatus();
    }

    public void UpdateHealthStatus()
    {
        for (int i = 0; i < healthHearts.Length; i++)
        {
            if (i < playerHealth)
            {
                healthHearts[i].color = new Color32(224,71,71,255); // RGBA
            }
            else
            {
                healthHearts[i].color = Color.black;
            }
        }
    }
}
