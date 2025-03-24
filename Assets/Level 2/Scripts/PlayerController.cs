using UnityEngine;
namespace level2
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 15f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheck;
        private Animator animator;

        private bool isGrounded;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
            HandleJump();
            UpdateAnimation();
        }
        private void HandleMovement()
        {
            float moveInput = Input.GetAxis("Horizontal");

            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            Vector3 currentScale = transform.localScale;

            if (moveInput > 0 && currentScale.x < 0) transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);

            else if (moveInput < 0 && currentScale.x > 0) transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }

        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        private void UpdateAnimation()
        {
            float isRunning = Mathf.Abs(rb.linearVelocity.x);
            bool isJumping = !isGrounded;
            animator.SetFloat("Speed", isRunning);
            animator.SetBool("Grounded", !isJumping);
        }
    }
}