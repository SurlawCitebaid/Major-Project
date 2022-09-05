using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AirEnemy_1 : MonoBehaviour
{
    //0:MOVING 1:CHASE 2:AIMING 3:ATTACKING 4:COOLDOWN 5:STUNNED
    [SerializeField] float flightSpeed = .5f;
    private EnemyAiController states;
    [SerializeField] private GameObject pivot;
    [SerializeField] private GameObject light2d;
    private GameObject player;
    public Material m_Material;
    private LineRenderer lr;
    SpriteRenderer theScale;
    
    
    private bool attacked = false, movePos = false, attackReady = false, moving = false, predictionLine = false, invalid = false;
    void Start()
    {
        DrawLine(new Vector3(1, 1, 1), new Vector3(2, 2, 2), this.transform);                   // initialize line
        alphaInvis();

        states = this.transform.GetComponent<EnemyAiController>();                                             // enemy state machine

        player = GameObject.FindGameObjectWithTag("Player");                                    // variable to track player
        theScale = GetComponent<SpriteRenderer>();
        light2d.SetActive(false);

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
                int index = 999;
                RaycastHit2D[] hits = Physics2D.RaycastAll(pivot.transform.position, pivot.transform.TransformDirection(Vector2.up), Mathf.Infinity);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.tag == "Wall")
                    {    
                        index = i;                              // get first wall collision
                        break;
                    }
                }

                
                moveLine(new Vector3(this.pivot.transform.position.x, this.pivot.transform.position.y, 1), new Vector3(hits[index].point.x, hits[index].point.y, 1));    // line position accounts of knockback
                if(!predictionLine)
                {
                    alphaSolid();
                    light2d.SetActive(true);
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
        alphaInvis();
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
                Debug.Log("ASDSA");
                angle = Random.Range(0, 2f * Mathf.PI);
                validPos = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (states.enemy.attack.range - 1f);
            }
            transform.position = validPos;
            movePos = true;

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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            light2d.SetActive(false);
            moving = true;
        }
    }
}
