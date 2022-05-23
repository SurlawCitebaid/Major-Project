using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class movement : MonoBehaviour
{

	//variables
	[Header("Movement")][Space]
	[Range(0, 400)] [SerializeField] private float m_MoveSpeed = 200f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSlippery = 0.05f;
	private bool m_DisableMovement = false;
	[Space][Space]

	[Header("Jump")][Space]
	[SerializeField] private LayerMask m_GroundLayer; // all things character can land on - can be set to multiple
	[SerializeField] private float m_JumpForce = 400f;
	[SerializeField] private int m_JumpStack = 2;
	private int jumpCount = 0;
	[Space][Space]

	[Header("Crouch")][Space]
	[SerializeField] private Transform m_BottomDetection;
	[SerializeField] private Transform m_TopDetection;
	[Range(0, 1)] [SerializeField] private float m_CrouchMultiplier = 0.4f;	
	[SerializeField] private Collider2D m_CrouchDisableCollider;
	[Space][Space]

	[Header("Dash")][Space]
	[Range(0, 100)][SerializeField] private float m_DashDistance = 5f;
	[SerializeField] private float m_DashDuration = 0.2f;
	[Range(0, 5)][SerializeField] private float m_DashCooldown = 3f; // in seconds
	[SerializeField] private bool m_DashGravitySwitch = false;
	private bool dashable = true;
	private bool grounded;
	private Rigidbody2D m_Rigidbody2D;
	private bool m_isFacingRight = true;
	private Vector3 m_Velocity = Vector3.zero;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		grounded = false;

		// set character state to grounded when intact with any part of floor's colliders
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_BottomDetection.position, 0.2f, m_GroundLayer);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				grounded = true;
				jumpCount = 0;
			}
		}
	}
	public bool GetGrounded()
	{
		return grounded;
	}
	public void Move(float move_direction, bool crouch, bool jump, float jump_charge, bool dash)
	{
		float speed = move_direction * m_MoveSpeed;

		// forces character to crouch when there's no room to stand
		if (!crouch)
		{
			if (Physics2D.OverlapCircle(m_TopDetection.position, 0.2f, m_GroundLayer))
			{
				crouch = true;
			}
		}

		// triggered when crouching
		if (crouch)
		{
			speed *= m_CrouchMultiplier;

			//	disable selected collider to half character's collision area
			if (m_CrouchDisableCollider != null)
			{
				m_CrouchDisableCollider.enabled = false;
			}

		} else {
			//	reenable collider when not crouching
			if (m_CrouchDisableCollider != null)
			{
				m_CrouchDisableCollider.enabled = true;
			}
		}

		// triggered when running
		if (dash && dashable == true)
		{
			StartCoroutine(Dash());
		}

		// apply velocity to the rigidbody to make object move
		if (m_DisableMovement == false)
		{
			Vector3 moveVelocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, moveVelocity, ref m_Velocity, m_MovementSlippery);
		}
		
		// flip character based on speed input, < 0 means moving right, > 0 means moving left
		FlipCharacter(speed);

		//only jump when player is on ground or have extra jumps left
		if ((grounded && jump) || ((jumpCount < m_JumpStack) && jump))	
		{
			grounded = false;

			// mitigate gravity when performing multiple jumps mid air
			if (m_Rigidbody2D.velocity.y < 0)
			{
				m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x, 0f);
				m_Rigidbody2D.AddForce(new Vector2(0.0f, m_JumpForce));
			} else if (jumpCount == 0) {
				m_Rigidbody2D.AddForce(new Vector2(0.0f, m_JumpForce * jump_charge));
			} else {
				m_Rigidbody2D.AddForce(new Vector2(0.0f, m_JumpForce));
			}
			jumpCount++;
		}
	}

	private void FlipCharacter(float speed)
	{
		if (speed > 0 && m_isFacingRight == false)	// moving right but facing left
		{
			m_isFacingRight = !m_isFacingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;					// reverse object
			transform.localScale = scale;
		}
		else if (speed < 0 && m_isFacingRight == true)	// moving left but facing right
		{
			m_isFacingRight = !m_isFacingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;						// reverse object
			transform.localScale = scale;
		}
	}
	private IEnumerator Dash()
	{
		dashable = false;
		m_DisableMovement = true;	// disable movement when dashing
		float gravity = m_Rigidbody2D.gravityScale;
		if (m_DashGravitySwitch == false)
		{
			m_Rigidbody2D.gravityScale = 0;		// set gravity to zero if gravity during dashing is not wanted
		}
		m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashDistance, 0f);
		yield return new WaitForSeconds(m_DashDuration);
		m_Rigidbody2D.gravityScale = gravity;	// return gravity to character
		m_DisableMovement = false;				// enable movement control after the dashing is done
		yield return new WaitForSeconds(m_DashCooldown);
		dashable = true;

	}
}
