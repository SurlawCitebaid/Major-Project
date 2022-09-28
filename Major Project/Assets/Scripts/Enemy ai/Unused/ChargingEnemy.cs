using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject enemy;
    EnemyAiController states;
    //0:MOVING 1:CHASE 2:AIMING 3:ATTACKING 4:COOLDOWN 5:STUNNED
    GameObject player;
    float chargeSpeed = 20f; //speed of the charging attack
    Vector2 distance;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<EnemyAiController>();
        player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<SpriteRenderer>().sprite = enemy.sprite;
        rb.drag = UnityEngine.Random.Range(1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        distance = new Vector2(player.transform.position.x - gameObject.transform.position.x, 0); //track the distance from the player

        switch(states.CurrentState()){
            case EnemyAiController.State.MOVING:
                MoveEnemy(enemy.moveSpeed);
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
                MoveEnemy(enemy.stunSpeed);
                break;
        }
    }


    //waits before attacking 
    IEnumerator Aiming() {
        states.SetState(2);

        float timePassed = 0;
        while (timePassed < 2f){
            if (distance.magnitude > enemy.attack.maxRange){
                //too far away
                break;
            }

            timePassed += Time.deltaTime;
            yield return null;
        }
        
        //if still in range after two seconds
        if (timePassed < 2f){
            states.SetState(0);
        } else {
            ChargePlayer();
        }
    }
    
    //Rams the player
    void ChargePlayer()
    {
        states.SetState(3); //ATTACKING
        rb.velocity = new Vector2(distance.normalized.x * chargeSpeed, rb.velocity.y);
        
        StartCoroutine(CooldownAttack());
    }

//cooldown period before entity can start moving again
    IEnumerator CooldownAttack(){
        states.SetState(4);//COOLDOWN
        yield return new WaitForSecondsRealtime(2);

        states.SetState(0);//MOVING
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
    
}