using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stuff : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject marker;
    
    
    void Start()
    {
        marker = transform.Find("Square").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 wPos = Input.mousePosition;
        wPos.z = transform.position.z - Camera.main.transform.position.z;
        wPos = Camera.main.ScreenToWorldPoint(wPos);
        Vector3 direction = wPos - transform.position;
        float radius = 2;
        direction = Vector3.Normalize(direction) * radius;
        
        


        Vector3 dir = transform.position - marker.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        marker.transform.position = transform.position + direction;
        marker.transform.rotation = Quaternion.AngleAxis(angle - 90, -Vector3.forward);


    }
}
