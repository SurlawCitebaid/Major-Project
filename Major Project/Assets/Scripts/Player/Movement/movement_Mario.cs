using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class movement_Mario : MonoBehaviour
{

	//variables
	[Header("General")][Space]
	public PlayerAttack m_playerAttack;
	public PlayerController m_playerController;
	[Range(0, .5f)] [SerializeField] private float m_CollisionRadius = 0.1f;
	[Space][Space]

	[Header("Movement")][Space]
	[Range(0, 1000)] private float m_MoveSpeed;
	[Range(0, .3f)] [SerializeField] private float m_MovementSlippery = 0.05f;
	private bool m_DisableMovement = false;
	[Space][Space]

	[Header("Jump")][Space]
	[SerializeField] private LayerMask m_GroundLayer; // all things character can land on - can be set to multiple
	[SerializeField] private int m_JumpStack = 2;
	[SerializeField] float m_JumpDuration = 0.35f;
	private float m_JumpForce;
	private int jumpCount = 0;
	[Space][Space]

	[Header("Crouch")][Space]
	[Range(0, 1)] [SerializeField] private float m_CrouchMultiplier = 0.4f;	
	[SerializeField] private Collider2D m_CrouchDisableCollider;
	[Space][Space]

	[Header("Dash")][Space]
	[Range(0, 100)][SerializeField] private float m_DashDistance = 25f;
	[SerializeField] private float m_DashDuration = 0.2f;
	[Range(0, 5)][SerializeField] private float m_DashCooldown = 1f; // in seconds
	[SerializeField] private bool m_DashGravitySwitch = false;
	[Space][Space]

	[Header("Misc")][Space]
	[SerializeField] private Transform m_FrontDetection;
	[SerializeField] private Transform m_BottomDetection;
	[SerializeField] private Transform m_TopDetection;
	[Range(-5, 5)][SerializeField] private float m_WallFriction = 0f;
	[Range(0, 5)][SerializeField] private float m_WallGrabDuration = 2.0f;
	[Space][Space]

	private bool dashable = true;
	private bool isGrounded, isTouchingWall, isWallGrabing, isfalling;
	private Rigidbody2D m_Rigidbody2D;
	private bool m_isFacingRight = true;
	private bool isWallJumping = false;
	private bool isDashing = false;
	private Vector3 m_Velocity = Vector3.zero;
	private float speed;
	private float gravity;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		gravity = m_Rigidbody2D.gravityScale;
	}

	private void FixedUpdate()
	{
		isGrounded = false;
		// set character state to grounded when intact with any part of floor's colliders
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_BottomDetection.position, m_CollisionRadius, m_GroundLayer);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				isGrounded = true;
				jumpCount = 0;
			}
		}

		// wall grab detect
		if (isTouchingWall && !isGrounded && speed != 0 && !isWallJumping)
		{
			isWallGrabing = true;
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_WallFriction);
			jumpCount = 0;
		} else {
			isWallGrabing = false;
		}

		if (m_Rigidbody2D.velocity.y > 0)
		{
			isfalling = false;
		} else {
			isfalling = true;
		}
	}
	public void Move(float move_direction, bool crouch, bool dash)
	{
		speed = move_direction * m_MoveSpeed;


		// triggered when running
		if (dash && dashable == true)
		{
			StartCoroutine(Dash());
		}

		isTouchingWall = Physics2D.OverlapCircle(m_FrontDetection.position, m_CollisionRadius, m_GroundLayer);
		

		// apply velocity to the rigidbody to make object move
		if (m_DisableMovement == false)
		{
			Vector3 moveVelocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, moveVelocity, ref m_Velocity, m_MovementSlippery);
		}

		// flip character based on speed input, < 0 means moving right, > 0 means moving left
		// suitable only when character object have only a single component
		if (move_direction > 0 && !m_isFacingRight)
		{
			FlipCharacter();
		}
		else if (move_direction < 0 && m_isFacingRight)
		{
			FlipCharacter();
		}

	}
	private void FlipCharacter()
	{
		m_isFacingRight = !m_isFacingRight; // moving right but facing left
		transform.Rotate(0f, 180f, 0f);
	}
	private IEnumerator Dash()
	{
		bool pre_inv = true;
		dashable = false;
		m_DisableMovement = true;						// disable movement when dashing
		isDashing = true;
		if ( m_playerController.isInvincible == false)  // differ dash inv and inv from other actions
		{
			m_playerController.isInvincible = true;
			pre_inv = false;
		}
		if (m_DashGravitySwitch == false)
		{
			m_Rigidbody2D.gravityScale = 0;				// set gravity to zero if gravity during dashing is not wanted
		}
		m_Rigidbody2D.velocity = new Vector2(transform.right.x * m_DashDistance, 0f);
		FindObjectOfType<AudioManager>().Play("PlayerDash");
		yield return new WaitForSeconds(m_DashDuration);
		m_Rigidbody2D.gravityScale = gravity;			// return gravity to character
		m_DisableMovement = false;						// enable movement control after the dashing is done
		isDashing = false;
		if (pre_inv == false)							// only turn off inv when the inv was given by dash
		{
			m_playerController.isInvincible = false;    // disable invincible state
		}
		yield return new WaitForSeconds(m_DashCooldown);
		dashable = true;
	}
	private IEnumerator disableMovement()
	{
		m_DisableMovement = true;
		m_Rigidbody2D.gravityScale = 0;
		yield return new WaitForSeconds(0.2f);
		m_DisableMovement = false;
		isWallJumping = false;
		m_Rigidbody2D.gravityScale = gravity;
	}
	public void Jump()
	{
		if (jumpCount < m_JumpStack)
		{
			isGrounded = false;
			m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x, 0f);
			m_Rigidbody2D.AddForce(new Vector2(0.0f, m_JumpForce), ForceMode2D.Impulse);
		}
	}
	public void WallJump()
	{
		isWallJumping = true;
		float force = 0;
		if (m_isFacingRight)
		{
			force = -7f;
		} else {
			force = 7f;
		}
		m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x, 0f);
		m_Rigidbody2D.AddForce(new Vector2(force, m_JumpForce), ForceMode2D.Impulse);
		FlipCharacter();
		StartCoroutine(disableMovement());
	}
	public void addJumpCount()
	{
		jumpCount++;
	}
	public float getJumpDuration()
	{
		return m_JumpDuration;
	}
	public bool getGrounded()
	{
		return isGrounded;
	}
	public bool getTouchingWall()
	{
		return isTouchingWall;
	}
	public bool getWallGrabing()
	{
		return isWallGrabing;
	}
	public bool getFalling()
	{
		return isfalling;
	}
	public bool getDashing()
	{
		return isDashing;
	}
	public float getMoveSpeed()
	{
		return m_MoveSpeed;
	}
	public void disableMovement(string bruh)
	{
		switch (bruh)
		{
			case "true":
				m_DisableMovement = true;
				break;
			case "false":
				m_DisableMovement = false;
				break;
		}
	}
	public void SetJumpForce(float value)
    {
		m_JumpForce = value;
    }
	public void setMoveSpeed(float value)
	{
		// bruh somebody want dat speed limit shit so fucking cringe lmaoooooooooooooo
		m_MoveSpeed = value;
	}
}
