using UnityEngine;

namespace level3
{
    public class PigController : Enemy
    {
        [SerializeField] private float leftCap;
        [SerializeField] private float rightCap;
        [SerializeField] private float runSpeed = 3f;
        [SerializeField] private Color gizmoColor = Color.green;

        private Collider2D coll;
        private bool facingLeft = true;

        protected override void Start()
        {
            base.Start();
            coll = GetComponent<Collider2D>();
        }

        void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector3 temp = transform.localScale;
            if (facingLeft)
            {
                if (transform.position.x > leftCap)
                {
                    if (transform.localScale.x != -Mathf.Abs(temp.x))
                    {
                        temp.x = -Mathf.Abs(temp.x);
                        transform.localScale = temp;
                    }
                    rb.linearVelocity = new Vector2(-runSpeed, rb.linearVelocity.y);
                }
                else facingLeft = false;
            }
            else
            {
                if (transform.position.x < rightCap)
                {
                    if (transform.localScale.x != Mathf.Abs(temp.x))
                    {
                        temp.x = Mathf.Abs(temp.x);
                        transform.localScale = temp;
                    }
                    rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
                }
                else facingLeft = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            // Draw patrol boundaries as vertical lines
            Gizmos.DrawLine(new Vector3(leftCap, transform.position.y - 0.5f, 0),
                            new Vector3(leftCap, transform.position.y + 0.5f, 0));

            Gizmos.DrawLine(new Vector3(rightCap, transform.position.y - 0.5f, 0),
                            new Vector3(rightCap, transform.position.y + 0.5f, 0));

            // Draw a line showing the patrol path
            Gizmos.DrawLine(new Vector3(leftCap, transform.position.y, 0),
                            new Vector3(rightCap, transform.position.y, 0));

            // Draw spheres at the patrol points
            Gizmos.DrawWireSphere(new Vector3(leftCap, transform.position.y, 0), 0.2f);
            Gizmos.DrawWireSphere(new Vector3(rightCap, transform.position.y, 0), 0.2f);
        }
    }
}