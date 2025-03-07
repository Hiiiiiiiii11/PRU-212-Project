using UnityEngine;
namespace level5
{
    public class TouchingDirection : MonoBehaviour
    {
        public ContactFilter2D castFilter;
        public float groundDistance = 0.05f;
        public float wallDistance = 0.02f;
        public float cellingDistance = 0.05f;

        CapsuleCollider2D touchingCol;
        Animator animator;

        RaycastHit2D[] groundHits = new RaycastHit2D[5];
        RaycastHit2D[] wallHits = new RaycastHit2D[5];
        RaycastHit2D[] cellingHits = new RaycastHit2D[5];


        [SerializeField]
        private bool _isGrounded;

        public bool IsGrounded
        {
            get
            {
                return _isGrounded;
            }
            private set
            {
                _isGrounded = value;
                animator.SetBool(AnimationStrings.isGrounded, value);
            }
        }

        [SerializeField]
        private bool _isOnWall;

        public bool IsOnWall
        {
            get
            {
                return _isOnWall;
            }
            private set
            {
                _isOnWall = value;
                animator.SetBool(AnimationStrings.isOnWall, value);
            }
        }

        [SerializeField]
        private bool _isOnCelling;
        private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        public bool IsOnCelling
        {
            get
            {
                return _isOnCelling;
            }
            private set
            {
                _isOnCelling = value;
                animator.SetBool(AnimationStrings.isOnCelling, value);
            }
        }

        private void Awake()
        {
            touchingCol = GetComponent<CapsuleCollider2D>();
            animator = GetComponent<Animator>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        private void FixedUpdate()
        {
            IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
            IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
            IsOnCelling = touchingCol.Cast(Vector2.up, castFilter, cellingHits, cellingDistance) > 0;
        }
    }
}