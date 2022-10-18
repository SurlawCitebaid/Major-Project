using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy_2 : MonoBehaviour
{
    [SerializeField] private Transform projectile;
    [SerializeField] private GameObject line;
    private EnemyAiController states;
    private GameObject player;
    public Material m_Material;
    Quaternion q;
    GameObject predLine;
    private bool attacked = false, movePos = false;
    private int resetCount = 0;
    void Start()
    {
        predLine = Instantiate(line, this.transform.position, this.transform.rotation);
        predLine.transform.parent = this.transform;
        predLine.transform.localScale = new Vector3(.3f, 30, 0);
        predLine.transform.position = new Vector3(transform.position.x, transform.position.y+23, 0);
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
            FindObjectOfType<AudioManager>().Play("Bang");

            StartCoroutine(Reset()); // determines attack delay
        }
    }
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(4f);
        if (resetCount != 3)
        {
            resetCount++;
            movePos = false;
            predLine.SetActive(false);
            states.SetState(0);
        } else
        {
            states.Die();
        }
        
    }
    IEnumerator Aiming()
    {
        predLine.SetActive(false);
        Vector3 vectorToTarget = player.transform.position - transform.position;
        float angle;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
        attacked = false;
        yield return new WaitForSeconds(1f);
        SetStateAttack();

        
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
            Vector2 validPos = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (Random.Range(states.enemy.attack.range, states.enemy.attack.range * 2));
            while (!Room.enemyLocationValid(validPos))
            {
                if (!Room.enemyLocationValid(player.transform.position))
                {
                    states.Die();
                    break;
                }
                angle = Random.Range(0, 2f * Mathf.PI);
                validPos = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (Random.Range(states.enemy.attack.range, states.enemy.attack.range * 2));
            }
            transform.position = validPos;
            states.SetState(2);
            movePos = true;

        }

    }
            
}
