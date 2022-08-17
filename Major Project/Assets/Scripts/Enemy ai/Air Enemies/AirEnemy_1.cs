using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy_1 : MonoBehaviour
{
    //0:MOVING 1:CHASE 2:AIMING 3:ATTACKING 4:COOLDOWN 5:STUNNED
    [SerializeField] float flightSpeed = .5f;
    private EnemyAiController states;
    private GameObject player;
    private Quaternion originalRot;
    public Material m_Material;
    private LineRenderer lr;
    private bool attacked = false, predictionLine = true, movePos = false, attackReady = false;
    private int bass = 0;
    void Start()
    {
        DrawLine(new Vector3(1, 1, 1), new Vector3(2, 2, 2), this.transform);                   // initialize line
        alphaInvis();

        states = GetComponent<EnemyAiController>();                                             // enemy state machine

        player = GameObject.FindGameObjectWithTag("Player");                                    // variable to track player

        originalRot = transform.rotation;                                                       // save original Rotation

    }

    void FixedUpdate()
    {
        switch (states.currentState())
        {
            case EnemyAiController.State.MOVING:
                float dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist > states.enemy.attack.range && !attackReady)
                {
                    rePosition();
                }
                else
                {
                    states.setState(2);
                }
                break;
            case EnemyAiController.State.AIMING:
                StartCoroutine(Aiming());
                break;
            case EnemyAiController.State.ATTACKING:
                int index = 0;
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector2.up));
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.tag == "Wall")
                    {
                        alphaSolid();
                        index = i;                              // get first wall collision
                        break;
                    }
                }
                moveLine(new Vector3(this.transform.position.x, this.transform.position.y, 1), new Vector3(hits[index].point.x, hits[index].point.y, 1));    // line position accounts of knockback
                Invoke("attack", 1f);
                break;
            case EnemyAiController.State.COOLDOWN:
                reset();
                break;
        }



            

    }
    void reset()
    {
        states.setState(0);
        movePos = false;
        attacked = true;
        attackReady = false;
        predictionLine = true;
    }
    private void attack()
    {
        if (!attacked)
        {
            if (flightSpeed > 2f)
            {
                flightSpeed = 2;
            }
            else if (flightSpeed < 0.1f)
            {
                flightSpeed = 0.1f;
            }
            transform.Translate(Vector2.up * flightSpeed);
        }
        Invoke("setStateCooldown", 2f);
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
    private void setStateCooldown()
    {
        states.setState(4);
    }
    public void DrawLine(Vector3 start, Vector3 end, Transform parent)
    {

        GameObject line = new GameObject();
        line.transform.parent = parent;
        line.transform.position = start;
        line.AddComponent<LineRenderer>();
        lr = line.GetComponent<LineRenderer>();
        lr.material = m_Material;
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
    public void alphaInvis()
    {
        lr.startColor = Color.clear;
        lr.endColor = Color.clear;
    }
    public void alphaSolid()
    {
        lr.startColor = Color.red;
        lr.endColor = Color.red;
    }
    public void moveLine(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
    private void rePosition()
    {
        if (!movePos)
        {
            movePos = true;
            float angle = Random.Range(0, 2f * Mathf.PI);
            transform.position = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (states.enemy.attack.range - 1f);
        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            alphaInvis();
            attacked = true;
            predictionLine = false;
        }
    }
}
