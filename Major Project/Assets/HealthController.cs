// Juliet Gobran

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int playerHealth;
    //Color pink = Color.FromArgb(255, 105, 249);

    [SerializeField] private Image[] healthHearts;

    private void Start()
    {
        UpdateHealthStatus();
    }

    private void UpdateHealthStatus()
    {
        for (int i = 0; i < healthHearts.Length; i++)
        {
            if (i < playerHealth)
            {
                healthHearts[i].color = Color.red;
            }
            else
            {
                healthHearts[i].color = Color.black;
            }
        }
    }
}
