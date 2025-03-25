using System;
using UnityEngine;
namespace level5
{
    [RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]
    public class Lizard : MonoBehaviour
    {
        public float walkAcceleration = 3f;
        public float maxSpeed = 3f;
        public float walkStopRate = 0.1f;
        public DetectionZone attackZone;
        public DetectionZone cliffDetectionZone;

        Rigidbody2D rb;
        TouchingDirection touchingDirection;
        Animator animator;
        Damageable damageable;
        public enum WalkableDirection { Right, Left }
        private WalkableDirection _walkDirection = WalkableDirection.Right;
        private Vector2 walkDirectionVector = Vector2.right;

        public WalkableDirection WalkDirection
        {
            get { return _walkDirection; }
            set
            {
                if (_walkDirection != value)
                {
                    float newScaleX = (value == WalkableDirection.Right) ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x);
                    gameObject.transform.localScale = new Vector2(newScaleX, transform.localScale.y);

                    walkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
                }
                _walkDirection = value;
            }
        }
        public bool _hasTarget = false;
        public bool HasTarget
        {
            get { return _hasTarget; }
            private set
            {
                _hasTarget = value;
                animator.SetBool(AnimationStrings.hasTarget, value);
            }
        }

        public bool CanMove
        {
            get
            {
                return animator.GetBool(AnimationStrings.canMove);
            }
        }

        public float AttackCooldown
        {
            get
            {
                return animator.GetFloat(AnimationStrings.attackCooldown);
            }
            private set
            {
                animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
            }
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            touchingDirection = GetComponent<TouchingDirection>();
            animator = GetComponent<Animator>();
            damageable = GetComponent<Damageable>();
        }

        private void Update()
        {
            HasTarget = attackZone.detectedColliders.Count > 0;
            if (AttackCooldown > 0)
            {
                AttackCooldown -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            if (touchingDirection.IsGrounded && touchingDirection.IsOnWall)
            {
                FlipDirection();
            }
            if (!damageable.LockVelocity)
            {
                if (CanMove)

                    rb.linearVelocity = new Vector2(
                        Mathf.Clamp(rb.linearVelocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed),
                        rb.linearVelocity.y);
                else
                    rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
            }
        }

        private void FlipDirection()
        {
            if (WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else if (WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else
            {
                Debug.LogError("Kill me");
            }
        }

        public void onHit(int damage, Vector2 knockback)
        {
            rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
        }

        public void onCliffDetected()
        {
            if (touchingDirection.IsGrounded)
            {
                FlipDirection();
            }
        }
    }
}