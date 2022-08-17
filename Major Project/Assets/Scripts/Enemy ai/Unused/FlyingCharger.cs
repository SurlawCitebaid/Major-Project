using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCharger : MonoBehaviour
{
    //0:MOVING 1:CHASE 2:AIMING 3:ATTACKING 4:COOLDOWN 5:STUNNED
    [SerializeField] float flightSpeed = .5f;
    private LineRendererController lr;
    private EnemyAiController states;
    private Quaternion originalRot;
    private GameObject player;
    //private Rigidbody2D rigid;
    private bool attacked = false, predictionLine = true, movePos = false, attackReady = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lr = GetComponent<LineRendererController>();
        states = GetComponent<EnemyAiController>();
        //rigid = GetComponent<Rigidbody2D>();
        originalRot = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (states.currentState())
        {
            case EnemyAiController.State.MOVING:
                float dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist > states.enemy.attack.range && !attackReady)
                {
                    rePosition();
                } else {
                    states.setState(2);
                }
                break;

            case EnemyAiController.State.AIMING:
                StartCoroutine(Aiming());
                break;

            case EnemyAiController.State.ATTACKING:
                int layer_mask = LayerMask.GetMask("Walls");
                RaycastHit2D[] hitInfo = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector2.up), layer_mask);
                for (int i = 0; i < hitInfo.Length; ++i)
                {
                    if (hitInfo[i].collider.gameObject.tag == "Walls")
                        Debug.LogFormat("The name of collider {0} is \"{1}\".",
                            i, hitInfo[i].collider.gameObject.name);
                                    predLine(hitInfo[i]);
                }

                Invoke("attack",1f);
                break;

            case EnemyAiController.State.COOLDOWN:
                reset();
                break;
        }
    }
    private void predLine(RaycastHit2D hitInfo)
    {
        if (predictionLine)
        {
            lr.DrawLine(new Vector3(transform.position.x, transform.position.y, 1), hitInfo.point, this.transform);
            lr.enabled = false;
            predictionLine = false;
        } else {
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
        Vector3 vectorToTarget = player.transform.position - transform.position;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.up);
        float angle;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 5f);
        if (dotProd >= .99)
        {
            yield return null;
            attacked = false;
            states.setState(3);
        }
    }
    private void attack()
    {
        if (!attacked)
        {
            if (flightSpeed > 2f)
            {
                flightSpeed = 2;  
            } else if (flightSpeed < 0.1f){
                flightSpeed = 0.1f;
            }
            transform.Translate(Vector2.up * flightSpeed);
        }
        Invoke("setStateCooldown", 2f);
    }
    private void setStateCooldown()
    {
        states.setState(4);
    }
    private void rePosition()
    {
        if(!movePos)                        // runs method once
        {
            movePos = true;
            transform.rotation = originalRot;
            float angle = Random.Range(0, 2f * Mathf.PI);
            transform.position = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (states.enemy.attack.range-1f);
            attackReady = true;
        }

    }

    void reset()
    {
        states.setState(0);
        if (!lr.enabled)
        {
             lr.changeAlpha(true);
        }
        movePos = false;
        attacked = true;
        attackReady = false;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            attacked = true;
            //rigid.velocity = Vector2.zero;
            //rigid.angularVelocity = 0f;
            lr.enabled = false;
        }
    }
}
