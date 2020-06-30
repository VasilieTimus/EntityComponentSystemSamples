using GameRig.Scripts.Systems.PubSubSystem;
using GameRig.Scripts.Utilities.Attributes;
using UnityEngine;

namespace GameRig.Scripts.Systems.AudioSystem
{
	/// <summary>
	/// This class handles some audio features 
	/// </summary>
	public static class AudioManager
	{
		private static AudioManagerBehaviour audioManagerBehaviour;

		/// <summary>
		/// The sound current mute state
		/// </summary>
		public static bool IsMuted => audioManagerBehaviour.IsMuted;

		public delegate void AudioStateChangeDelegate();

		public static AudioStateChangeDelegate OnAudioStateChange = delegate { };

		[InitializeOnLaunch(typeof(PubSubManager))]
		private static void Initialize()
		{
			audioManagerBehaviour = GameRigCore.InitializeManagerBehaviour<AudioManagerBehaviour>();
		}

		/// <summary>
		/// Mutes audio sources
		/// </summary>
		public static void Mute()
		{
			audioManagerBehaviour.IsMuted = true;
		}

		/// <summary>
		/// Unmutes audio sources 
		/// </summary>
		public static void UnMute()
		{
			audioManagerBehaviour.IsMuted = false;
		}

		/// <summary>
		/// Plays SFX Audio Clip with given volume scale
		/// </summary>
		/// <param name="audioClip"> Audio Clip to play</param>
		/// <param name="volume">Used to override the default volume</param>
		public static void PlaySfx(AudioClip audioClip, float volume = 1f)
		{
			audioManagerBehaviour.PlayOneShot(audioClip, volume);
		}

		/// <summary>
		/// Plays music Audio Clip with given fade duration
		/// </summary>
		/// <param name="clip">Music to play</param>
		/// <param name="fadeDuration">Fade duration in seconds between the previous and current music track</param>
		public static void PlayMusic(AudioClip clip, float fadeDuration = 1)
		{
			audioManagerBehaviour.PlayMusic(clip, fadeDuration);
		}

		/// <summary>
		/// Pauses music audio sources.
		/// </summary>
		/// <example>It is a good practice to pause music when Ads are displayed</example>
		public static void PauseMusic()
		{
			audioManagerBehaviour.PauseMusic();
		}

		/// <summary>
		/// Resumes music audio sources
		/// </summary>
		public static void ResumeMusic()
		{
			audioManagerBehaviour.ResumeMusic();
		}
	}
}