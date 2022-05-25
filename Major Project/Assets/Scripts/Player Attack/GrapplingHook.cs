using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GrapplingHook : MonoBehaviour {
    private Camera cam;
    //private SpringJoint2D sj;
    private DistanceJoint2D dj;

    [SerializeField] private LayerMask lm_walls;
    [SerializeField] private Transform hook;
    [SerializeField] private float hookClimbSpeed = 1 / 3;

    private Vector3 hookPos;
    private Transform hookObj;

    private void Awake() {
        cam = Camera.main;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            Grapple();
        }

        if (Input.GetMouseButtonUp(1)) {
            Unhook();
        }

        if (dj ?? true) {
            if (!this.gameObject.GetComponent<PlayerMovement>().isGrounded) {
                if (Input.mouseScrollDelta.y < 0) {
                    dj.distance += hookClimbSpeed;
                }
            }
            if (Input.mouseScrollDelta.y > 0) {
                dj.distance -= hookClimbSpeed;
            }
        }

        /*
        if (Input.mouseScrollDelta.y > 0) {
            if (sj ?? true)
                sj.distance -= hookClimbSpeed;
        } else if (Input.mouseScrollDelta.y < 0) {
            if (sj ?? true)
                sj.distance += hookClimbSpeed;
        }*/
    }

    private void Grapple() {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - transform.position, 100f, lm_walls);
        hookPos = hit.point;

        /*
        sj = this.gameObject.AddComponent<SpringJoint2D>();
        sj.connectedAnchor = hookPos;
        sj.autoConfigureDistance = false;
        sj.distance = 3.5f;
        sj.dampingRatio = 0.5f;
        sj.enableCollision = true;
        if (hookPos.y < this.gameObject.transform.position.y)
            sj.distance = 0f;
        */

        dj = this.gameObject.AddComponent<DistanceJoint2D>();
        dj.connectedAnchor = hookPos;
        dj.autoConfigureDistance = false;
        //dj.distance = 3.5f;
        dj.distance = 0.5f;
        dj.enableCollision = true;
        //if (hookPos.y < this.gameObject.transform.position.y)
            //dj.distance = 1f;

        Vector3 vectorToTarget = hookPos - this.gameObject.transform.position;
        float hookAngle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        hookObj = Instantiate(hook, hookPos, Quaternion.identity, null);
        hookObj.eulerAngles = new Vector3(0, 0, hookAngle);

        this.gameObject.GetComponent<PlayerMovement>().isHooked = true;
    }

    private void Unhook() {
        /*
         if (sj ?? false) {
            Destroy(sj);
            Destroy(hookObj.gameObject);
            this.gameObject.GetComponent<PlayerMovement>().isHooked = false;
        }
         */
        if (dj ?? false) {
            Destroy(dj);
            Destroy(hookObj.gameObject);
            this.gameObject.GetComponent<PlayerMovement>().isHooked = false;
        }
    }
}
