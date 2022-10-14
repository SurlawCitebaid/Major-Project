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
    [Space]
    [Space]

    public static PlayerController Instance;
    public float playerSpeed, playerJumpForce;
    public bool isInvincible ,zapOn;
    public float baseDamage;
    public int maxHealth, zaps;
    public int health;
    public int currency;
    SpriteRenderer sprite;
    Color originalColor;

    void Awake()
    {
        Instance = this;

        sprite = gameObject.GetComponent<SpriteRenderer>();
        GetComponent<movement_Mario>().setMoveSpeed(playerScriptableObject.speed);
        GetComponent<movement_Mario>().SetJumpForce(playerScriptableObject.jumpForce);

        playerSpeed = playerScriptableObject.speed;
        baseDamage = playerScriptableObject.damage;
        maxHealth = playerScriptableObject.maxHealth;                                       //base values
        currency = playerScriptableObject.currency;
        playerJumpForce = playerScriptableObject.jumpForce;
        health = maxHealth;


        currency += 100;
    }
    private void Update()
    {
        foreach(ItemValues item in Inventory.instance.inventory)
        {
            if (item.GetName() == "Worry Doll" || item.GetName() == "Cordial" || item.GetName() == "Feather" || item.GetName() == "Spring" || item.GetName() == "Battery")
            {
                if(item.GetName() == "Cordial")
                {
                    maxHealth = item.GetAmount() + playerScriptableObject.maxHealth;
                }
                if(item.GetName() == "Worry Doll")
                {
                    baseDamage = (1+item.GetAmount()) * playerScriptableObject.damage;
                }
                if (item.GetName() == "Feather")
                {
                    playerSpeed = (50 * item.GetAmount()) + playerScriptableObject.speed;
                    GetComponent<movement_Mario>().setMoveSpeed(playerSpeed);
                }
                if (item.GetName() == "Spring")
                {
                    playerJumpForce = item.GetAmount() + playerScriptableObject.jumpForce;
                    GetComponent<movement_Mario>().SetJumpForce(playerJumpForce);
                }
                if (item.GetName() == "Battery")
                {            
                    zapOn = true;
                    zaps = item.GetAmount()+3;

                }
            }
            else continue;
        }

        
    }
    // Update is called once per frame
    public void damage(int damageAmount)
    {
        
        if (isInvincible == true)
        {
        } else {
            FindObjectOfType<AudioManager>().Play("PlayerHurt");
            health -= damageAmount;

            // health hearts status

            originalColor = sprite.color;
            sprite.color = Color.red;
            if (health <= 0)
            {
                Die();
            }
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
    public int getCurrency()
    {
        return currency;
    }
    public void SetCurrency(int value)
    {
        currency = value;
    }

    public void AddCurrency(int amount){ //can be used with negative values too
        currency += amount;
    }

    public void RegenHealth(){ //heal 1 heart
        health++;
        if(health > maxHealth){
            health = maxHealth;
        }
    }
}
