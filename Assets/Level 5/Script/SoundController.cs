using UnityEngine;
using UnityEngine.UI;
namespace level5
{
    public class SoundController : MonoBehaviour
    {
        public Slider _musicSlider, _sfxSlider;

        public void ToggleMusic()
        {
            AudioManager.Instance.ToggleMusic();
        }

        public void ToggleSFX()
        {
            AudioManager.Instance.ToggleSFX();
        }

        public void MusicVolume()
        {
            AudioManager.Instance.MusicVolume(_musicSlider.value);
        }

        public void SFXVolume()
        {
            AudioManager.Instance.SFXVolume(_sfxSlider.value);
        }
    }
}