using level3;
using UnityEngine;
namespace level3 
{
    public class SkeletonController : MonoBehaviour
    {
        [SerializeField] public Transform player;
        [SerializeField] public bool isFlipped = false;
        [SerializeField] public int attackDamage = 20;
        [SerializeField] private GameObject attackPoint;
        [SerializeField] public float attackRange = 1f;
        [SerializeField] public LayerMask attackMask;

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
        }

        public void Attack()
        {
            Vector3 pos = attackPoint.transform.position;
            Collider2D coll = Physics2D.OverlapCircle(pos, attackRange, attackMask);
            if (coll != null)
                coll.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
        }
    } 
}
