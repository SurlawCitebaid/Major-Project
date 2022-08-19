using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class GrapplingHook : MonoBehaviour {
    private const float MIN_HOOK_DISTANCE = 0.6f;

    [SerializeField] private Camera cam;

    private DistanceJoint2D dj;

    private enum GrappleMode { PULL, SWING };
    private GrappleMode grapple_mode;
    [SerializeField] private Transform grapple_mode_gui;
    [SerializeField] private Sprite grapple_mode_pull_sprite;
    [SerializeField] private Sprite grapple_mode_swing_sprite;

    [SerializeField] private LayerMask lm_walls;
    [SerializeField] private Transform hook;
    [SerializeField] private float hookClimbSpeed = 1 / 3;

    private Vector3 hookPos;
    private Transform hookObj;

    private void Start() {
        grapple_mode = GrappleMode.PULL;
        UpdateGrappleGUI();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (grapple_mode == GrappleMode.PULL)
                grapple_mode = GrappleMode.SWING;
            else
                grapple_mode = GrappleMode.PULL;
            UpdateGrappleGUI();
        }

        if (Input.GetMouseButtonDown(1)) {
            Grapple();
        }

        if (Input.GetMouseButtonUp(1)) {
            Unhook();
        }

        if (dj ?? true) {
            if (!this.gameObject.GetComponent<movement_Mario>().getGrounded()) {
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

    private void UpdateGrappleGUI() {
        if (grapple_mode_gui == null)
            return;

        if (grapple_mode == GrappleMode.PULL)
            grapple_mode_gui.GetComponent<Image>().sprite = grapple_mode_pull_sprite;

        if (grapple_mode == GrappleMode.SWING)
            grapple_mode_gui.GetComponent<Image>().sprite = grapple_mode_swing_sprite;
    }

    private void Grapple() {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - transform.position, 6f, lm_walls);
        if (hit.collider != null)
        {
            hookPos = hit.point;

            dj = this.gameObject.AddComponent<DistanceJoint2D>();
            dj.connectedAnchor = new Vector3(hookPos.x, hookPos.y);
            dj.autoConfigureDistance = false;
            if (grapple_mode == GrappleMode.PULL)
            {
                dj.distance = MIN_HOOK_DISTANCE;
            }
            else if (grapple_mode == GrappleMode.SWING)
            {
                dj.autoConfigureDistance = true;
                dj.distance = dj.distance / 1.1f;
            }
            dj.enableCollision = true;
            Vector3 vectorToTarget = hookPos - this.gameObject.transform.position;
            float hookAngle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
            hookObj = Instantiate(hook, hookPos, Quaternion.identity, null);
            hookObj.eulerAngles = new Vector3(0, 0, hookAngle);

            this.gameObject.GetComponent<PlayerMovement>().isHooked = true;
        }
    }

    private void Unhook() {
        if (dj ?? false) {
            Destroy(dj);
            Destroy(hookObj.gameObject);
            this.gameObject.GetComponent<PlayerMovement>().isHooked = false;
        }
    }
}
