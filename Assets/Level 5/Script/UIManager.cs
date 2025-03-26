using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
namespace level5
{
    public class UIManager : MonoBehaviour
    {
        public GameObject dammageTextPrefab;
        public GameObject healthTextPrefab;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject gameWinUI;
        [SerializeField] private float gameOverDelay = 1f;
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

        public void GameOver()
        {
            Time.timeScale = 1; // Keep time scale normal during the delay
            StartCoroutine(ShowGameOverAfterDelay());
        }

        public void GameWin()
        {
            Time.timeScale = 0;
            gameWinUI.SetActive(true);
        }

        public void RestartGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void NextLevel()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Level 1");
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        private IEnumerator ShowGameOverAfterDelay()
        {
            yield return new WaitForSeconds(gameOverDelay);
            Time.timeScale = 0; // Stop the game when Game Over UI appears
            gameOverUI.SetActive(true);
            AudioManager.Instance.PlaySFX("Game Over");
        }
    }
}