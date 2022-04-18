using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float thrust = 14f, speed = 5f, attackRange = 7f, flightHeight, yPos, angle;
    private GameObject player;
    private enum State { MOVING, CHASE, AIMING, ATTACKING, COOLDOWN };
    private State state;
    private bool attacked = false, predictionLine = true;
    private Quaternion q, x;
    private Vector3 vectorToTarget, startRot;
    public LineRenderer lineOfSight;
    private LineRendererController lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRendererController>();
        state = State.MOVING;
        startRot = transform.position;
        x = transform.rotation;
        yPos = transform.position.y;                                    //consistent yPos
        rigid = GetComponent<Rigidbody2D>();                                //get ai physics
        flightHeight = Random.Range(1f, 9f);
        player = GameObject.FindGameObjectWithTag("Player");                //so ai knows where player is
    }

    // Update is called once per frame 
    void Update()
    {
        switch (state)
        {
            case State.MOVING:
                if (yPos <= player.transform.position.y)
                {
                    rePosition();
                }
                else 
                {
                    state = State.CHASE;

                }
                break;
            case State.CHASE:
                float dist = Mathf.Abs(transform.position.x - player.transform.position.x);
                if (dist > attackRange)
                {
                    chase();
                }
                else
                {
                    state = State.AIMING;
                }
                break;
            case State.AIMING:
                aimAttack();
                break;
            case State.ATTACKING:

                Vector3 endPoint = new Vector3(0, 0, 0);
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up));

                if (hitInfo.transform.tag == "Wall")
                {
                    endPoint = hitInfo.point;
                }

                if(predictionLine)
                { 
                    lr.DrawLine(new Vector3(transform.position.x, transform.position.y, 1), new Vector3(hitInfo.point.x, hitInfo.point.y, 1));
                    predictionLine = false;
                }

                lr.updateStartPoint(transform.position);
                Invoke("attack", 1f);
                break;

            case State.COOLDOWN:
                Invoke("reset", 1f);
                break;
        }
    }
    private void aimAttack()
    {
        Vector3 dirFromAtoB = (player.transform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.up);

        vectorToTarget = player.transform.position - transform.position;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        if (dotProd == 1)
        {
            state = State.ATTACKING;
        }
    }
    private void attack()
    {
        if (!attacked)
        {
            rigid.AddForce(transform.up * thrust, ForceMode2D.Impulse);
            attacked = true;
        }
    }
    private void chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speed * Time.deltaTime);

    }
    private void rePosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, player.transform.position.y + flightHeight), speed * Time.deltaTime);
        if (Mathf.Round(transform.position.y) == Mathf.Round((flightHeight + player.transform.position.y))) //you cant compare float values by themselves
        {
            
            yPos = transform.position.y;
        }
    }
    private void reset()
    {

        if (transform.rotation != x)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, x, Time.deltaTime * speed);
        }
        else
        {
            flightHeight = Random.Range(1f, 9f);
            yPos = transform.position.y;
            state = State.MOVING;
            attacked = false;
            predictionLine = true;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = 0f;
            lr.destroyLine();
            state = State.COOLDOWN;
        }
    }
}
