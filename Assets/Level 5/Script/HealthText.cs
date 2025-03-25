using TMPro;
using UnityEngine;
namespace level5
{
    public class HealthText : MonoBehaviour
    {
        public Vector3 moveSpeed = new Vector3(0, 100, 0);
        public float timeToFade = 1f;
        RectTransform textTransform;
        TextMeshProUGUI textMeshPro;
        private float timeElasped = 0f;
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
            timeElasped += Time.deltaTime;
            if (timeElasped < timeToFade)
            {
                float fadeAlpha = startColor.a * (1 - (timeElasped / timeToFade));
                textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}