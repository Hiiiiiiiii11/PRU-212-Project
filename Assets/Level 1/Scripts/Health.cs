
using Unity.VisualScripting;
using UnityEngine;
namespace level1
{
    public class Health : MonoBehaviour
    {
        private GameManager gameManager;
        [SerializeField] private float startingHealth;
        private Animator animator;
        private bool dead;
        public float currentHealth { get; private set; }

        private Audio audio;
        private bool isHurt = false;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            animator = GetComponent<Animator>();
            currentHealth = startingHealth;
            audio = FindAnyObjectByType<Audio>();
            gameManager = FindAnyObjectByType<GameManager>();
        }
        public void TakeDamage(float _damage)
        {
            if (isHurt || dead) return; // Nếu đang bị thương hoặc đã chết, không nhận sát thương nữa

            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

            if (currentHealth > 0)
            {
                isHurt = true;
                audio.PlayPlayerHurt();
                animator.SetTrigger("PlayerHurt");

                // **Hủy trạng thái Attack hoặc chạy nếu có**
                animator.ResetTrigger("isAttacking");
                animator.SetBool("isRunning", false);

                // Chặn di chuyển trong 0.3 giây
                GetComponent<Player>().moveSpeed = 0;
                Invoke(nameof(RecoverFromHurt), 0.3f);
            }
            else
            {
                if (!dead)
                {
                    dead = true;
                    animator.SetTrigger("PlayerHurt");
                    audio.PlayPlayerHurt();
                    animator.SetTrigger("PlayerDeath");

                    GetComponent<Player>().SetDead();
                }
            }
        }

        private void RecoverFromHurt()
        {
            isHurt = false;
            GetComponent<Player>().moveSpeed = GetComponent<Player>().originalSpeed; // Phục hồi tốc độ di chuyển
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("HealingItem") && currentHealth < startingHealth)
            {
                Heal(1);
                audio.HealingItem();
                Destroy(collision.gameObject); // Xóa vật thể hồi máu sau khi sử dụng
            }
            if (collision.CompareTag("Clock"))
            {
                gameManager.AddPragment(1);
                audio.PickUpItem();
                Destroy(collision.gameObject);
            }
            if (collision.CompareTag("Portal")) // Kiểm tra nếu chạm vào cổng
            {
                if (gameManager != null && gameManager.GetClockPragment() == 1) // Kiểm tra nếu Clock Pragment >= 1
                {
                    audio.PlayEnterPortal();
                    Invoke(nameof(DelayGameWin), 2f); // Delay 2 giây trước khi hiển thị UI
                }
                else
                {
                    gameManager.ShowNeedPragmentText();
                }
            }

        }
        private void DelayGameWin()
        {
            gameManager.GameWin(); // Gọi hàm hiển thị Win UI sau 2 giây
        }

        private void Heal(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        }
    }
}
