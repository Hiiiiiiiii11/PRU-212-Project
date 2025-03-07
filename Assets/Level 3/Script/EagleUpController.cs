using UnityEngine;
namespace level3
{
    public class EagleUpController : Enemy
    {
        [SerializeField] private float highCap;
        [SerializeField] private float lowCap;
        [SerializeField] private float flySpeed = 3f;
        [SerializeField] private Color gizmoColor = Color.green;

        private Collider2D coll;
        private bool movingUp = true;

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
            if (movingUp)
            {
                if (transform.position.y < highCap) // rightCap now represents the upper boundary
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, flySpeed);
                }
                else movingUp = false;
            }
            else
            {
                if (transform.position.y > lowCap) // leftCap now represents the lower boundary
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, -flySpeed);
                }
                else movingUp = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            // Get the global positions for the top and bottom caps
            float xPos = transform.position.x;
            Vector3 highCapPos = new Vector3(xPos, highCap, 0);
            Vector3 lowCapPos = new Vector3(xPos, lowCap, 0);

            // Draw horizontal lines at the top and bottom boundaries
            Gizmos.DrawLine(new Vector3(xPos - 0.5f, highCap, 0), new Vector3(xPos + 0.5f, highCap, 0));
            Gizmos.DrawLine(new Vector3(xPos - 0.5f, lowCap, 0), new Vector3(xPos + 0.5f, lowCap, 0));

            // Draw a line showing the vertical patrol path
            Gizmos.DrawLine(lowCapPos, highCapPos);

            // Draw spheres at the patrol points
            Gizmos.DrawWireSphere(highCapPos, 0.2f);
            Gizmos.DrawWireSphere(lowCapPos, 0.2f);
        }
    }
}
