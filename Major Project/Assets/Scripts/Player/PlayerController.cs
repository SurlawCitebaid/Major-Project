using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject gameOverUI;

    [Header("Player Object")][Space]
    [SerializeField] PlayerSO playerScriptableObject;
    [Space][Space]

    [Header("Variables")][Space]
	[Range(0, 5)][SerializeField] private float m_invicibleTime = 2.0f;
	[Space][Space]

    public int health;
    public bool isInvincible;
    private int currency = 0;
    SpriteRenderer sprite;
    Color originalColor;

    [SerializeField] private HealthController _healthController;

    // Start is called before the first frame update
    void Start()
    {
        health = playerScriptableObject.maxHealth;
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void damage(int damageAmount)
    {
        
        if (isInvincible == true)
        {
            Debug.Log("inv");
        } else {
            FindObjectOfType<AudioManager>().Play("PlayerHurt");
            health -= damageAmount;

            // health hearts status
            _healthController.playerHealth = health;
            _healthController.UpdateHealthStatus();

            originalColor = sprite.color;
            sprite.color = Color.red;
            if (health <= 0)
            {
                Die();
            }
            Debug.Log("damage");
            StartCoroutine(onHitSpriteChange());
            StartCoroutine(triggerInvincible());
        }
    }

    public void Die()
    {
        // added in gameOver UI active here & paused time
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public IEnumerator triggerInvincible()
	{
		isInvincible = true;
		yield return new WaitForSeconds(m_invicibleTime);
        isInvincible = false;
	}

    public IEnumerator onHitSpriteChange()
	{
		yield return new WaitForSeconds(0.3f);
        sprite.color = originalColor;
	}
}
