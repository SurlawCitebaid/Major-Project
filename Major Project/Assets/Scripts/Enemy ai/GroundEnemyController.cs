using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyController : MonoBehaviour
{
    [SerializeField] private Color32 defaultColour = new Color32();
    [SerializeField] EnemyScriptableObject enemy;
    EnemyAIController states;
    //0:MOVING 1:CHASE 2:AIMING 3:ATTACKING 4:COOLDOWN 5:STUNNED
    GameObject player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    
    Vector2 distance;
    [SerializeField] bool immune; //enemy immune to stun, during attack
    bool isGrounded;
    [SerializeField] private LayerMask lm_ground;
    [SerializeField] private Transform groundCheckPos;

    //Local Enemy Values
    int health;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        states = GetComponent<EnemyAIController>();
        player = GameObject.FindGameObjectWithTag("Player");

        sr.color = defaultColour;
        sr.sprite = enemy.sprite;
        health = enemy.health;

        rb.drag = UnityEngine.Random.Range(1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        distance = new Vector2(player.transform.position.x - gameObject.transform.position.x, 0); //track the distance from the player
        GroundCheck();

        if (isGrounded){
            switch(states.currentState()){
                case EnemyAIController.State.MOVING:
                    MoveEnemy(enemy.moveSpeed);
                    break;
                case EnemyAIController.State.CHASE:
                    break;
                case EnemyAIController.State.AIMING:
                    break;
                case EnemyAIController.State.ATTACKING:
                    break;
                case EnemyAIController.State.COOLDOWN:
                    break;
                case EnemyAIController.State.STUNNED:
                    MoveEnemy(enemy.stunSpeed);
                    break;
            }
        }
        
    }

    private void GroundCheck() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(groundCheckPos.position.x, groundCheckPos.position.y), 0.1f, lm_ground);
        if (hits.Length > 0)
            isGrounded = true;
        else {
            isGrounded = false;
        }
    }

    void MoveEnemy(float speed){
        //states.setState(0);

        if (distance.magnitude > enemy.attack.range) {
            //attempt to move towards player
            rb.velocity = new Vector2(distance.normalized.x * speed, rb.velocity.y);
        } else if (distance.magnitude < enemy.attack.minRange) {
            //attempt to move away from player
            rb.velocity = new Vector2(-distance.normalized.x * speed, rb.velocity.y);
        } else {
            //Aiming
            StartCoroutine(Aiming());
        }
    }

    IEnumerator Aiming() {
        states.setState(2);

        float timePassed = 0;
        while (timePassed < 2f && states.currentState() == EnemyAIController.State.AIMING){
            if (distance.magnitude > enemy.attack.maxRange){
                //too far away
                break;
            }

            timePassed += Time.deltaTime;
            yield return null;
        }
        
        //if still in range after two seconds
        if (timePassed < 2f){
            states.setState(0);
        } else {
            Attack();
        }
    }

    void Attack()
    {
        states.setState(3); //ATTACKING
        immune = true;

        switch (enemy.attack.name) {
            case "Charge":
                float chargeSpeed = 20f; //speed of the charging attack
                rb.velocity = new Vector2(distance.normalized.x * chargeSpeed, rb.velocity.y);
            break;

            case "Swipe":
                Debug.Log("Swiper no swipe yet");
            break;

            default:
                Debug.Log("No attack type for " + enemy.name);
            break;
        }
        StartCoroutine(Immunity());
        StartCoroutine(CooldownAttack());
    }

    IEnumerator CooldownAttack(){
        states.setState(4);//COOLDOWN
        yield return new WaitForSeconds(enemy.attack.cooldownTime);

        states.setState(0);//MOVING
    }

    IEnumerator Immunity(){
        yield return new WaitForSeconds(enemy.attack.immunityTime);
        immune = false;
    }
    
    public void Damage(float damageAmount, float knockbackForce, float knockbackDirection) {
        if (immune) return;

        Debug.Log("Hit for " + damageAmount);
        health -= (int)damageAmount;//damageAmount may be changed to int
        Knockback(knockbackForce, knockbackDirection);
        StartCoroutine(HitFlash());

        if (health < 0)
            Die();
    }

    private void Knockback(float knockbackForce, float knockbackDirection) {
        rb.AddForce(Vector2.right * knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private IEnumerator HitFlash() {
        states.setState(5);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        sr.color = defaultColour;

        states.setState(0);
    }

    void Die(){
        Destroy(gameObject, 0.5f);
    }
}
