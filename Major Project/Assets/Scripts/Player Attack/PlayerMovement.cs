using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody2D rb;

    private const float HORIZONTAL_SPEED_CLAMP = 5f;

    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravityScale = 10f;
    [SerializeField] private float frictionForce = 10f;
    [SerializeField] private int jumpCount = 2;
    [SerializeField] private LayerMask lm_ground;
    [SerializeField] private Transform groundCheckPos;

    public bool isGrounded { get; private set; }
    public bool isHooked = false;

    private void Awake() {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        GroundCheck();

        if (!isHooked) {
            rb.gravityScale = 0;
            Move();
        } else {
            rb.gravityScale = 9f;
            MoveOnHook();
        }

        if (isGrounded)
            if (Input.GetKeyDown(KeyCode.W))
                Jump();
    }

    private void GroundCheck() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(groundCheckPos.position.x, groundCheckPos.position.y), 0.1f, lm_ground);
        if (hits.Length > 0)
            isGrounded = true;
        else {
            isGrounded = false;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPos.position, 0.1f);
    }

    private void Move() {
        float horizontal = rb.velocity.x + Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vertical = rb.velocity.y - gravityScale * Time.deltaTime;
        if (((rb.velocity.x != 0) && (Input.GetAxis("Horizontal") == 0)) || ((rb.velocity.x > 0) && (Input.GetAxis("Horizontal") < 0)) || ((rb.velocity.x < 0) && (Input.GetAxis("Horizontal") > 0)))
            horizontal = Mathf.Lerp(rb.velocity.x, 0, frictionForce * Time.deltaTime);
        if (horizontal > HORIZONTAL_SPEED_CLAMP)
            horizontal = HORIZONTAL_SPEED_CLAMP;
        if (horizontal < -HORIZONTAL_SPEED_CLAMP)
            horizontal = -HORIZONTAL_SPEED_CLAMP;
        rb.velocity = new Vector2(horizontal, vertical);
    }

    private void MoveOnHook() {
        rb.AddForce(Vector2.right * moveSpeed * Input.GetAxis("Horizontal"), ForceMode2D.Force);
        if (rb.velocity.x > HORIZONTAL_SPEED_CLAMP)
            rb.velocity = new Vector2(HORIZONTAL_SPEED_CLAMP, rb.velocity.y);
        if (rb.velocity.x < -HORIZONTAL_SPEED_CLAMP)
            rb.velocity = new Vector2(-HORIZONTAL_SPEED_CLAMP, rb.velocity.y);
    }

    private void Jump() {
        Vector2 velocityVector = new Vector2(rb.velocity.x, jumpForce);
        rb.velocity = velocityVector;
    }
}
