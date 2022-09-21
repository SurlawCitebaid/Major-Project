using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyController : MonoBehaviour
{
    EnemyAiController states;
    //0:MOVING 1:CHASE 2:AIMING 3:ATTACKING 4:COOLDOWN 5:STUNNED
    GameObject player;
    Vector2 distance;
    bool isGrounded;
    bool facingRight;
    [SerializeField] private LayerMask lm_ground;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private ParticleSystem dust;
    EnemyScriptableObject enemy;
    [SerializeField] Attack attack;

    private void Awake() {
        states = GetComponent<EnemyAiController>();
        player = GameObject.FindGameObjectWithTag("Player");
        attack = GetComponent<Attack>();

        distance = new Vector2(player.transform.position.x - gameObject.transform.position.x, 0); //track the distance from the player
        
        if(distance.normalized.x > 0)
            facingRight = true;
        else {
            facingRight = false;
            Flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = new Vector2(player.transform.position.x - gameObject.transform.position.x, 0); //track the distance from the player
        //GroundCheck();

        if (states.currentState() != EnemyAiController.State.COOLDOWN)
        {
            if((distance.normalized.x < 0 && facingRight) || (distance.normalized.x > 0 && !facingRight))
            Flip();
        }

        if (true){
            switch(states.currentState()){
                case EnemyAiController.State.MOVING:
                    MoveEnemy(states.enemy.moveSpeed);
                    if(states.getVelocity().x == 0){
                        StartCoroutine(Jump());
                    }
                    break;
                case EnemyAiController.State.CHASE:
                    break;
                case EnemyAiController.State.AIMING:
                    break;
                case EnemyAiController.State.ATTACKING:
                    break;
                case EnemyAiController.State.COOLDOWN:
                    break;
                case EnemyAiController.State.STUNNED:
                    MoveEnemy(states.enemy.stunSpeed);
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
        

        if (distance.magnitude > states.enemy.attack.range) {
            //attempt to move towards player
            states.changeVelocity(new Vector2(distance.normalized.x * speed, states.getYVelocity()));
        } else if (distance.magnitude < states.enemy.attack.minRange) {
            //attempt to move away from player
            states.changeVelocity(new Vector2(-distance.normalized.x * speed, states.getYVelocity()));
        } else {
            //Aiming
            StartCoroutine(Aiming());
        }
    }

    IEnumerator Aiming() {
        states.setState(2);

        float timePassed = 0;
        while (timePassed < 1f && states.currentState() == EnemyAiController.State.AIMING){
            if (distance.magnitude > states.enemy.attack.maxRange){
                //too far away
                break;
            }

            timePassed += Time.deltaTime;
            yield return null;
        }
        
        //if still in range after two seconds
        if (timePassed < 1f){
            states.setState(0);
        } else {
            Attack();
        }
    }

    void Attack()
    {
        states.setState(3); //ATTACKING
        states.setImmune(true);

        attack.DoAttack(states, distance, gameObject);

        StartCoroutine(states.Immunity());
        StartCoroutine(states.CooldownAttack(states.enemy.attack.cooldownTime,0));
    }
    
    IEnumerator Jump(){
        states.changeVelocity(states.getVelocity() + new Vector2(distance.normalized.x * states.enemy.jumpHeight, states.enemy.jumpHeight));
        dust.Play();
        yield return new WaitForSeconds(0.5f);
    }

    void Flip(){
        Vector3 scale = gameObject.transform.localScale;
        scale.x *= -1;
        facingRight = !facingRight;
        transform.localScale = scale;
        dust.Play();
    }
}
