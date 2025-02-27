using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class MusicPlayer : MonoBehaviour
	{
		public AudioSource introSource, loopSource;

		void Start()
		{
			introSource.Play();
			loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
		}
	}
}