using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Kiểm tra nếu va chạm với kẻ địch
        {
            other.GetComponent<Skeleton>()?.TakeDamage(damage);
            other.GetComponent<Goblin>()?.TakeDamage(damage);
            other.GetComponent<Eyefly>()?.TakeDamage(damage);
            other.GetComponent<Mushroom>()?.TakeDamage(damage);
        }
    }
}
