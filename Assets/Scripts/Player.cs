using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private float JumpForce = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float dashSpeed = 60;
    private Animator animator;

    private int lastAttackType = -1;
    public float attackCooldown = 0.5f; // Thời gian chờ giữa các lần tấn công
    private float lastAttackTime;
    public float DashCooldown = 0;

    private bool isGrounded;
    private Rigidbody2D rb;
    private bool isTouchingWall; // Kiểm tra trạng thái tiếp xúc với tường
    private bool isWallClinging;
    private bool isDead = false;
    private bool isTrapped = false;
    private float originalSpeed;

    public void SetDead()
    {
        isDead = true;
        moveSpeed = 0;
        rb.linearVelocity = Vector2.zero;
        // Dừng mọi di chuyển
    }
    public void isTrap()
    {
        isTrapped = true;
        moveSpeed = 0; // Ngăn di chuyển
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Trapclose");

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isTrapped && Input.GetKeyDown(KeyCode.E)) // Press "E" to escape trap
        {
            isTrapped = false;
            moveSpeed = originalSpeed;
        }
        if (!isTrapped)
        {
            HandleMovement();
        }

        HandleJump();
        UpdateAnimation();
        HandleClingWall();
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            HanldeAttack();
        }
        HanldeDash();


    }


    private void HandleMovement()
    {
        // Nếu không bám tường mới cho phép di chuyển
        if (!isWallClinging || !isDead || !isTrapped)
        {
            float moveInput = Input.GetAxisRaw("Horizontal"); // Sử dụng Input.GetAxisRaw để có phản hồi tức thì
            Vector2 targetVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            // Interpolate velocity for smooth acceleration and deceleration
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.1f);

            if (!isDead) // Chỉ đổi hướng nếu nhân vật chưa chết
            {
                if (moveInput > 0)
                    transform.localScale = new Vector3(1, 1, 1);
                else if (moveInput < 0)
                    transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            return;
        }
    }



    private void HandleJump()
    {
        if (isDead || isTrapped)
            return;
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
        }
        // if (Input.GetButtonDown("Jump") && isGrounded)
        // {
        //     rb.linearVelocity = new Vector2(rb.linearVelocity.y, wallForce);
        // }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
        // isTouchingWall = Physics2D.OverlapCircle(groundCheck.position, 0.1f, wallLayer);
    }
    private void HandleClingWall()
    {
        // Kiểm tra tiếp xúc với tường và mặt đất
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);

        // Nếu tiếp xúc với tường và không ở trên mặt đất
        if (isTouchingWall && !isGrounded)
        {
            isWallClinging = true; // Chuyển sang trạng thái bám tường
            rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động
            rb.gravityScale = 0.2f; // Vô hiệu hóa trọng lực
            animator.SetBool("isClinging", true); // Cập nhật trạng thái trong Animator

        }
        else if (!isTouchingWall || isGrounded) // Thoát trạng thái bám tường khi chạm đất hoặc rời khỏi tường
        {
            isWallClinging = false;
            rb.gravityScale = 3; // Khôi phục trọng lực
            animator.SetBool("isClinging", false); // Cập nhật trạng thái trong Animator
        }

        // Nhảy khỏi tường nếu đang bám và nhấn Jump
        if (isWallClinging && Input.GetButtonDown("Jump"))
        {
            float moveInput = Input.GetAxisRaw("Horizontal"); // Lấy hướng di chuyển từ phím mũi tên hoặc A/D
            Vector2 jumpDirection;

            if (moveInput == 0) // Nếu không có hướng di chuyển
            {
                // Không thực hiện nhảy, chỉ dừng logic tại đây
                return;
            }
            else // Nhảy theo hướng trái hoặc phải nếu nhấn phím di chuyển
            {
                jumpDirection = new Vector2(moveInput, 1).normalized;

                // Kết hợp nhảy ngang và nhảy dọc
            }

            // Áp dụng lực nhảy
            rb.linearVelocity = jumpDirection * JumpForce * 1.5f;

            // Thoát trạng thái bám tường
            isWallClinging = false;
            rb.gravityScale = 3; // Khôi phục trọng lực
            animator.SetBool("isClinging", false); // Cập nhật trạng thái trong Animator
        }
    }



    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

    private void HanldeAttack()
    {
        // Lấy trạng thái hiện tại của animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Chỉ cho phép tấn công tiếp theo nếu animation hiện tại không phải "Attack" hoặc đã gần kết thúc
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime < 0.9f)
        {
            return;
        }

        // Chọn kiểu tấn công mới không trùng với lần trước
        int attackType = (lastAttackType + Random.Range(1, 3)) % 3 + 1;


        // Gán kiểu tấn công mới
        animator.SetInteger("AttackType", attackType);
        lastAttackType = attackType; // Cập nhật kiểu tấn công cuối cùng

        // Đánh dấu thời điểm tấn công và kích hoạt trigger
        lastAttackTime = Time.time;
        animator.SetTrigger("isAttacking");
    }

    private void HanldeDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector2 dashDirection = new Vector2(transform.localScale.x, 0); // Di chuyển theo chiều X
            rb.linearVelocity = dashDirection * dashSpeed;

            // Tạm dừng vận tốc trong thời gian dash
            animator.SetTrigger("Dash");
        }
    }

}

