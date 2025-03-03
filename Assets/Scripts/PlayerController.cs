using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
	public float runSpeed = 250f;
	public float jumpImpulse = 9f;

	public float dashSpeed = 15f;
	public float dashDuration = 0.3f;
	public float dashCooldown = 1f;

	public float wallSlideSpeed = 0f;
	public float wallJumpForceX = 7f;
	public float wallJumpForceY = 7f;
	public float wallJumpCooldown = 0.2f;
	public float wallJumpDuration = 0.5f;

	private bool canDash = true;
	private bool canWallJump = true;
	Vector2 moveInput;

	TouchingDirections touchingDirection;
	Rigidbody2D rb;
	Animator animator;
	Damageable damageable;
	public UIManager uiManager;

	[SerializeField]
	private bool _isWallJumpping = false;
	public bool IsWallJumpping
	{
		get => _isWallJumpping;
		private set
		{
			_isWallJumpping = value;
			animator.SetBool(AnimationStrings.wallJump, value);
		}
	}
	[SerializeField] private bool _isDashing = false;
	public bool IsDashing
	{
		get => _isDashing;
		private set
		{
			_isDashing = value;
			animator.SetBool(AnimationStrings.isDashing, value);
		}
	}

	[SerializeField]
	private bool _isRunning = false;
	public bool IsRunning
	{
		get => _isRunning;
		private set
		{
			_isRunning = value;
			animator.SetBool(AnimationStrings.isRunning, value);
		}
	}

	[SerializeField]
	private bool _isFacingRight = true;
	public bool IsFacingRight
	{
		get => _isFacingRight;
		private set
		{
			if (_isFacingRight != value)
			{
				transform.localScale *= new Vector2(-1, 1);
			}
			_isFacingRight = value;
		}
	}

	public bool CanMove => animator.GetBool(AnimationStrings.canMove);
	public bool IsAlive => animator.GetBool(AnimationStrings.isAlive);

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		touchingDirection = GetComponent<TouchingDirections>();
		damageable = GetComponent<Damageable>();
	}

	private void Update()
	{
		HandleWallSlide();

		if (!damageable.IsAlive) StartCoroutine(ShowGameOver());
	}

	private IEnumerator ShowGameOver()
	{
		yield return new WaitForSeconds(2f);
		uiManager.GameOver();
	}

	private void FixedUpdate()
	{
		if (!damageable.LockVelocity && !IsDashing && !IsWallJumpping && CanMove)
		{
			rb.linearVelocity = new Vector2(moveInput.x * runSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
		}
		else if (!CanMove)
		{
			rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
		}

		animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		if (CanMove)
		{
			moveInput = context.ReadValue<Vector2>();

			if (IsAlive)
			{
				IsRunning = moveInput != Vector2.zero;
				SetFacingDirection(moveInput);
			}
			else
			{
				IsRunning = false;
			}
		}
	}

	private void SetFacingDirection(Vector2 moveInput)
	{
		if (moveInput.x > 0 && !IsFacingRight)
		{
			IsFacingRight = true;
		}
		else if (moveInput.x < 0 && IsFacingRight)
		{
			IsFacingRight = false;
		}
	}
	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			if (touchingDirection.IsGrounded && CanMove)
			{
				animator.SetTrigger(AnimationStrings.jump);
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
			}
			else if (touchingDirection.IsOnWall && !touchingDirection.IsGrounded && canWallJump)
			{
				StartCoroutine(WallJump());
			}
		}
	}
	private void HandleWallSlide()
	{
		if (touchingDirection.IsOnWall && !touchingDirection.IsGrounded && !IsWallJumpping)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
		}
	}

	private IEnumerator WallJump()
	{
		IsWallJumpping = true;
		canWallJump = false;

		float jumpDirection = IsFacingRight ? -1 : 1;

		rb.linearVelocity = new Vector2(wallJumpForceX * jumpDirection, wallJumpForceY);

		IsFacingRight = !IsFacingRight;

		yield return new WaitForSeconds(wallJumpDuration);

		IsWallJumpping = false;

		yield return new WaitForSeconds(wallJumpCooldown);
		canWallJump = true;
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		if (context.started && touchingDirection.IsGrounded && !IsDashing && !IsWallJumpping && !touchingDirection.IsOnWall)
		{
			animator.SetTrigger(AnimationStrings.attack);
			moveInput = Vector2.zero;
			IsRunning = false;
		}
	}

	public void OnHit(int damage, Vector2 knockback)
	{
		rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		if (context.started && canDash)
		{
			StartCoroutine(PerformDash());
		}
	}

	private IEnumerator PerformDash()
	{
		IsDashing = true;
		canDash = false;

		float originalGravity = rb.gravityScale;
		rb.gravityScale = 0; 
		rb.linearVelocity = new Vector2(IsFacingRight ? dashSpeed : -dashSpeed, 0);

		yield return new WaitForSeconds(dashDuration);

		rb.gravityScale = originalGravity;
		IsDashing = false;

		yield return new WaitForSeconds(dashCooldown);
		canDash = true;
	}
}
