using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class Chest : MonoBehaviour
	{
		public UIManager uIManager;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				uIManager.GameWin();
			}
		}
	}
}