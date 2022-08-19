using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    PlayerSO playerScriptableObject;
    public float health;
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
        health -= damageAmount;
        if (health <= 0)
            Die();
    }

    public void Die()
    {
        //back to menu eventually gameOver Scene here
        SceneManager.LoadScene("Main menu");
    }

}
