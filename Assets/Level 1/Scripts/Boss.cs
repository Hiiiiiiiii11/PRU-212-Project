using UnityEngine;
namespace level1
{
    public class Boss : MonoBehaviour
    {
        [SerializeField] private float startingHealth;

        [Header("Di chuyển")]
        public float walkSpeed = 4f;
        public float leftDistance = 15f;
        public float rightDistance = 10f;

        [Header("Ground Check")]
        public Transform groundCheckEnemy;
        public float groundCheckRadius = 0.2f;
        public LayerMask groundLayer;

        private Animator animator;
        private Rigidbody2D rb;
        private bool movingRight = true;
        private bool isAttacking = false;
        private bool isHurt = false; // Trạng thái bị thương

        private float leftLimit;
        private float rightLimit;

        public DetectingZone AttackZone;
        private Transform player;
        public float currentHealth { get; private set; }

        private bool hasTarget = false;
        private bool isDead = false;

        [Header("Hitbox Attack")]
        public GameObject attackHitbox;
        private Audio audio;
        private bool bossFight = false;

        // New fields for enrage logic
        private bool isEnraged = false;
        public float attackDamage = 1f; // Initial boss attack damage

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
                        CancelAttack();
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
            audio = FindAnyObjectByType<Audio>();

            leftLimit = transform.position.x - leftDistance;
            rightLimit = transform.position.x + rightDistance;

            // Ẩn hitbox ban đầu
            attackHitbox.SetActive(false);
        }

        void Update()
        {
            if (isHurt || isDead) return; // Nếu đang bị thương hoặc đã chết, không làm gì cả

            HasTarget = AttackZone.detectedColliders.Exists(c => c.CompareTag("Player"));

            if (HasTarget && !isAttacking)
            {
                Attack();
            }

            // You might still want to check distance for other audio effects here.
            CheckPlayerDistanceForBossFightAudio();
        }

        void FixedUpdate()
        {
            if (isHurt || isDead) return; // Không di chuyển khi đang bị thương hoặc đã chết

            bool isGrounded = Physics2D.OverlapCircle(groundCheckEnemy.position, groundCheckRadius, groundLayer);
            if (!isGrounded) return;

            if (isAttacking || HasTarget) return;

            Move();
        }

        void Move()
        {
            if (!hasTarget)
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
        }

        void Attack()
        {
            if (isAttacking || isHurt) return; // Không tấn công khi đang bị thương

            isAttacking = true;
            animator.SetBool("isAttack", true);
            rb.linearVelocity = Vector2.zero; // Dừng di chuyển khi tấn công
        }

        private void AttackAudio()
        {
            audio.PlayAttackBoss();
        }

        private void DeadAudio()
        {
            audio.PlayBossDead();
        }

        private void BossRoar()
        {
            audio.PlayBossRoar();
        }

        void ResetAttack()
        {
            isAttacking = false;
            animator.SetBool("isAttack", false);
        }

        void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        public void EnableHitbox()
        {
            attackHitbox.SetActive(true);
        }

        public void DisableHitbox()
        {
            attackHitbox.SetActive(false);
        }

        public void TakeDamage(float _damage)
        {
            if (isDead || isHurt) return; // Không nhận sát thương khi đã chết hoặc đang bị thương

            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

            // Check if boss health falls below 50% and if not already enraged.
            if (!isEnraged && currentHealth <= startingHealth * 0.5f)
            {
                isEnraged = true;
                attackDamage = 1f;
                walkSpeed = 20f; // Increase boss damage by 1
                BossRoar();
                // Play boss roar audio to signal enrage
            }

            if (currentHealth > 0)
            {
                isHurt = true; // Đánh dấu là đang bị thương
                animator.SetTrigger("isHurt");
                CancelAttack();

                // Gọi hàm ResetHurt sau 0.5s để kết thúc trạng thái bị thương
                Invoke(nameof(ResetHurt), 0.5f);
            }
            else
            {
                Die();
            }
        }

        private void TouchEnemy()
        {
            audio.AttackOnEnemy();
        }

        void ResetHurt()
        {
            isHurt = false;
        }

        void CancelAttack()
        {
            isAttacking = false;
            animator.SetBool("isAttack", false);
        }

        private void Die()
        {
            if (isDead) return;
            animator.SetTrigger("isHurt");
            isDead = true;
            animator.SetBool("isDead", true);
            GetComponent<Collider2D>().enabled = false;
            rb.simulated = false;
        }

        void CheckPlayerDistanceForBossFightAudio()
        {
            if (player == null) return;
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= 10f && !bossFight)
            {
                bossFight = true;
                BossRoar();
            }
            else if (distance > 50f)
            {
                bossFight = false;
            }
        }
    }
}
