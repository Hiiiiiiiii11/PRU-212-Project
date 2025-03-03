
using UnityEngine;



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
    public float DashCooldown = 0.5f;
    [SerializeField] private float attackDamage = 1; // Sát thương mỗi đòn đánh
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject playerHitBox;

    private bool isGrounded;
    private Rigidbody2D rb;
    private bool isTouchingWall; // Kiểm tra trạng thái tiếp xúc với tường
    private bool isWallClinging;
    private bool isDead = false;
    private bool isTrapped = false;
    private float originalSpeed;

    [System.Obsolete]
    public void SetDead()
    {
        isDead = true;
        moveSpeed = 0;
        rb.linearVelocity = Vector2.zero;
        GameManager gameManager = FindObjectOfType<GameManager>();
        Invoke(nameof(DelayedGameOver), 2.5f);
    }

    [System.Obsolete]
    private void DelayedGameOver()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
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
        playerHitBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (isTrapped && Input.GetKeyDown(KeyCode.E)) // Press "E" to escape trap
            {
                isTrapped = false;
                moveSpeed = originalSpeed;
            }
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
            HandleAttack();
        }
        HanldeDash();


    }


    private void HandleMovement()
    {
        if (isWallClinging || isDead || isTrapped)
            return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 targetVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.1f);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
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
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);

        if (isTouchingWall && !isGrounded)
        {
            isWallClinging = true;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * 0.1f); // Giảm tốc độ rơi

            rb.gravityScale = 0; // Dừng rơi
            animator.SetBool("isClinging", true);

            if (Input.GetButtonDown("Jump"))
            {
                float jumpDirection = Input.GetAxisRaw("Horizontal"); // -1 (A), 1 (D), 0 (không nhấn)
                if (jumpDirection == 0)
                {
                }
                else
                {
                    rb.linearVelocity = new Vector2(jumpDirection * moveSpeed, JumpForce); // Nhảy theo hướng
                }

                isWallClinging = false; // Thoát bám tường
                rb.gravityScale = 3; // Khôi phục trọng lực
                animator.SetBool("isClinging", false);
            }
        }
        else
        {
            isWallClinging = false;
            rb.gravityScale = 3;
            animator.SetBool("isClinging", false);
        }
    }





    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

    private void HandleAttack()
    {
        // Kiểm tra nếu đang tấn công thì không thực hiện tiếp
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime < 0.9f) return;

        // Chọn kiểu tấn công mới nhưng không được trùng quá 2 lần liên tiếp
        int attackType;
        do
        {
            attackType = Random.Range(1, 4); // Random từ 1 đến 3
        } while (attackType == lastAttackType);

        animator.SetInteger("AttackType", attackType);
        lastAttackType = attackType;

        lastAttackTime = Time.time;
        animator.SetTrigger("isAttacking");

        // Bật Hitbox khi tấn công
        playerHitBox.SetActive(true);

        // Tắt Hitbox sau một khoảng thời gian (để tránh gây damage liên tục)
        Invoke(nameof(DisableHitbox), 0.5f);
    }


    private void DisableHitbox()
    {
        playerHitBox.SetActive(false);
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

    // void DamageEnemy()
    // {
    //     Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 2f);
    //     foreach (Collider2D enemy in hitEnemies)
    //     {
    //         if (enemy.CompareTag("Enemy"))
    //         {
    //             // Kiểm tra xem enemy là loại nào và gọi TakeDamage()
    //             Skeleton skeleton = enemy.GetComponent<Skeleton>();
    //             if (skeleton != null) skeleton.TakeDamage(1);

    //             Eyefly eyeFly = enemy.GetComponent<Eyefly>();
    //             if (eyeFly != null) eyeFly.TakeDamage(1);

    //             Goblin goblin = enemy.GetComponent<Goblin>();
    //             if (goblin != null) goblin.TakeDamage(1);

    //             Mushroom mushroom = enemy.GetComponent<Mushroom>();
    //             if (mushroom != null) mushroom.TakeDamage(1);
    //         }
    //     }
    // }






}

