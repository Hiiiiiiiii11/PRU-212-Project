using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace level1
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;
        public Slider _musicSlider;
        public AudioSource musicSource;
        public Slider _sfxSlider;
        public AudioSource sfxSource;

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
            musicSource.mute = !musicSource.mute;
        }

        public void MusicVolume()
        {
            float volume = _musicSlider.value;
            PlayerPrefs.SetFloat("musicVolume", volume);
            musicSource.volume = volume;
        }

        public void ToggleSFX()
        {
            sfxSource.mute = !sfxSource.mute;
        }

        public void SFXVolume()
        {
            float volume = _sfxSlider.value;
            PlayerPrefs.SetFloat("sfxVolume", volume);
            sfxSource.volume = volume;
        }
    }
}