using System;
using UnityEngine;

public class Eyefly : MonoBehaviour
{
    [Header("Di chuyển")]

    [SerializeField] private float startingHealth;
    private float currentHealth { get; set; }
    private bool isDead = false;
    public float walkSpeed = 2f;      // Tốc độ di chuyển của enemy
    public float leftDistance = 3f;   // Khoảng cách tối đa di chuyển sang trái
    public float rightDistance = 3f;  // Khoảng cách tối đa di chuyển sang phải

    [Header("Ground Check")]
    public Transform groundCheckEnemy;


    private Animator animator;
    private Rigidbody2D rb;
    private bool movingRight = true;
    private bool isAttacking = false;

    private float leftLimit;
    private float rightLimit;

    public DetectingZone AttackZone;
    private Transform player;
    public GameObject attackHitbox;

    private bool hasTarget = false;
    public bool HasTarget
    {
        get { return hasTarget; }
        private set
        {
            if (hasTarget != value)
            {
                hasTarget = value;
                animator.SetBool("hasTarget", hasTarget);
                if (!hasTarget)
                {
                    // CancelAttack();
                }
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


        if (isAttacking || HasTarget) return; // Nếu đang tấn công hoặc có mục tiêu, không di chuyển

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
        Invoke("EnableHitbox", 0.55f); // Chờ 0.2s trước khi bật hitbox để khớp với animation
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
        rb.gravityScale = 0; // Tăng trọng lực để rơi nhanh hơn

        // Không tắt Collider ngay lập tức, chỉ tắt trigger nếu có
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = false;

        rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động trước đó
    }

    // Khi va chạm với mặt đất, enemy sẽ bị vô hiệu hóa hoàn toàn
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.simulated = false; // Dừng vật lý
        }
    }
    private void EnableGravity()
    {
        rb.gravityScale = 2; // Bắt đầu rơi xuống
    }


}
