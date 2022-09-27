using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AirEnemy_1 : MonoBehaviour
{
    //0:MOVING 1:CHASE 2:AIMING 3:ATTACKING 4:COOLDOWN 5:STUNNED
    [SerializeField] float flightSpeed = .5f;
    private EnemyAiController states;
    [SerializeField] private GameObject pivot;
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject light2d;
    GameObject predLine;
    private GameObject player;
    public Material m_Material;
    private LineRenderer lr;
    SpriteRenderer theScale;
    
    
    private bool attacked = false, movePos = false, attackReady = false, moving = false, predictionLine = false, invalid = false;
    void Start()
    {
        predLine = Instantiate(line, pivot.transform.position, pivot.transform.rotation);
        predLine.transform.parent = pivot.transform;
        predLine.transform.localScale = new Vector3 (.3f,50,0);
        predLine.transform.position = new Vector2(predLine.transform.position.x, predLine.transform.position.y+37.5f);
        states = this.transform.GetComponent<EnemyAiController>();                                             // enemy state machine
        player = GameObject.FindGameObjectWithTag("Player");                                    // variable to track player
        theScale = GetComponent<SpriteRenderer>();
        light2d.SetActive(false);
        predLine.SetActive(false);
    }

    void FixedUpdate()
    {
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, .5f);
        foreach (Collider2D coll in array)
        {
            PlayerController ass = coll.gameObject.GetComponent<PlayerController>();

            if (ass != null && attacked)
            {
                ass.damage(1);
            }
        }
        switch (states.currentState())
        {
            case EnemyAiController.State.MOVING:
                float dist = Vector3.Distance(pivot.transform.position, player.transform.position);
                if (dist > states.enemy.attack.range && !attackReady || invalid)
                {
                    rePosition();
                    invalid = false;
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

                
                if(!predictionLine)
                {
                    light2d.SetActive(true);
                    predLine.SetActive(true);
                    predictionLine = true;
                }
                
                Invoke("attack", 1f);
                break;
            case EnemyAiController.State.COOLDOWN:
                reset();
                break;
        }
    }
    void reset()
    {
        if(!Room.enemyLocationValid(transform.position))
        {
            invalid = true;
        }
        predictionLine = false;
        states.setState(0);
        attackReady = false;
        attacked = false;
        movePos = false;
        moving = true;
    }
    public float AngleDir(Vector2 A, Vector2 B)
    {
        return -A.x * B.y + A.y * B.x;
    }

    private void attack()
    {
        if (!moving)
        {
            if (flightSpeed > 2f)
            {
                flightSpeed = 2;
            }
            else if (flightSpeed < 0.1f)
            {
                flightSpeed = 0.1f;
            }
            predLine.SetActive(false);
            transform.Translate(pivot.transform.up * flightSpeed);
            attacked = true;
        }
        
        Invoke("setStateCooldown", 2f);
    }
    IEnumerator Aiming()
    {
        if (AngleDir(player.transform.position, this.transform.position) < 0)
        {
            theScale.flipX = true;
        }
        else if (AngleDir(player.transform.position, this.transform.position) > 0)
        {
            theScale.flipX = false;
        }
        Vector3 dirFromAtoB = (player.transform.position - pivot.transform.position).normalized;
        Vector3 vectorToTarget = player.transform.position - pivot.transform.position;
        float dotProd = Vector3.Dot(dirFromAtoB, pivot.transform.up);
        float angle;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        pivot.transform.rotation = Quaternion.Slerp(pivot.transform.rotation, q, Time.deltaTime * 5f);
        if (dotProd >= .99)
        {
            yield return null;
            moving = false;
            states.setState(3);
        }
    }
    private void rePosition()
    {
        if (!movePos)
        {
            float angle = Random.Range(0, 2f * Mathf.PI);
            Vector2 validPos = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (states.enemy.attack.range - 1f);
            while (!Room.enemyLocationValid(validPos))
            {
                if(!Room.enemyLocationValid(player.transform.position))
                {
                    states.Die();
                    break;
                }
                angle = Random.Range(0, 2f * Mathf.PI);
                validPos = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (states.enemy.attack.range - 1f);
            }
            transform.position = validPos;
            movePos = true;

        }

    }
    private void setStateCooldown()
    {
        if(states.currentState() == EnemyAiController.State.COOLDOWN)
        {
            return;
        }     else
        {
            states.setState(4);
        }

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Debug.Log(col.gameObject.name);
            light2d.SetActive(false);
            moving = true;
        }
    }
}
