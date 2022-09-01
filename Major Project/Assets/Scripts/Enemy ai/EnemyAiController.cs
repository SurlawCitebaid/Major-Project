using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiController : MonoBehaviour
{
    public enum State { MOVING, CHASE, AIMING, ATTACKING, COOLDOWN, STUNNED };

    private State state = State.MOVING;
    [SerializeField] private Color32 defaultColour = new Color32();
    [SerializeField] public EnemyScriptableObject enemy;
    [SerializeField] private bool immune; //enemy immune to stun, during attack
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    //Local Enemy Values
    public int health;
    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        health = enemy.health;
        sr = GetComponent<SpriteRenderer>();
        sr.color = defaultColour;
        sr.sprite = enemy.sprite;                //move this later
        rb.drag = UnityEngine.Random.Range(1f, 2f);
    }
    public float getYVelocity()
    {
        return rb.velocity.y;
    }

    public Vector2 getVelocity(){
        return rb.velocity;
    }
    public void changeVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }
    public void setState(int changeState)
    {
        state = (State) changeState;
    }
    public void setImmune(bool state)
    {
        immune = state;
    }
    public State currentState()
    {
        return state;
    }
    public void Die()
    {
        //Minus an enemy alive
        EnemySpawner.totalEnemiesAlive--;
        if(EnemySpawner.totalEnemiesAlive == 0)
        {
            EnemySpawner.enemiesAlive = false;
        }
        //if the scriptable object death particles have been set do explosion
        Debug.Log("dead");
        if (enemy.enemyDeathParticles != null)
        {
            GameObject.Instantiate(enemy.enemyDeathParticles, gameObject.transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
    public IEnumerator HitFlash(SpriteRenderer sprite , Color32 originalColor)
    {
        setState(5);
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.color = originalColor;

        setState(0);
    }
    public IEnumerator damageIndicator(SpriteRenderer sprite, Color32 originalColor)
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.color = originalColor;
    }
    public IEnumerator CooldownAttack(float cooldownTime, int newStateIndex)
    {
        setState(4);//COOLDOWN
        yield return new WaitForSeconds(cooldownTime);

        setState(newStateIndex);// state set based on enemy data
    }
    public IEnumerator Immunity()
    {
        yield return new WaitForSeconds(enemy.attack.immunityTime);
        immune = false;
    }
    public void Damage(float damageAmount, float knockbackForce, float knockbackDirection)
    {
        if (immune) return;

        Debug.Log("Hit for " + damageAmount);
        health -= (int)damageAmount;//damageAmount may be changed to int
        if (knockbackForce != 0)
        {
            Knockback(knockbackForce, knockbackDirection);
        }
        StartCoroutine(damageIndicator(sr, defaultColour));

        if (health < 0)
            Die();
    }
    public void Knockback(float knockbackForce, float knockbackDirection)
    {
        rb.AddForce(Vector2.right * knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
}