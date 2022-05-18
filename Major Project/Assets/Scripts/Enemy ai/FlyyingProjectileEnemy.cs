using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyyingProjectileEnemy : MonoBehaviour
{
    private float speed = 5f, attackRange = 14f, flightHeight, angle;
    private bool attacked = false, predictionLine = true;
    private LineRendererController lr;
    private EnemyAiController states;
    private Rigidbody2D rigid;
    private GameObject player;
    [SerializeField] private Transform projectile;
    private Vector3 endPoint;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lr = GetComponent<LineRendererController>();
        states = GetComponent<EnemyAiController>();
        flightHeight = Random.Range(1f, 9f);
        states.setState(1);
    }

    // Update is called once per frame
    void Update()
    {
        switch (states.currentState())
        {
            case EnemyAiController.State.CHASE:
                Debug.Log("ASS");
                float dist = Mathf.Abs(transform.position.x - player.transform.position.x);
                if (dist > attackRange)
                {
                    chase();
                    Debug.Log("CHASE");
                }
                else
                {
                    Debug.Log("CHASE2");
                    states.setState(2);
                }
                break;
            case EnemyAiController.State.AIMING:
                StartCoroutine(aimAttack());
                Debug.Log("AIM");
                break;
            case EnemyAiController.State.ATTACKING:
                if (!attacked)
                {
                    Debug.Log("ATTACK");
                    attack();
                }
                break;
            case EnemyAiController.State.COOLDOWN:
                Debug.Log("COOLDOWN");
                Invoke("setStateMoving", 1f);
                break;

        }
    }
    public void setStateMoving()
    {
        Debug.Log("ASS");
        states.setState(1);
    }
    private void attack()
    {
        attacked = true;
        Transform bullet = Instantiate(projectile, transform.position, Quaternion.identity).transform;
        Vector3 shootDir = (endPoint - transform.position).normalized;
        bullet.GetComponent<Projectile>().Setup(shootDir);
        states.setState(4);
    }
    private void chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speed * Time.deltaTime);
    }
    IEnumerator aimAttack()
    {
        Vector3 dirFromAtoB = (player.transform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.up);

        Vector3 vectorToTarget = player.transform.position - transform.position;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        if (dotProd == 1)
        {

            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up));
            if (hitInfo.transform.tag == "Wall")
            {
                endPoint = hitInfo.point;
            }

            if (predictionLine)
            {
                lr.DrawLine(new Vector3(transform.position.x, transform.position.y, 1), new Vector3(hitInfo.point.x, hitInfo.point.y, 1));
                predictionLine = false;                 //Line has higher z so its behind everything
                lr.destroyLineAfterPeriod(2f);
            }
            yield return new WaitForSeconds(1);
            states.setState(3);

        }
        yield return null;
    }
}
