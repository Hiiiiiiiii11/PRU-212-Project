using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace level2
{
    public class GameManager : MonoBehaviour
    {
        private int keys = 0;
        private int fragment = 0;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject gameOverUI;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gameOverUI.SetActive(false);
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
            fragment++;
            UpdateScore();
        }

        public bool CanOpenChest()
        {
            return keys - 1 >= 0;
        }
        public bool CanOpenGate()
        {
            return fragment - 1 >= 0;
        }
        public void UpdateScore()
        {
            scoreText.text = "Keys: " + keys.ToString() + "\n" + "Fragments: " + fragment.ToString();
        }

        public void GameOver()
        {
            keys = fragment = 0;
            gameOverUI.SetActive(true);
        }

        public void RestartGame()
        {
            UpdateScore();
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}