using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy_2 : MonoBehaviour
{
    [SerializeField] private Transform projectile;
    [SerializeField] float flightSpeed = .5f;
    [SerializeField] private GameObject line;
    private EnemyAiController states;
    private GameObject player;
    public Material m_Material;
    GameObject predLine;
    private bool attacked = false, movePos = false;
    private int resetCount = 0;
    void Start()
    {
        predLine = Instantiate(line, this.transform.position, this.transform.rotation);
        predLine.transform.parent = this.transform;
        predLine.transform.localScale = new Vector3(.3f, 10, 0);
        predLine.transform.position = new Vector2(transform.position.x, transform.position.y+12f);
        states = GetComponent<EnemyAiController>();                                             // enemy state machine

        player = GameObject.FindGameObjectWithTag("Player");                                    // variable to track player
        predLine.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (states.CurrentState())
        {
            case EnemyAiController.State.MOVING:
                 RePosition();
                break;
            case EnemyAiController.State.AIMING:
                StartCoroutine(Aiming());
                break;
            case EnemyAiController.State.ATTACKING:
                StartCoroutine(FireProjectile());
                break;
        }
    }
    IEnumerator FireProjectile()
    {
        if (!attacked)
        {
            attacked = true;
            predLine.SetActive(true);

            yield return new WaitForSeconds(2f);
            predLine.SetActive(false);
            Transform bullet = Instantiate(projectile, transform.position, Quaternion.identity).transform;
            Vector3 shootDir = transform.up;
            bullet.GetComponent<Projectile>().Setup(shootDir);

            StartCoroutine(reset()); // determines attack delay
        }
    }
    IEnumerator reset()
    {
        yield return new WaitForSeconds(4f);
        if (resetCount != 5)
        {
            resetCount++;
            movePos = false;
            predLine.SetActive(false);
            states.SetState(0);
        }
        
    }
    IEnumerator Aiming()
    {
        predLine.SetActive(false);
        Vector3 dirFromAtoB = (player.transform.position - transform.position).normalized;
        Vector3 vectorToTarget = player.transform.position - transform.position;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.up);
        float angle;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 3f);
        if (dotProd >= .90)
        {
            attacked = false;
            yield return null;
            Invoke("SetStateAttack", 1f);

        }
    }
    private void SetStateAttack()
    {
        states.SetState(3);
    }
    private void RePosition()
    {
        if (!movePos)
        {
            float angle = Random.Range(0, 2f * Mathf.PI);
            Vector2 validPos = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (states.enemy.attack.range - 1f);
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
            transform.position = validPos;
            states.SetState(2);
            movePos = true;

        }

    }
            
}
