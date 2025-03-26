using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace level5
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] public GameObject pauseMenu;

        public void Pause()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        public void Home()
        {
            SceneManager.LoadScene("Main Menu");
            Time.timeScale = 1;
        }

        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
    }
}