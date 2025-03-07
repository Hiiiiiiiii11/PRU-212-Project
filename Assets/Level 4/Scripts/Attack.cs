using UnityEngine;
namespace level4
{
	public class Attack : MonoBehaviour
	{
		public int attackDamage = 10;
		public Vector2 knockback = Vector2.zero;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			Damageable dameable = collision.GetComponent<Damageable>();

			if (dameable != null)
			{
				Vector2 deliveredKnockBack = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
				dameable.Hit(attackDamage, deliveredKnockBack);
			}
		}
	}
}