using UnityEngine;
using UnityEngine.SceneManagement;

namespace level3
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadSceneAsync(1);
        }

        // Update is called once per frame
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}