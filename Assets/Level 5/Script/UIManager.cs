using TMPro;
using UnityEngine;
namespace level5
{
    public class UIManager : MonoBehaviour
    {
        public GameObject dammageTextPrefab;
        public GameObject healthTextPrefab;
        public Canvas gameCanvas;

        private void Awake()
        {
            gameCanvas = FindFirstObjectByType<Canvas>();
        }

        private void OnEnable()
        {
            CharacterEvents.characterDamaged += (CharacterTookDamage);
            CharacterEvents.characterHealed += (CharacterHealed);
        }

        private void OnDisable()
        {
            CharacterEvents.characterDamaged -= (CharacterTookDamage);
            CharacterEvents.characterHealed -= (CharacterHealed);
        }

        public void CharacterTookDamage(GameObject character, int damageReceived)
        {
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
            TMP_Text tmpText = Instantiate(dammageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
                .GetComponent<TMP_Text>();
            tmpText.text = damageReceived.ToString();
        }

        public void CharacterHealed(GameObject character, int healthRestored)
        {
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
            TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
                .GetComponent<TMP_Text>();
            tmpText.text = healthRestored.ToString();
        }
    }
}