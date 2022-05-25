using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDisplay : MonoBehaviour {
    [SerializeField] private Transform pfHeart;

    private HealthManager healthManager;

    private void Awake() {
        healthManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
    }

    private void Start() {
        for (int i = 0; i < Mathf.Ceil(healthManager.maxHealth / 2); i++) {
            Transform heart = Instantiate(pfHeart, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
        }
        UpdateHeartDisplay();
    }

    public void UpdateHeartDisplay() {
        int emptyHearts = healthManager.maxHealth - healthManager.health;
        HeartController[] hearts = GetComponentsInChildren<HeartController>();
        for (int i = hearts.Length - 1; i >= 0; i--) {
            if (emptyHearts >= 2) {
                hearts[i].SetValue(0);
                emptyHearts -= 2;
            } else if (emptyHearts == 1) {
                hearts[i].SetValue(1);
                emptyHearts--;
            } else if (emptyHearts == 0) {
                hearts[i].SetValue(2);
            }
        }
    }
}
