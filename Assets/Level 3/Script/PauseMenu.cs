using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace level3
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;
        public Slider _musicSlider, _sfxSlider;

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

        public void ToggleMusic()
        {
            AudioManager.instance.ToggleMusic();
        }

        public void ToggleSFX()
        {
            AudioManager.instance.ToggleSFX();
        }

        public void MusicVolume()
        {
            AudioManager.instance.MusicVolume(_musicSlider.value);
        }

        public void SFXVolume()
        {
            AudioManager.instance.SFXVolume(_sfxSlider.value);
        }
    }
}