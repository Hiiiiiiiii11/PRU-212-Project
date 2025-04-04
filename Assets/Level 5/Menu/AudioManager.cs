using System;
using UnityEngine;
namespace level5
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public Sound[] musicSound, sfxSound;
        public AudioSource musicSource, sfxSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            PlayMusic("Theme");
        }

        public void PlayMusic(string name)
        {
            Sound s = Array.Find(musicSound, x => x.name == name);
            if (s == null)
            {
                Debug.Log("Sound not found");
            }
            else
            {
                musicSource.clip = s.clip;
                musicSource.Play();
            }
        }

        public void PlaySFX(string name)
        {
            Sound s = Array.Find(sfxSound, x => x.name == name);
            if (s == null)
            {
                Debug.Log("Sound not found");
            }
            else
            {
                sfxSource.PlayOneShot(s.clip);
            }
        }

        public void ToggleMusic()
        {
            musicSource.mute = !musicSource.mute;
        }

        public void ToggleSFX()
        {
            sfxSource.mute = !sfxSource.mute;
        }

        public void MusicVolume(float volume)
        {
            musicSource.volume = volume;
        }

        public void SFXVolume(float volume)
        {
            sfxSource.volume = volume;
        }
    }
}
