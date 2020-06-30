using GameRig.Scripts.Systems.AudioSystem;
using UnityEngine;
using UnityEngine.UI;

namespace GameRig.Scripts.UI
{
	/// <summary>
	/// This class handles the automatic audio trigger for attached button
	/// </summary>
	[RequireComponent(typeof(Button))]
	public class ButtonAudioController : MonoBehaviour
	{
		[SerializeField] [Tooltip("Audio clip to play on button click")]
		private AudioClip sfx;

		private Button thisButton;

		private void Awake()
		{
			thisButton = GetComponent<Button>();

			if (thisButton != null)
			{
				thisButton.onClick.AddListener(PlaySfx);
			}
		}

		public void SetAudioClip(AudioClip audioClip)
		{
			sfx = audioClip;
		}

		private void PlaySfx()
		{
			AudioManager.PlaySfx(sfx);
		}
	}
}