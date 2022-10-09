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
    public float playerSpeed;
    public bool isInvincible;
    public float baseDamage;
    public int maxHealth;
    public int health;

    private int currency = 0;
    SpriteRenderer sprite;
    Color originalColor;


    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        GetComponent<movement_Mario>().setMoveSpeed(playerScriptableObject.speed);
        playerSpeed = playerScriptableObject.speed;
        baseDamage = playerScriptableObject.damage;
        maxHealth = playerScriptableObject.maxHealth;               // change this after start to change UI maxHearts
        health = maxHealth;
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        foreach(ItemValues item in Inventory.instance.inventory)
        {
            if (item.GetName() == "Worry Doll" || item.GetName() == "Cordial" || item.GetName() == "Feather")
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
}
