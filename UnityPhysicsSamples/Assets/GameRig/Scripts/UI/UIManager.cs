using GameRig.Scripts.Systems.AudioSystem;
using GameRig.Scripts.Systems.GameStateSystem;
using UnityEngine;

namespace GameRig.Scripts.UI
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private GameObject[] turnedOffVibrationIcons;
		[SerializeField] private GameObject[] turnedOffSoundIcons;
		[SerializeField] private GameObject mainMenuScreen;
		[SerializeField] private GameObject hudScreen;
		[SerializeField] private GameObject levelCompleteScreen;
		[SerializeField] private GameObject levelFailedScreen;

		private void Awake()
		{
			AudioManager.OnAudioStateChange += HandleAudioStateChange;
			GameStateManager.OnGameStateChange += HandleGameStateChange;

			HandleAudioStateChange();
			HandleVibrationStateChange();
		}

		public void OnPlayButtonClick()
		{
			GameStateManager.CurrentState = GameState.Game;
		}

		public void OnToggleSoundButtonClick()
		{
			if (AudioManager.IsMuted)
			{
				AudioManager.UnMute();
			}
			else
			{
				AudioManager.Mute();
			}
		}

		public void OnToggleVibrationButtonClick()
		{
		}

		private void HandleGameStateChange(GameState gameState)
		{
			switch (gameState)
			{
				case GameState.Game:
					OnGameStart();

					break;

				case GameState.LevelComplete:
					OnLevelComplete();

					break;

				case GameState.LevelFailed:
					OnLevelFailed();

					break;
			}
		}

		private void HandleAudioStateChange()
		{
			foreach (GameObject turnedOffSoundIcon in turnedOffSoundIcons)
			{
				turnedOffSoundIcon.SetActive(AudioManager.IsMuted);
			}
		}

		private void HandleVibrationStateChange()
		{
			foreach (GameObject turnedOffVibrationIcon in turnedOffVibrationIcons)
			{
			}
		}

		private void OnGameStart()
		{
			mainMenuScreen.SetActive(false);
			hudScreen.SetActive(true);
		}

		private void OnLevelComplete()
		{
			hudScreen.SetActive(false);
			levelCompleteScreen.SetActive(true);
		}

		private void OnLevelFailed()
		{
			hudScreen.SetActive(false);
			levelFailedScreen.SetActive(true);
		}

		private void OnDestroy()
		{
			AudioManager.OnAudioStateChange -= HandleAudioStateChange;
			GameStateManager.OnGameStateChange -= HandleGameStateChange;
		}
	}
}