using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
	public UnityEvent noCollidersRemain;
    public List<Collider2D> detectionCollitions = new List<Collider2D>();

	private void OnTriggerEnter2D(Collider2D collision)
	{
		detectionCollitions.Add(collision);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		detectionCollitions.Remove(collision);

		if (detectionCollitions.Count <= 0)
		{
			noCollidersRemain.Invoke();
		}
	}
}
