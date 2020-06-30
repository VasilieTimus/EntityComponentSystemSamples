using System.Collections;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;

namespace GameRig.Scripts.Systems.AudioSystem
{
	/// <summary>
	/// This class handles AudioSources for AudioManager
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class AudioManagerBehaviour : MonoBehaviour
	{
		private const float DefaultMusicVolume = 0.25f;

		private AudioSource[] musicAudioSources;
		private AudioSource thisAudioSource;
		private int activeMusicSourceIndex;
		private bool isMuted;

		public bool IsMuted
		{
			get => isMuted;
			set
			{
				isMuted = value;

				AudioManager.OnAudioStateChange();

				if (isMuted)
				{
					foreach (AudioSource musicAudioSource in musicAudioSources)
					{
						musicAudioSource.volume = 0f;
					}

					PauseMusic();
				}
				else
				{
					foreach (AudioSource musicAudioSource in musicAudioSources)
					{
						musicAudioSource.volume = DefaultMusicVolume;
					}
				}

				ResumeMusic();

				SaveManager.Save(GameRigSaveKeys.IsMuted, isMuted);
			}
		}

		private void Awake()
		{
			SetupReferences();
		}

		public void PlayOneShot(AudioClip audioClip, float volumeScale)
		{
			if (IsMuted || audioClip == null)
			{
				return;
			}

			thisAudioSource.PlayOneShot(audioClip, volumeScale);
		}

		public void PlayMusic(AudioClip clip, float fadeDuration)
		{
			if (clip == null)
			{
				return;
			}

			activeMusicSourceIndex = 1 - activeMusicSourceIndex;
			musicAudioSources[activeMusicSourceIndex].clip = clip;
			musicAudioSources[activeMusicSourceIndex].Play();

			StartCoroutine(AnimateMusicCrossFade(fadeDuration));
		}

		public void PauseMusic()
		{
			musicAudioSources[activeMusicSourceIndex].Pause();
		}

		public void ResumeMusic()
		{
			musicAudioSources[activeMusicSourceIndex].UnPause();
		}

		private void SetupReferences()
		{
			thisAudioSource = GetComponent<AudioSource>();

			Transform musicSourcesHolder = new GameObject("Music Audio Sources").transform;
			musicSourcesHolder.SetParent(transform);

			const int audioSourcesCount = 2;

			musicAudioSources = new AudioSource[audioSourcesCount];

			for (int audioSourceIndex = 0; audioSourceIndex < audioSourcesCount; audioSourceIndex++)
			{
				GameObject newAudioSourceGameObject = new GameObject("Music Audio Source " + (audioSourceIndex + 1));
				newAudioSourceGameObject.transform.SetParent(musicSourcesHolder);

				AudioSource newAudioSource = newAudioSourceGameObject.AddComponent<AudioSource>();
				newAudioSource.playOnAwake = false;
				newAudioSource.loop = true;

				musicAudioSources[audioSourceIndex] = newAudioSource;
			}

			IsMuted = SaveManager.Load(GameRigSaveKeys.IsMuted, false);
		}

		private IEnumerator AnimateMusicCrossFade(float duration)
		{
			float percent = 0;

			musicAudioSources[activeMusicSourceIndex].volume = 0f;
			musicAudioSources[activeMusicSourceIndex].Play();

			float musicVolume = isMuted ? 0f : DefaultMusicVolume;

			while (percent < 1f)
			{
				percent += Time.deltaTime * 1 / duration;
				musicAudioSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolume, percent);
				musicAudioSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolume, 0, percent);

				yield return null;
			}

			musicAudioSources[1 - activeMusicSourceIndex].Stop();
		}
	}
}