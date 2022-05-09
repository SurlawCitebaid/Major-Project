using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject enemy;
    [SerializeField] GameObject player;
    float mvSpeed, range;
    [SerializeField] float chargeSpeed;
    [SerializeField] float tooClose; //distance too close to attack from
    Vector2 distance;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        mvSpeed = enemy.moveSpeed;
        range = enemy.range;
        GetComponent<SpriteRenderer>().sprite = enemy.sprite;
        
        //distance = Vector2.Distance(gameObject.transform.position, player.transform.position);
        distance = new Vector2(player.transform.position.x - gameObject.transform.position.x, 0);
        //Debug.Log("Distance: " + distance);
    }

    // Update is called once per frame
    void Update()
    {
        distance = new Vector2(player.transform.position.x - gameObject.transform.position.x, 0);

        

        

        if (Input.GetKeyDown(KeyCode.Space)){
            //ChargePlayer();
            StartCoroutine(ChargePlayer());
        }
        
    }

    void FixedUpdate() {
        MoveEnemy();

        if (Input.GetKeyDown(KeyCode.Space)){
            //ChargePlayer();
            StartCoroutine(ChargePlayer());
        }
    }

    void DistCheck() {
        if (distance.magnitude < range || distance.magnitude > 2){
            Debug.Log("STILL IN CHARGE AREA");
            //StartCoroutine(ChargePlayer());
        } else {
            Debug.Log("Cannot Charge");
        }
    }
    
    IEnumerator ChargePlayer()
    {
        

        float timePassed = 0;
        while (timePassed < 0.5f){
            //rb.AddForce(distance.normalized * chargeSpeed);
            rb.velocity = new Vector2(distance.normalized.x * chargeSpeed, rb.velocity.y);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }

    void MoveEnemy(){
        if (distance.magnitude > range){//attempt to move towards player
            rb.velocity = new Vector2(distance.normalized.x * mvSpeed, rb.velocity.y);
            //rb.AddForce(distance.normalized * mvSpeed);
        } else if (distance.magnitude < tooClose){ //attempt to move away from player
            rb.velocity = new Vector2(-distance.normalized.x * mvSpeed, rb.velocity.y);
            //rb.AddForce(-distance.normalized * mvSpeed);
        } else {
            //Invoke("DistCheck", 2.0f);
        } 
    }

}
