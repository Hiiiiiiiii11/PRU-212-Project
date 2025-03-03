using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("EnemyHitBox") && collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
                Debug.Log($"[AttackHitbox] Player bị tấn công bởi {gameObject.name}");
            }
        }
        else
        {
            Debug.Log($"[AttackHitbox] Không gây sát thương - {gameObject.name} chạm vào {collision.gameObject.name}");
        }
    }
}
