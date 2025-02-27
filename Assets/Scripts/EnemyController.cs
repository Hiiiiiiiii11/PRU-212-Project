using Assets.Scripts;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class EnemyController : MonoBehaviour
{
	public float walkSpeed = 3f;
	public float walkStopRate = 0.6f;

	public enum WalkableDirection { Right, Left }

	private WalkableDirection _walkDirection;
	private Vector2 walkDirectionVector = Vector2.right;

	public bool CanMove
	{
		get => animator.GetBool(AnimationStrings.canMove);
	}

	public WalkableDirection WalkDirection
	{
		get { return _walkDirection; }
		set
		{
			if (_walkDirection != value)
			{
				transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * (value == WalkableDirection.Right ? 1 : -1), transform.localScale.y);

				if (value == WalkableDirection.Right)
				{
					walkDirectionVector = Vector2.right;
				}
				else if (value == WalkableDirection.Left)
				{
					walkDirectionVector = Vector2.left;
				}
			}
			_walkDirection = value;
		}
	}

	private bool _hasTarget = false;
	public bool HasTarget
	{
		get => _hasTarget;
		private set
		{
			_hasTarget = value;
			animator.SetBool(AnimationStrings.hasTarget, value);
		}
	}

	public float AttackCooldown { 
		get => animator.GetFloat(AnimationStrings.attackCooldown); 
		private set
		{
			animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
		}
	}

	Rigidbody2D rb;
	TouchingDirections touchingDirections;
	private Animator animator;
	public DetectionZone attackZone;
	public DetectionZone cliffZone;
	Damageable damageable;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		touchingDirections = GetComponent<TouchingDirections>();
		damageable = GetComponent<Damageable>();
	}

	void Update()
	{
		HasTarget = attackZone.detectionCollitions.Count > 0;

		if (AttackCooldown > 0)
			AttackCooldown -= Time.deltaTime;
	}

	private void FixedUpdate()
	{
		if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
		{
			FlipDirection();
		}

		if (!damageable.LockVelocity)
		{
			if (CanMove) rb.linearVelocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.linearVelocity.y);
			else rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
		}
		
	}

	private void FlipDirection()
	{
		WalkDirection = (WalkDirection == WalkableDirection.Right) ? WalkableDirection.Left : WalkableDirection.Right;
	}

	public void OnHit(int damage, Vector2 knockback)
	{
		rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
	}

	public void OnCliffDetected()
	{
		if (touchingDirections.IsGrounded)
		{
			FlipDirection();
		}
	}
}
