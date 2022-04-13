using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float thrust = 4f, speed = 3f, attackRange = 4f, flightHeight, yPos, angle;
    private GameObject player;
    private enum State { MOVING, CHASE, AIMING, ATTACKING, COOLDOWN };
    private State state;
    private bool inPosition = false, attacked = false, aiming = false;
    private Quaternion q, x;
    private Vector3 vectorToTarget, startRot;
    // Start is called before the first frame update
    void Start()
    {
        state = State.MOVING;
        startRot = transform.position;
        x = transform.rotation;
        yPos = transform.position.y;                                    //consistent yPos
        rigid = GetComponent<Rigidbody2D>();                                //get ai physics
        flightHeight = Random.Range(0f, 8f);
        player = GameObject.FindGameObjectWithTag("Player");                //so ai knows where player is
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null)
            return;

        switch (state)
        {
            case State.MOVING:
                if (yPos < player.transform.position.y)
                {
                    rePosition();
                }
                else if (yPos >= player.transform.position.y)
                {
                    state = State.CHASE;

                }
                break;
            case State.CHASE:
                flightHeight = Random.Range(0f, 9f);
                float dist = Mathf.Abs(transform.position.x - player.transform.position.x);
                if (dist > attackRange)
                {
                    chase();
                } else
                {
                    state = State.AIMING;
                }
                break;
            case State.AIMING:

                aimAttack();
                break;
            case State.ATTACKING:
                Invoke("attack", 2f);
                break;
        }
    }
    private void aimAttack()
    {
        vectorToTarget = player.transform.position - transform.position;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        if(transform.rotation == q)
        {
            state = State.ATTACKING;
        }
    }
    private void attack()
    {
        rigid.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        
    }
    private void chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speed * Time.deltaTime);

    }
    private void rePosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, player.transform.position.y + flightHeight), speed * Time.deltaTime);
        if (Mathf.Abs(transform.position.y) == Mathf.Abs(flightHeight + player.transform.position.y)) //you cant compare float values by themselves
        {
            yPos = transform.position.y;
        }
    }
    private void reset()
    {
        yPos = transform.position.y;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("ASS");
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = 0f;
    }
}
