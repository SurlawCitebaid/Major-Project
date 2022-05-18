using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLaserEnemyAI : MonoBehaviour
{
    private LineRendererController lr;
    private bool spawnLine = false;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRendererController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 30f * Time.deltaTime);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down));               
        if(!spawnLine)
        {
            lr.DrawLine(transform.position, hitInfo.point);             //spawns the line
            spawnLine = true;
        }

        if(hitInfo.collider != null)
        {
            lr.setPos(1, new Vector3(hitInfo.point.x, hitInfo.point.y,1));      //updates the line
        } 
        lr.setPos(0, transform.position);
    }
}
