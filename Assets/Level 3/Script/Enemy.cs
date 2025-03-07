using UnityEngine;
namespace level3
{
    public class Enemy : MonoBehaviour
    {
        protected Rigidbody2D rb;
        protected Animator animator;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        public void JumpedOn()
        {
            animator.SetTrigger("isDeath");
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void Death()
        {
            Destroy(this.gameObject);
        }
    }
}