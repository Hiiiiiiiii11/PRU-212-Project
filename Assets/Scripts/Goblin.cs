using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField] private float startingHealth;

    [Header("Di chuyển")]
    public float walkSpeed = 2f;
    public float leftDistance = 3f;
    public float rightDistance = 3f;

    [Header("Ground Check")]
    public Transform groundCheckEnemy;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Animator animator;
    private Rigidbody2D rb;
    private bool movingRight = true;
    private bool isAttacking = false;

    private float leftLimit;
    private float rightLimit;

    public DetectingZone AttackZone;
    private Transform player;
    public float currentHealth { get; private set; }

    private bool hasTarget = false;
    private bool isDead = false;

    [Header("Hitbox Attack")]
    public GameObject attackHitbox;


    public bool HasTarget
    {
        get { return hasTarget; }
        private set
        {
            if (hasTarget != value)
            {
                hasTarget = value;
                animator.SetBool("hasTarget", hasTarget);
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = startingHealth;

        leftLimit = transform.position.x - leftDistance;
        rightLimit = transform.position.x + rightDistance;


        // Ẩn hitbox ban đầu
        attackHitbox.SetActive(false);
    }

    void Update()
    {
        HasTarget = AttackZone.detectedColliders.Exists(c => c.CompareTag("Player"));

        if (HasTarget && !isAttacking)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheckEnemy.position, groundCheckRadius, groundLayer);
        if (!isGrounded) return;

        if (isAttacking || HasTarget) return;

        Move();
    }

    void Move()
    {
        Vector2 currentPos = transform.position;

        if (movingRight)
        {
            rb.linearVelocity = new Vector2(walkSpeed, rb.linearVelocity.y);
            if (currentPos.x >= rightLimit)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-walkSpeed, rb.linearVelocity.y);
            if (currentPos.x <= leftLimit)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetBool("isAttack", true);

        // Dừng di chuyển
        rb.linearVelocity = Vector2.zero;

        // Bật hitbox khi animation bắt đầu
        Invoke("EnableHitbox", 0.45f); // Chờ 0.2s trước khi bật hitbox để khớp với animation
        Invoke("ResetAttack", 1f);
    }

    void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttack", false);

        // Tắt hitbox khi animation kết thúc
        DisableHitbox();

        if (!HasTarget)
        {
            walkSpeed = 2f;
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // 👉 Khi animation bắt đầu, mở hitbox
    public void EnableHitbox()
    {
        attackHitbox.SetActive(true);

    }

    // 👉 Khi animation kết thúc, đóng hitbox
    public void DisableHitbox()
    {
        attackHitbox.SetActive(false);
    }

    public void TakeDamage(float _damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        CancelAttack();

        if (currentHealth > 0)
        {
            animator.SetTrigger("isHurt");
        }
        else
        {
            animator.SetTrigger("isHurt");
            Die();
        }
    }

    void CancelAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttack", false);
        walkSpeed = 2f;
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;
    }
}
