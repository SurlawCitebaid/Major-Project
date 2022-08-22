using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy_2 : MonoBehaviour
{
    [SerializeField] private Transform projectile;
    [SerializeField] float flightSpeed = .5f;
    private EnemyAiController states;
    private GameObject player;
    public Material m_Material;
    private LineRenderer lr;
    private bool attacked = false, movePos = false, attackReady = false, moving = false, predictionLine = false;
    void Start()
    {
        DrawLine(new Vector3(1, 1, 1), new Vector3(2, 2, 2), this.transform);                   // initialize line
        alphaInvis();

        states = GetComponent<EnemyAiController>();                                             // enemy state machine

        player = GameObject.FindGameObjectWithTag("Player");                                    // variable to track player

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, .5f);
        //foreach (Collider2D coll in array)
        //{
        //    PlayerController ass = coll.gameObject.GetComponent<PlayerController>();

        //    if (ass != null && attacked)
        //    {
        //        ass.damage(1);

        //    }
        //}
        switch (states.currentState())
        {
            case EnemyAiController.State.MOVING:
                float dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist > states.enemy.attack.range && !attackReady)
                {
                    alphaInvis();
                    predictionLine = false;
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
                int index = 999;
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector2.up), Mathf.Infinity);
               
                    
                Debug.Log(hits.Length);
                   
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.tag == "Wall")
                    {
                        index = i;                              // get first wall collision

                    }
                }
                    

                
                    predictionLine = true;
                    moveLine(new Vector3(this.transform.position.x, this.transform.position.y, 1), new Vector3(hits[index].point.x, hits[index].point.y, 1));    // line position accounts of knockback
                    alphaSolid();
                
                
                
                if (!attacked)
                {
                    if (index != 999)
                    {
                        Transform bullet = Instantiate(projectile, transform.position, Quaternion.identity).transform;
                        Debug.Log(index);
                        Vector3 shootDir = (hits[index].point - (Vector2)transform.position).normalized;
                        bullet.GetComponent<Projectile>().Setup(shootDir);
                        attacked = true;
                        Invoke("reset", 1f);
                    }
                   

                }
                break;
            case EnemyAiController.State.COOLDOWN:
                
                break;
        }
    }
    void reset()
    {
        states.setState(0);
        attackReady = false;
        attacked = false;
        movePos = false;
        moving = true;
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
                angle = Random.Range(0, 2f * Mathf.PI);
                validPos = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (states.enemy.attack.range - 1f);
            }
            transform.position = validPos;
            movePos = true;

        }

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
            
}