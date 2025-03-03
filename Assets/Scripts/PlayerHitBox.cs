using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private float attackDamage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out Skeleton skeleton)) skeleton.TakeDamage(attackDamage);
            if (collision.TryGetComponent(out Eyefly eyeFly)) eyeFly.TakeDamage(attackDamage);
            if (collision.TryGetComponent(out Goblin goblin)) goblin.TakeDamage(attackDamage);
            if (collision.TryGetComponent(out Mushroom mushroom)) mushroom.TakeDamage(attackDamage);
        }
    }
}
