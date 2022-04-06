using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed;
    private GameObject player;
    private bool attacked = true;
    private float flightHeight;
    private float attackRange = 4f;
    // Start is called before the first frame update
    void Start()
    {
        flightHeight = Random.Range(1f, 4f);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null)
            return;
        if(attacked == false)
            flightHeight = Random.Range(1f, 4f);
        if (transform.position.y- flightHeight <= player.transform.position.y && attacked == true)
        {
            Debug.Log(flightHeight);
            rePosition();
            if (Mathf.Abs(player.transform.position.y - transform.position.y) >= flightHeight)
            {
                attacked = false;

            }
        }
        if(attacked == false)
        {
            float dist = Vector2.Distance(player.transform.position, transform.position);
            Debug.Log(flightHeight);
            if (dist > attackRange)
            {
                chase();
            }
        }



    }
    private void chase()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x,transform.position.y ), speed * Time.deltaTime);
    }
    private void rePosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, player.transform.position.y+ flightHeight), speed * Time.deltaTime);
    }
}
