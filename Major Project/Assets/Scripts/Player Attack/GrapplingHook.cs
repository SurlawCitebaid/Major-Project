using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GrapplingHook : MonoBehaviour {
    private const float MIN_HOOK_DISTANCE = 0.6f;

    private Camera cam;
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
                if (dj.distance - hookClimbSpeed > MIN_HOOK_DISTANCE)
                    dj.distance -= hookClimbSpeed;
                else
                    dj.distance = MIN_HOOK_DISTANCE;
            }
        }
    }

    private void Grapple() {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - transform.position, 100f, lm_walls);
        hookPos = hit.point;

        dj = this.gameObject.AddComponent<DistanceJoint2D>();
        dj.connectedAnchor = new Vector3(hookPos.x, hookPos.y);
        dj.autoConfigureDistance = false;
        dj.distance = MIN_HOOK_DISTANCE;
        dj.enableCollision = true;
        Vector3 vectorToTarget = hookPos - this.gameObject.transform.position;
        float hookAngle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        hookObj = Instantiate(hook, hookPos, Quaternion.identity, null);
        hookObj.eulerAngles = new Vector3(0, 0, hookAngle);

        this.gameObject.GetComponent<PlayerMovement>().isHooked = true;
    }

    private void Unhook() {
        if (dj ?? false) {
            Destroy(dj);
            Destroy(hookObj.gameObject);
            this.gameObject.GetComponent<PlayerMovement>().isHooked = false;
        }
    }
}
