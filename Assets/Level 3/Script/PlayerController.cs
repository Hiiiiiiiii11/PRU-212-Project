using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace level3
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float lastOnGroundTime;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private GameObject attackPoint;
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.03f, 0.03f);
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask groundLayer = default;
        [SerializeField] private LayerMask wallLayer = default;

        [HideInInspector] public Vector2 move;
        [HideInInspector] private PlayerAnimation playerAnimation;
        [HideInInspector] private GameManager gameManager;
        private Rigidbody2D rb;
        private Animator animator;

        //Dash
        [HideInInspector] public bool isDashing;
        private bool isDashable = true;
        private bool dashButtonPressed;

        //Jump
        [HideInInspector] public bool isJumping;
        private bool jumpButtonPressed;

        //Wall Sliding and Wall Jump
        [HideInInspector] public bool isWallJumping;
        [HideInInspector] public bool isWallSliding;
        private RaycastHit2D wall;
        private float jumpTime;

        // Health
        [Header("Health")]
        public int maxHealth = 100;
        public int currentHealth;
        public HealthBar healthBar;
        [SerializeField] public float hurtForce = 10f;
        [SerializeField] public float attackRadius = 0.5f;

        //Movement data
        [Header("Run")]
        public float runMaxSpeed = 6f; //Target speed we want the player to reach.
        public float runAcceleration = 2f; //Time (approx.) time we want it to take for the player to accelerate from 0 to the runMaxSpeed.
        [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
        public float runDecceleration = 1f; //Time (approx.) we want it to take for the player to accelerate from runMaxSpeed to 0.
        [HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
        [Range(0.01f, 1)] public float accelInAir = 0.5f; //Multipliers applied to acceleration rate when airborne.
        [Range(0.01f, 1)] public float deccelInAir = 0.5f;
        public bool doConserveMomentum = true;
        [HideInInspector] private float runSFXCooldown = 0.2f; // Adjust delay as needed (seconds)
        [HideInInspector] private float lastRunSFXTime = 0f;   // Tracks last played time

        [Header("Jump")]
        public float jumpHeight = 10f; //Height of the player's jump

        [Header("Dash")]
        public float dashPower = 7f;
        public float dashingCoolDown = 0.5f;
        public float dashingTime = 0.2f;

        [Header("Wall Sliding and Wall Jumping")]
        public float wallDistance = 0.03f;
        [HideInInspector] public float wallJumpTime = 0.2f;
        public float wallSlideSpeed = 0.3f;
        public float wallJumpingYPower = 9f;
        public float wallJumpingXPower = 4f;
        public float WallJumpTimeInSecond = 0.1f;


        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerAnimation = GetComponent<PlayerAnimation>();
            gameManager = FindFirstObjectByType<GameManager>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        void Update()
        {
            if (isDashing || isWallJumping || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                return;
            if (Input.GetMouseButton(0))
            {
                AudioManager.instance.PlaySFX("Attack");
                Attack();
            }
            lastOnGroundTime -= Time.deltaTime;

            //Input Handler
            move.x = Input.GetAxisRaw("Horizontal");
            dashButtonPressed = Input.GetKeyDown(KeyCode.LeftShift);
            jumpButtonPressed = Input.GetButtonDown("Jump");

            Jump(false);

            if (move.x != 0)
                CheckDirectionToFace(move.x > 0);

            if (dashButtonPressed && isDashable && !isWallSliding)
                StartCoroutine(Dash());

            if (isWallSliding && jumpButtonPressed)
                StartCoroutine(WallJump());
        }

        void FixedUpdate()
        {
            if (isDashing || isWallJumping || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                return;
            if (!isWallSliding)
                Run();
            WallSliding();
        }

        private void Run()
        {
            float targetSpeed = move.x * runMaxSpeed;
            float accelRate;
            targetSpeed = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, 1);

            //Calculate Acceleration and Decceleration
            if (lastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;
            if (doConserveMomentum && Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.linearVelocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && lastOnGroundTime < 0)
                accelRate = 0;
            float speedDif = targetSpeed - rb.linearVelocity.x;
            float movement = speedDif * accelRate;

            //Implementing run
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
            if (Mathf.Abs(targetSpeed) > 0.01f && lastOnGroundTime > 0) // Only play when grounded
            {
                if (Time.time - lastRunSFXTime >= runSFXCooldown) // Check cooldown
                {
                    AudioManager.instance.PlaySFX("Running");
                    lastRunSFXTime = Time.time; // Update last played time
                }
            }
            else AudioManager.instance.StopSFX();
        }

        private IEnumerator Dash()
        {
            isDashable = false;
            isDashing = true;
            float originalGravivty = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(transform.localScale.x * dashPower, 0f);
            yield return new WaitForSeconds(dashingTime);
            rb.linearVelocity = new Vector2(Mathf.Sign(move.x) * runMaxSpeed, rb.linearVelocity.y);
            AudioManager.instance.PlaySFX("Dash");
            rb.gravityScale = originalGravivty;
            isDashing = false;
            yield return new WaitForSeconds(dashingCoolDown);
            isDashable = true;
        }

        private void Jump(bool isOnEnemy)
        {
            if (IsGrounded())
                isJumping = false;
            if (jumpButtonPressed && IsGrounded())
            {
                isJumping = true;
                AudioManager.instance.PlaySFX("Jump");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);                
            }
            else if (isOnEnemy)
            {
                AudioManager.instance.PlaySFX("Jump");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
            }
        }

        private void WallSliding()
        {
            if (!IsGrounded() && IsWalled())
            {
                isWallSliding = true;                
                jumpTime = Time.time + wallJumpTime;
            }
            else if (jumpTime < Time.time)
                isWallSliding = false;
            if (isWallSliding)
                rb.linearVelocity = new Vector2(0, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }

        private IEnumerator WallJump()
        {
            isWallJumping = true;
            isWallSliding = false;
            float direction = transform.localScale.x; // Get the facing direction
            rb.linearVelocity = new Vector2(-direction * wallJumpingXPower, wallJumpingYPower);// Apply force to move away from the wall
            AudioManager.instance.PlaySFX("Jump");
            yield return new WaitForSeconds(WallJumpTimeInSecond);
            isWallJumping = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    // Margin to account for box collider edges
                    float aboveMargin = 0.1f;

                    // Check if the player is above the enemy and falling down
                    bool isAboveEnemy = contact.point.y > enemy.transform.position.y + aboveMargin;
                    bool isFalling = rb.linearVelocity.y < 0;

                    if (isAboveEnemy && isFalling)
                    {
                        enemy.JumpedOn();
                        AudioManager.instance.PlaySFX("Monster Death");
                        gameManager.AddScore(1);
                        Jump(true);
                        return; // Exit function to avoid hurt logic
                    }
                }
                rb.linearVelocity = new Vector2(enemy.transform.position.x > transform.position.x ? -hurtForce : hurtForce, rb.linearVelocity.y);
                TakeDamage(20);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"Collided with: {collision.gameObject.name}");

            if (collision.gameObject.CompareTag("Coin"))
            {
                Destroy(collision.gameObject);
                AudioManager.instance.PlaySFX("Item");
                gameManager.AddScore(1);
            }
            else if (collision.gameObject.CompareTag("Trap"))
            {
                Debug.Log("Hit by trap");
                TakeDamage(20);
                Jump(true);
            }
            else if (collision.gameObject.CompareTag("FallBox"))
            {
                Debug.Log("Hit FallBox");
                playerAnimation.Death();
                AudioManager.instance.PlaySFX("Game Over");
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.simulated = false;
                gameManager.GameOver();
            }
            else if (collision.gameObject.CompareTag("WinBox"))
            {
                Debug.Log("Hit WinBox");
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.simulated = false;
                AudioManager.instance.musicSource.Stop();
                AudioManager.instance.PlaySFX("Level Completed");
                gameManager.GameWin();
            }
        }

        private void Attack()
        {
            // Detect enemies within the attack range using OverlapCollider
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemyLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    Debug.Log("Hit enemy");
                    enemyComponent.JumpedOn(); // Kill the enemy
                    AudioManager.instance.PlaySFX("Monster Death");
                    gameManager.AddScore(3);
                }
            }
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            AudioManager.instance.PlaySFX("Hit");
            healthBar.SetHealth(currentHealth);
            if (currentHealth > 0)
                playerAnimation.Hurt();
            else
            {
                playerAnimation.Death();
                AudioManager.instance.PlaySFX("Game Over");
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.simulated = false;
                gameManager.GameOver();
            }
        }

        public bool IsGrounded()
        {
            if (Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, groundLayer))
            {
                lastOnGroundTime = 0.1f;
                return true;
            }
            return false;
        }

        private bool IsWalled()
        {
            return Physics2D.OverlapCircle(wallCheckPoint.position, 0.2f, wallLayer);
        }

        public bool IsWallSliding()
        {
            if (Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, groundLayer))
            {
                lastOnGroundTime = 0.1f;
                return true;
            }
            return false;
        }

        private void CheckDirectionToFace(bool isMovingRight)
        {
            Vector3 temp = transform.localScale;
            temp.x = isMovingRight ? Mathf.Abs(temp.x) : -Mathf.Abs(temp.x);
            transform.localScale = temp;
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
        }

        private void OnValidate()
        {
            runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
            runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;
            runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
            runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        }
    }
}