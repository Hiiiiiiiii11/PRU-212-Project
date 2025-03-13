using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace level3
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;
        public Slider _musicSlider, _sfxSlider;

        private void Start()
        {
            if (PlayerPrefs.HasKey("musicVolume"))
                LoadVolume();
            else
                MusicVolume();
        }

        private void LoadVolume()
        {
            _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            _sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
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
            float volume = _musicSlider.value;
            PlayerPrefs.SetFloat("musicVolume", volume);
            AudioManager.instance.MusicVolume(volume);
        }

        public void SFXVolume()
        {
            float volume = _sfxSlider.value;
            PlayerPrefs.SetFloat("sfxVolume", volume);
            AudioManager.instance.SFXVolume(volume);
        }
    }
}