using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMe : MonoBehaviour
{
    Camera cam;
    public float moveAmount;
    // Start is called before the first frame update
    void Start()
    {
        cam = transform.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = cam.transform.position.x + moveAmount;
        cam.transform.position = new Vector2(move, cam.transform.position.y);
    }
}
