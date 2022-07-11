using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCharger : MonoBehaviour
{
    //0:MOVING 1:CHASE 2:AIMING 3:ATTACKING 4:COOLDOWN 5:STUNNED
    [SerializeField] float flightSpeed = .5f;
    private LineRendererController lr;
    private EnemyAIController states;
    private Quaternion originalRot;
    private GameObject player;
    private Rigidbody2D rigid;
    private bool attacked = false, predictionLine = true, movePos = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lr = GetComponent<LineRendererController>();
        states = GetComponent<EnemyAIController>();
        rigid = GetComponent<Rigidbody2D>();
        originalRot = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey("space"))
        {
            Debug.Log(states.health);
           states.Damage(1,0,0);
        }
        switch (states.currentState())
        {
            case EnemyAIController.State.MOVING:
                float dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist > states.enemy.attack.range)
                {
                    Invoke("rePosition", 1f);
                }
                else
                {

                    Invoke("setStateAiming",1f);
                }
                break;
            case EnemyAIController.State.AIMING:
                StartCoroutine(Aiming());
                break;
            case EnemyAIController.State.ATTACKING:
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up));

                predLine(hitInfo);


                Invoke("attack",1f);
                break;
            case EnemyAIController.State.COOLDOWN:
                reset();
                break;
            case EnemyAIController.State.STUNNED:
                break;
        }
    }
    private void predLine(RaycastHit2D hitInfo)
    {

        if (predictionLine)
        {
            lr.DrawLine(new Vector3(transform.position.x, transform.position.y, 1), new Vector3(hitInfo.point.x, hitInfo.point.y, 1));
            lr.enabled = false;
            predictionLine = false;
        }
        else
        {
            lr.moveLine(new Vector3(transform.position.x, transform.position.y, 1), new Vector3(hitInfo.point.x, hitInfo.point.y, 1));
        }
        if (lr.enabled)
        {

            lr.updateStartPoint(new Vector3(transform.position.x, transform.position.y, 1));
        }
        if (!attacked)
        {
            lr.changeAlpha(false);
        }
    }
    IEnumerator Aiming()
    {
        Vector3 dirFromAtoB = (player.transform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.up);
        float angle;
        Vector3 vectorToTarget = player.transform.position - transform.position;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 5f);
        if (dotProd == 1)
        {
            yield return null;
            attacked = false;
            states.setState(3);
            Debug.Log("ASS");
        }


    }
    private void attack()
    {

        if (!attacked)
        {
            if (flightSpeed > 1f)
            {
                flightSpeed = 1;
                transform.Translate(Vector2.up * flightSpeed);
            } else if (flightSpeed < 0.1f)
            {
                flightSpeed = 0.1f;
                transform.Translate(Vector2.up * flightSpeed);
            }
            else
            {
                transform.Translate(Vector2.up * flightSpeed);
            }
        }
        Invoke("setStateCooldown", 2f);
    }
    private void setStateAiming()
    {
        states.setState(2);
    }
    private void setStateCooldown()
    {
        states.setState(4);
    }
    private void rePosition()
    {
        if(!movePos)
        {
            movePos = true;
            transform.rotation = originalRot;
            float angle = Random.Range(0, 2f * Mathf.PI);
            transform.position = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (states.enemy.attack.range-1f);
        }

    }

    void reset()
    {
        states.setState(0);
        movePos = false;
        attacked = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            attacked = true;
            rigid.velocity = Vector2.zero;
            rigid.angularVelocity = 0f;
            lr.enabled = false;
            lr.changeSortingOrder(1);
            lr.changeAlpha(true);
        }
    }
}
