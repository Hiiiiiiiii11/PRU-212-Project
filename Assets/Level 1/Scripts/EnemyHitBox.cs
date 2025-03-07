using UnityEngine;
namespace level1
{
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

                }
            }
        }
    }
}