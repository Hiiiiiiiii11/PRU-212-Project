using System;
using UnityEngine;
namespace level3
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] musicSounds, sfxSounds;
        public AudioSource musicSource, sfxSource;

        public void PlayMusic()
        {
            Sound s = Array.Find(musicSounds, x => x.name == name);
            if (s == null)
                Debug.Log("Sound Not Found");
            else
            {
                musicSource.clip = s.clip;
                musicSource.Play();
            }
        }

        public void PlaySFX(String name)
        {
            Sound s = Array.Find(sfxSounds, x => x.name == name);
            if (s == null)
                Debug.Log("Sound Not Found");
            else
                sfxSource.PlayOneShot(s.clip);
        }
    }

}
