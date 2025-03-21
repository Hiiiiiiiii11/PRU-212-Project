using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace level5
{
    [RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]
    public class PlayerController : MonoBehaviour
    {
        public float walkSpeed = 5f;
        public float jumpImpulse = 10f;
        public float airMoveSpeed = 3f;
        Vector2 moveInput;
        TouchingDirection touchingDirection;
        Damageable damageable;
        public float CurrentMoveSpeed
        {
            get
            {
                if (canMove)
                {
                    if (IsMoving && !touchingDirection.IsOnWall)
                    {
                        if (touchingDirection.IsGrounded)
                        {
                            return walkSpeed;
                        }
                        else
                        {
                            return airMoveSpeed;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        [SerializeField]
        private bool _isMoving = false;

        public bool IsMoving
        {
            get
            {
                return _isMoving;
            }
            private set
            {
                _isMoving = value;
                animator.SetBool(AnimationStrings.isMoving, value);
            }
        }

        public bool _isFacingRight = true;
        public bool IsFacingRight
        {
            get { return _isFacingRight; }
            private set
            {
                if (_isFacingRight != value)
                {
                    transform.localScale *= new Vector2(-1, 1);
                }
                _isFacingRight = value;
            }
        }

        public bool canMove
        {
            get
            {
                return animator.GetBool(AnimationStrings.canMove);
            }
        }

        public bool isAlive
        {
            get
            {
                return animator.GetBool(AnimationStrings.isAlive);
            }
        }

        Rigidbody2D rb;
        Animator animator;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            touchingDirection = GetComponent<TouchingDirection>();
            damageable = GetComponent<Damageable>();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (!damageable.LockVelocity)
                rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);

            animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            if (isAlive)
            {
                IsMoving = moveInput != Vector2.zero;
                SetFacingDirection(moveInput);
            }
            else
            {
                IsMoving = false;
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
            if (context.started && touchingDirection.IsGrounded && canMove)
            {
                animator.SetTrigger(AnimationStrings.jump);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                animator.SetTrigger(AnimationStrings.attack);
            }
        }

        public void OnHit(int damage, Vector2 knockback)
        {
            rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
        }
    }
}