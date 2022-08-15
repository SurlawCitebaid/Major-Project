using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy_1 : MonoBehaviour
{
    [SerializeField] LayerMask ass;
    private EnemyAiController states;
    private GameObject player;
    private LineRendererController lr;
    private bool attacked = false, predictionLine = true, movePos = false;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRendererController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int layerMask = ~(LayerMask.GetMask("Player"));
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), layerMask);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector2.up));
        for (int i = 0; i < hits.Length; ++i)
        {
            Debug.LogFormat("The name of collider {0} is \"{1}\".",
                i, hits[i].collider.gameObject.name);
            predLine(hits[1]);
        }


    }
    private void predLine(RaycastHit2D hitInfo)
    {
        if (predictionLine)
        {
            lr.DrawLine(new Vector3(transform.position.x, transform.position.y, 1), hitInfo.point, this.transform);             //creates initial line
            predictionLine = false;
        }
        else
        {
            lr.moveLine(new Vector3(transform.position.x, transform.position.y, 1), new Vector3(hitInfo.point.x, hitInfo.point.y, 1));      //moves line
        }

        if (!attacked)
        {
            lr.changeAlpha(false);                      //makes line visible 
        }
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
}
