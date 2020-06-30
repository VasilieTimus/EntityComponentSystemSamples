using GameRig.Scripts.Systems.AudioSystem;
using UnityEngine;

namespace GameRig.Scripts.Utilities
{
	/// <summary>
	/// This class is used to create a sequence of sounds with changeable pitch
	/// </summary>
	public class CustomAudioPitchController : MonoBehaviour
	{
		[SerializeField] [Tooltip("Audio source used for this effect")]
		private AudioSource audioSource;

		[SerializeField] [Tooltip("Audio clip used for this effect")]
		private AudioClip audioClip;

		[SerializeField] [Tooltip("Volume used for this audio clip")]
		private float volume;

		[SerializeField] [Tooltip("Pitch used from the start")]
		private float startPitch;

		[SerializeField] [Tooltip("Minimum pitch value")]
		private float minPitch;

		[SerializeField] [Tooltip("Maximum pitch value")]
		private float maxPitch;

		[SerializeField] [Tooltip("Pitch change step on every play")]
		private float pitchIncrementStep;

		[SerializeField] [Tooltip("Pitch change speed on every frame")]
		private float pitchDecrementSpeed;

		private float currentPitch;

		/// <summary>
		/// Plays sound with current pitch
		/// </summary>
		public void PlayEffect()
		{
			audioSource.pitch = CurrentPitch;

			if (!AudioManager.IsMuted)
			{
				audioSource.PlayOneShot(audioClip, volume);
			}

			CurrentPitch += pitchIncrementStep;
		}

		private float CurrentPitch
		{
			get => currentPitch;
			set => currentPitch = Mathf.Clamp(value, minPitch, maxPitch);
		}

		private void Start()
		{
			CurrentPitch = startPitch;
		}

		private void Update()
		{
			CurrentPitch -= pitchDecrementSpeed * Time.deltaTime;
		}
	}
}