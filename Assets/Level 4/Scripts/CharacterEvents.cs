using UnityEngine;
using UnityEngine.Events;
namespace level4
{
	public class CharacterEvents
	{
		public static UnityAction<GameObject, int> characterDamaged;
		public static UnityAction<GameObject, int> characterHealed;
	}
}