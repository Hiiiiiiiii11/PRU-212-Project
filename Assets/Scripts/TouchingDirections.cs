using Assets.Scripts;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
	public LayerMask layerMask;
	public ContactFilter2D castFilter;
	public float groundDistance = 0.05f;
	public float wallDistance = 0.2f;

	CapsuleCollider2D touchingCol;
	Animator animator;

	RaycastHit2D[] groundHits = new RaycastHit2D[5];
	RaycastHit2D[] wallHits = new RaycastHit2D[5];

	[SerializeField]
	private bool _isGrounded;
	public bool IsGrounded
	{ 
		get => _isGrounded;
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
		get => _isOnWall;
		private set
		{
			_isOnWall = value;
			animator.SetBool(AnimationStrings.isOnWall, value);
		}
	}

	private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

	private void Awake()
	{
		touchingCol = GetComponent<CapsuleCollider2D>();
		animator = GetComponent<Animator>();

		castFilter = new ContactFilter2D();
		castFilter.SetLayerMask(layerMask);
		castFilter.useTriggers = false;
	}
	void FixedUpdate()
	{
		IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
		IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
	}
}
