using GameRig.Scripts.Systems.InputSystem;
using GameRig.Scripts.Utilities.Attributes;
#if UNITY_STANDALONE
using UnityEngine;
using GameRig.Scripts.Utilities.GameRigConstantValues;
#endif

namespace GameRig.Scripts.Systems.CreativeSystem
{
	/// <summary>
	/// This class is used to store game changes required by Creative Team
	/// </summary>
	public static class CreativeManager
	{
#if UNITY_STANDALONE
		private static CreativeManagerBehaviour creativeManagerBehaviour;
		private static ScreenshotSettings[] screenshotsSettings;
#endif

		[InitializeOnLaunch(typeof(InputManager))]
		private static void Initialize()
		{
#if UNITY_STANDALONE
			creativeManagerBehaviour = GameRigCore.InitializeManagerBehaviour<CreativeManagerBehaviour>();

			screenshotsSettings = Resources.LoadAll<ScreenshotSettings>(GameRigResourcesPaths.ScreenshotsSettings);

			InputManager.Subscribe(KeyEventType.KeyDown, KeyCode.P, SaveScreenshots, false);
			InputManager.Subscribe(KeyEventType.KeyDown, KeyCode.R, ResetTheResolution, false);
#endif
		}

#if UNITY_STANDALONE
		private static void SaveScreenshots()
		{
			if (Application.platform != RuntimePlatform.WindowsEditor)
			{
				creativeManagerBehaviour.SaveScreenshots(screenshotsSettings);
			}
		}

		private static void ResetTheResolution()
		{
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
		}
#endif
	}
}