using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float thrust = 20f;
    public float speed;
    private GameObject player;
    private bool attacked = true;
    private float flightHeight;
    private float attackRange = 4f;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        flightHeight = Random.Range(1f, 4f);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null)
            return;

        if (transform.position.y - flightHeight <= player.transform.position.y && attacked == true)
        {
            rePosition();                                           //enemy below player
        } else if (transform.position.y - flightHeight >= player.transform.position.y && attacked == true)
        {
            attacked = false;                                       //enemy above player
        }else
        {
            flightHeight = Random.Range(1f, 4f);
            float dist = Vector2.Distance(player.transform.position, transform.position);
            if (dist > attackRange)
            {
                chase();
            }
            else
                attack();
        }
    }
    private void attack()
    {
        rigid.AddForce(transform.right * thrust);
    }
    private void chase()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x,transform.position.y ), speed * Time.deltaTime);
    }
    private void rePosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, player.transform.position.y+ flightHeight), speed * Time.deltaTime);
        if (Mathf.Abs(transform.position.y) == Mathf.Abs(flightHeight +player.transform.position.y) ) //you cant compare float values by themselves
        {
            attacked = false;
        }
    }
}
