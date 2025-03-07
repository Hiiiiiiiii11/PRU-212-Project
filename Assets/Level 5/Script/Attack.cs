using UnityEngine;
namespace level5
{
    public class Attack : MonoBehaviour
    {
        public int attackDamage = 10;
        public Vector2 knocback = Vector2.zero;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Damageable damageable = collision.GetComponent<Damageable>();
            if (damageable != null)
            {
                Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knocback : new Vector2(-knocback.x, knocback.y);
                bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            }
        }
    }
}