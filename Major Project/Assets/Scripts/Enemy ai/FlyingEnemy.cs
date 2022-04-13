using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float thrust = 8f, speed = 3f, attackRange = 4f, flightHeight, yPos, angle;
    private GameObject player;
    private bool inPosition = false, attacked = false, aiming = false;
    private Quaternion q, x;
    private Vector3 vectorToTarget, startRot;
    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.position;
        x = transform.rotation;
        yPos = transform.position.y;                                    //consistent yPos
        rigid = GetComponent<Rigidbody2D>();                                //get ai physics
        flightHeight = Random.Range(1f, 4f);
        player = GameObject.FindGameObjectWithTag("Player");                //so ai knows where player is
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null)
            return;
        if (yPos < player.transform.position.y && inPosition == false)              //true false false 
        {
            rePosition();                                           //enemy below player
        }
        else if (yPos >= player.transform.position.y && inPosition == false)        //true false false 
        {
            inPosition = true;                                       //enemy above player
        }
        else
        {
            flightHeight = Random.Range(0f, 4f);
            float dist = Mathf.Abs(transform.position.x - player.transform.position.x);
            if (dist > attackRange && !attacked)
            {
                chase();                                                        //true true true 
            }
            else
            {
                if (aiming)
                {
                    aimAttack();
                }
                Invoke("attack", 2f);                                       //true false true 
            }
        }
    }
    private void aimAttack()
    {
        vectorToTarget = player.transform.position - transform.position;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

    }
    private void attack()
    {
        if (attacked == false)
        {
            attacked = true;
            aiming = false;
            rigid.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }
    }
    private void chase()
    {
        aiming = true;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speed * Time.deltaTime);

    }
    private void rePosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, player.transform.position.y + flightHeight), speed * Time.deltaTime);
        if (Mathf.Abs(transform.position.y) == Mathf.Abs(flightHeight + player.transform.position.y)) //you cant compare float values by themselves
        {
            yPos = transform.position.y;
            attacked = false;
        }
    }
    private void reset()
    {
        yPos = transform.position.y;
        inPosition = false;

    }
    IEnumerator resetPos()
    {

        float elapsedTime = 0.0f;
        float time = 100f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, x, (elapsedTime / time));
        }
        yield return new WaitForSeconds(1f);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = 0f;
        StartCoroutine(resetPos());
        Invoke("reset", 3f);
    }
}
