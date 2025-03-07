using System.Collections;
using TMPro;
using UnityEngine;

namespace level4
{
    public class HealthText : MonoBehaviour
	{
		public Vector3 moveSpeed = new Vector3(0, 75, 0);
		public float timeToFade = 1f;
		private float timeElapsed = 0f;

		RectTransform textTransform;
		TextMeshProUGUI textMeshPro;
		private Color startColor;

		private void Awake()
		{
			textTransform = GetComponent<RectTransform>();
			textMeshPro = GetComponent<TextMeshProUGUI>();
			startColor = textMeshPro.color;
		}

		private void Update()
		{
			textTransform.position += moveSpeed * Time.deltaTime;

			timeElapsed += Time.deltaTime;

			if (timeElapsed < timeToFade)
			{
				float fadeAlpha = startColor.a * (1 - (timeElapsed / timeToFade));
				textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}