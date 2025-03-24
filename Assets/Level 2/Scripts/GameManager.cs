using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.SocialPlatforms.Impl;

namespace level2
{
    public class GameManager : MonoBehaviour
    {
        public int keys = 0;
        public int crystal = 0;
        public AudioClip gameOverSound;
        public AudioSource audioSource;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject gameWinUI;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gameOverUI.SetActive(false);
            gameWinUI.SetActive(false);
            audioSource = GetComponent<AudioSource>();
            UpdateScore();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void AddKey()
        {
            keys++;
            UpdateScore();
        }
        public void ReduceKey()
        {
            keys--;
            UpdateScore();
        }
        public void AddFragment()
        {
            crystal++;
            UpdateScore();
        }

        public bool CanOpenChest()
        {
            return keys - 1 >= 0;
        }
        public bool CanOpenGate()
        {
            return crystal - 1 >= 0;
        }
        public void UpdateScore()
        {
            scoreText.text = "Keys: " + keys.ToString() + "\n" + "Crytals: " + crystal.ToString();
        }

        public void GameOver()
        {
            keys = crystal = 0;
            gameOverUI.SetActive(true);
            if (gameOverSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(gameOverSound);
            }
        }

        public void GameWin()
        {
            keys = crystal = 0;
            Time.timeScale = 0;
            gameWinUI.SetActive(true);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void RestartGame()
        {
            UpdateScore();
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void NextLevel()
        {
            keys = crystal = 0;
            UpdateScore();
            Time.timeScale = 1;
            SceneManager.LoadScene("Level 3");
        }
    }
}