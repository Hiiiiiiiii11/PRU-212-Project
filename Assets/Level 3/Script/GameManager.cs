using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace level3
{
    public class GameManager : MonoBehaviour
    {
        private int score = 0;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject gameWinUI;
        [SerializeField] private float gameOverDelay = 1f;
        private bool isGameOver = false;
        private bool isGameWin = false;
        void Start()
        {
            UpdateScore();
            gameOverUI.SetActive(false);
            gameWinUI.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddScore(int point)
        {
            score += point;
            UpdateScore();
        }

        private void UpdateScore()
        {
            scoreText.text = $"SCORE: {score.ToString("D3")}";
        }

        public void GameOver()
        {
            isGameOver = true;
            score = 0;
            Time.timeScale = 1; // Keep time scale normal during the delay
            StartCoroutine(ShowGameOverAfterDelay());
        }

        public void GameWin()
        {
            isGameWin = true;
            score = 0;
            Time.timeScale = 0;
            gameWinUI.SetActive(true);
        }

        public void RestartGame()
        {
            isGameOver = false;
            isGameWin = false;
            score = 0;
            UpdateScore();
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        }
    }
}