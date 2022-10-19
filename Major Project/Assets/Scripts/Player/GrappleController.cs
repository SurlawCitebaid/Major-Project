using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleController : MonoBehaviour
{
    LineRenderer line;

    [Header("General")][Space]
    [SerializeField] LayerMask grapplableLayer;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float grappleSpeed = 10f;
    [SerializeField] float grappleShootSpeed = 20f;
    public GameObject grapplingHook;


    bool isGrappling = false;
    bool canReach;
    float grav;
    Rigidbody2D rb;
    GameObject hook;
    [HideInInspector] public bool retracting = false;

    Vector2 target;

    private void Start() {

        line = GetComponent<LineRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        grav = rb.gravityScale;
        canReach = true;
    }

    private void Update() {

        if (Input.GetButtonDown("Grapple") && !isGrappling) {
            StartGrapple();
        }

        if (retracting) {
            Vector2 grapplePos = Vector2.Lerp(transform.position, target, grappleSpeed * Time.deltaTime);
            float dist = Vector2.Distance(target, transform.position);
            if (dist < 0.6)
            {
                canReach = false;
            }
            if(canReach)
            {
                transform.position = grapplePos;
            }
            rb.gravityScale = 0.0f;
            line.SetPosition(0, transform.position);

            if (Input.GetMouseButton(0) || (Input.GetButtonUp("Grapple") || Input.GetButtonDown("Rush_B") || Input.GetButtonDown("Jump")) || (canReach == false && dist > 3)) 
            {
                if(hook != null)
                {
                    Destroy(hook);
                }
                retracting = false;
                isGrappling = false;
                line.enabled = false;
                canReach = true;
                rb.gravityScale = grav;
            }
        }
    }

    private void StartGrapple() {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, grapplableLayer);

        if (hit.collider != null) {
            Vector3 pos = Camera.main.WorldToScreenPoint(this.transform.position);
            pos = Input.mousePosition - pos;
            float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
            hook = Instantiate(grapplingHook, this.transform.position, Quaternion.AngleAxis(angle - 90, Vector3.forward));
            isGrappling = true;
            target = hit.point;
            line.enabled = true;
            line.positionCount = 2;
            StartCoroutine(Grapple(hook));
        }
    }

    IEnumerator Grapple(GameObject hook) {
        float t = 0;
        float time = 10;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position); 

        Vector2 newPos;

        for (; t < time; t += grappleShootSpeed * Time.deltaTime) {
            newPos = Vector2.Lerp(transform.position, target, t / time);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);
            hook.transform.position = newPos;
            yield return null;
        }
        
        line.SetPosition(1, target);
        retracting = true;
    }
}
