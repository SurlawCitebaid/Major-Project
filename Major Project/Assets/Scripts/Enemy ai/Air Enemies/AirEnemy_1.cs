using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AirEnemy_1 : MonoBehaviour
{
    //0:MOVING 1:CHASE 2:AIMING 3:ATTACKING 4:COOLDOWN 5:STUNNED
    private EnemyAiController states;
    [SerializeField] private GameObject pivot;
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject light2d;
    GameObject predLine;
    private GameObject player, fireAnim;
    public Material m_Material;
    SpriteRenderer theScale;
    Rigidbody2D rb;


    private bool movePos = true, Aim = false;
    void Start()
    {
        predLine = Instantiate(line, pivot.transform.position, pivot.transform.rotation);
        predLine.transform.parent = pivot.transform;
        predLine.transform.localScale = new Vector3 (.3f,50,0);
        predLine.transform.position = new Vector2(predLine.transform.position.x-37.5f, predLine.transform.position.y);

        fireAnim = pivot.transform.Find("AttackAnim").gameObject;
        
        states = this.transform.GetComponent<EnemyAiController>();                                             // enemy state machine
        player = GameObject.FindGameObjectWithTag("Player");                                    // variable to track player
        theScale = GetComponent<SpriteRenderer>();

        light2d.SetActive(false);
        predLine.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        states.SetState(4);
    }

    void FixedUpdate()
    {
        
        if (states.CurrentState()!= EnemyAiController.State.ATTACKING)
        {
            rb.velocity = Vector3.zero;

            fireAnim.SetActive(false);
        }
        if(!Room.enemyLocationValid(this.transform.position))
        {
            StartCoroutine(Reset());
        }
        switch (states.CurrentState())
        {
            case EnemyAiController.State.MOVING:

                Movement();
            break;
            case EnemyAiController.State.AIMING:
            StartCoroutine(Aiming());
                
                break;
            case EnemyAiController.State.ATTACKING:
            StartCoroutine(Attack());
                break;
            case EnemyAiController.State.COOLDOWN:
                StartCoroutine(Reset());
                break;
        }
        

    }
    void Movement()
    {
        if (movePos)
        {
            float angle = Random.Range(0, 2f * Mathf.PI);
            Vector2 validPos = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (states.enemy.attack.range - 1f);
            if (!Room.enemyLocationValid(validPos))
            {
                while (!Room.enemyLocationValid(validPos))
                {
                    if (!Room.enemyLocationValid(player.transform.position))
                    {
                        states.Die();
                        break;
                    }
                    angle = Random.Range(0, 2f * Mathf.PI);
                    validPos = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (states.enemy.attack.range - 1f);
                }
            }
            transform.position = validPos;
            movePos = false;
        }
        StartCoroutine(MovementDelay());



    }
    IEnumerator MovementDelay()
    {
        yield return new WaitForSeconds(2);
        states.SetState(2);
    }
    IEnumerator Reset()
    {
        movePos = true;
        Aim = false;
        light2d.SetActive(false);
        yield return new WaitForSeconds(2);
        states.SetState(0);

    }
    public float AngleDir(Vector2 A, Vector2 B)
    {
        return -A.x * B.y + A.y * B.x;
    }

    IEnumerator Attack()
    {
        yield return null;
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, .5f);
        foreach (Collider2D coll in array)
        {
            PlayerController ass = coll.gameObject.GetComponent<PlayerController>();

            if (ass != null)                // checks player
            {
                ass.damage(1);
            }
        }
        rb.velocity = pivot.transform.up * 20f;
        fireAnim.SetActive(true);
        
    }
    IEnumerator Aiming()
    {
        
        if (!Aim)
        {
            if (AngleDir(player.transform.position, this.transform.position) < 0)
            {
                theScale.flipX = true;
            }
            else if (AngleDir(player.transform.position, this.transform.position) > 0)
            {
                theScale.flipX = false;
            }
            Vector3 vectorToTarget = player.transform.position - pivot.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            pivot.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            Aim = true;
        }
        predLine.SetActive(true);
        light2d.SetActive(true);
        yield return new WaitForSeconds(2);
        predLine.SetActive(false);

        states.SetState(3);
        
    }
}
