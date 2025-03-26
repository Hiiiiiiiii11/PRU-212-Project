using level1;
using System.Collections;
using TMPro;
using UnityEngine;

namespace level3
{
    public class SkeletonController : MonoBehaviour
    {
        [HideInInspector] private Animator animator;
        [HideInInspector] private Rigidbody2D rb;
        [HideInInspector] private GameManager gameManager;
        [HideInInspector] private PlayerController playerController;
        [HideInInspector] private SpawnBox spawnBox;

        [Header("Attack")]
        [SerializeField] public Transform player;
        [SerializeField] public bool isFlipped = false;
        [SerializeField] public int attackDamage = 10;
        [SerializeField] private GameObject attackPoint;
        [SerializeField] public float attackRange = 0.7f;
        [SerializeField] public float enragedAttackRange = 1f;
        [SerializeField] public LayerMask attackMask;

        [Header("Health")]
        public HealthBar healthBar;
        [SerializeField] public int maxHealth = 300;
        [SerializeField] public int currentHealth;
        [SerializeField] float knockbackForce = 3f;
        [SerializeField] public float hurtForce = 10f;

        [Header("Damage Text")]
        [SerializeField] private TextMeshProUGUI healthText;

        private void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }
        }

        void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            gameManager = FindFirstObjectByType<GameManager>();
            playerController = FindFirstObjectByType<PlayerController>();
            spawnBox = FindFirstObjectByType<SpawnBox>();
            if (healthText == null)
            {
                healthText = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        public void LookAtPlayer()
        {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;

            if (transform.position.x > player.position.x && isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
            else if (transform.position.x < player.position.x && !isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }
            if (healthText != null)
            {
                Vector3 textScale = healthText.transform.localScale;
                textScale.x = isFlipped ? -Mathf.Abs(textScale.x) : Mathf.Abs(textScale.x);
                healthText.transform.localScale = textScale;
            }
        }

        public void Attack()
        {
            Vector3 pos = attackPoint.transform.position;
            Collider2D coll = Physics2D.OverlapCircle(pos, attackRange, attackMask);
            if (coll != null)
            {
                Vector2 attackDirection = (coll.transform.position - transform.position).normalized;
                coll.GetComponent<PlayerController>().TakeDamage(attackDamage, attackDirection);
            }
        }

        public void EnragedAttack()
        {
            Vector3 pos = attackPoint.transform.position;
            Collider2D coll = Physics2D.OverlapCircle(pos, enragedAttackRange, attackMask);
            if (coll != null)
            {
                Vector2 attackDirection = (coll.transform.position - transform.position).normalized;
                coll.GetComponent<PlayerController>().TakeDamage(attackDamage + 10, attackDirection);
            }
        }

        public void TakeDamage(int damage, Vector2 attackDirection)
        {
            if (currentHealth <= 0) return;
            currentHealth -= damage;
            UpdateHealth(currentHealth);
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 120)
            {
                animator.SetBool("isEnraged", true);
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (currentHealth <= 0)
            {
                animator.SetTrigger("isDeath");
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                AudioManager.instance.PlaySFX("Monster Death");
                spawnBox.SpawnKey();
                gameManager.AddScore(10);
            }
            else
                rb.AddForce(attackDirection.normalized * knockbackForce, ForceMode2D.Impulse);           
        }

        public void UpdateHealth(int health)
        {
            healthText.text = $"Health: {health}/300";
        }

        public void Death()
        {
            Destroy(this.gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
            Gizmos.DrawWireSphere(attackPoint.transform.position, enragedAttackRange);
        }
    }
}