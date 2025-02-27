using UnityEngine;

namespace Assets.Scripts
{
	public class DropChest : MonoBehaviour
	{
		public UIManager uIManager;
		private Damageable bossDamageable;

		private void Awake()
		{
			bossDamageable = GetComponent<Damageable>();
		}

		private void Update()
		{
			if (!bossDamageable.IsAlive) 
			{
				uIManager.ActiveChest();
			}
		}
	}
}