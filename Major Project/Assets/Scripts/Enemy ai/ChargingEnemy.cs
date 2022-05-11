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
    float mvSpeed, range;
    [SerializeField] float chargeSpeed; //speed of the charging attack
    [SerializeField] float tooClose; //distance too close to attack from
    float tooFar; //distance too far to attack from
    Vector2 distance;
    Rigidbody2D rb;
    //For Testing
    Vector3 worldPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<EnemyAiController>();
        player = GameObject.FindGameObjectWithTag("Player");

        mvSpeed = enemy.moveSpeed;
        range = enemy.range;
        tooFar = range * 2;
        GetComponent<SpriteRenderer>().sprite = enemy.sprite;
        
        distance = new Vector2(player.transform.position.x - gameObject.transform.position.x, 0);
    }

    // Update is called once per frame
    void Update()
    {
        distance = new Vector2(player.transform.position.x - gameObject.transform.position.x, 0); //track the distance from the player

        Debug.Log("Current State: " + states.currentState());

        switch(states.currentState()){
            case EnemyAiController.State.MOVING:
                MoveEnemy(mvSpeed);
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

    void FixedUpdate() {
        
        
    }

    //waits before attacking 
    IEnumerator Aiming() {
        states.setState(2);
        //yield return new WaitForSecondsRealtime(2);
        float timePassed = 0;
        while (timePassed < 2f){
            if (distance.magnitude > tooFar){
                //too far away
                break;
            }

            timePassed += Time.deltaTime;
            yield return null;
        }
        
        if (timePassed < 2f){
            states.setState(0);
        } else {
            ChargePlayer();
        }
    }
    
    //Rams the player
    void ChargePlayer()
    {
        states.setState(3); //ATTACKING
        rb.velocity = new Vector2(distance.normalized.x * chargeSpeed, rb.velocity.y);
        
        StartCoroutine(CooldownAttack());
    }

//cooldown period before entity can start moving again
    IEnumerator CooldownAttack(){
        states.setState(4);//COOLDOWN
        yield return new WaitForSecondsRealtime(2);

        states.setState(0);//MOVING
    }

    void MoveEnemy(float speed){
        //states.setState(0);

        if (distance.magnitude > range) {
            //attempt to move towards player
            rb.velocity = new Vector2(distance.normalized.x * speed, rb.velocity.y);
        } else if (distance.magnitude < tooClose) {
            //attempt to move away from player
            rb.velocity = new Vector2(-distance.normalized.x * speed, rb.velocity.y);
        } else {
            //Aiming
            StartCoroutine(Aiming());
        }
    }

}
