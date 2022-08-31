using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [Header("Player Object")][Space]
    [SerializeField] PlayerSO playerScriptableObject;
    [Space][Space]

    [Header("Variables")][Space]
	[Range(0, 5)][SerializeField] private float m_invicibleTime = 2.0f;
	[Space][Space]
    public float health;
    public bool isInvincible;
    // Start is called before the first frame update
    void Start()
    {
        health = playerScriptableObject.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void damage(float damageAmount)
    {
        if (isInvincible == true)
        {
            Debug.Log("Anthony gae");
        } else {
            health -= damageAmount;
            if (health <= 0)
            {
                Die();
            }
            Debug.Log("damage");
            StartCoroutine(triggerInvincible());
        }
    }

    public void Die()
    {
        //back to menu eventually gameOver Scene here
        SceneManager.LoadScene("Main menu");
    }

    public IEnumerator triggerInvincible()
	{
		isInvincible = true;
		yield return new WaitForSeconds(m_invicibleTime);
        isInvincible = false;
	}

}
