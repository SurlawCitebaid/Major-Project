using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyController : MonoBehaviour
{


    EnemyAIController states;
    //0:MOVING 1:CHASE 2:AIMING 3:ATTACKING 4:COOLDOWN 5:STUNNED
    GameObject player;



    Vector2 distance;
    
    bool isGrounded;
    [SerializeField] private LayerMask lm_ground;
    [SerializeField] private Transform groundCheckPos;



    private void Awake() {


        states = GetComponent<EnemyAIController>();
        player = GameObject.FindGameObjectWithTag("Player");






    }

    // Update is called once per frame
    void Update()
    {
        distance = new Vector2(player.transform.position.x - gameObject.transform.position.x, 0); //track the distance from the player
        GroundCheck();

        if (isGrounded){
            switch(states.currentState()){
                case EnemyAIController.State.MOVING:
                    MoveEnemy(states.enemy.moveSpeed);
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
        while (timePassed < 2f && states.currentState() == EnemyAIController.State.AIMING){
            if (distance.magnitude > states.enemy.attack.maxRange){
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
        states.setImmune(true);

        switch (states.enemy.attack.name) {
            case "Charge":
                float chargeSpeed = 20f; //speed of the charging attack
                states.changeVelocity(new Vector2(distance.normalized.x * chargeSpeed, states.getYVelocity()));
            break;

            case "Swipe":
                Debug.Log("Swiper no swipe yet");
            break;

            default:
                Debug.Log("No attack type for " + states.enemy.name);
            break;
        }
        StartCoroutine(states.Immunity());
        StartCoroutine(states.CooldownAttack(states.enemy.attack.cooldownTime,0));
    }
    




}
