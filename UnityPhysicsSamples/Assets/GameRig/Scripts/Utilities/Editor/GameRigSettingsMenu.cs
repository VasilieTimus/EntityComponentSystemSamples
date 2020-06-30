using GameRig.Scripts.Systems.AdvertisingSystem;
using GameRig.Scripts.Systems.CreativeSystem;
using GameRig.Scripts.Systems.CurrencySystem;
using GameRig.Scripts.Systems.FadeGameSystem;
using GameRig.Scripts.Systems.OfflineSystem;
using GameRig.Scripts.Systems.PoolingSystem;
using GameRig.Scripts.Systems.RankingSystem;
using GameRig.Scripts.Systems.SocialSystem;
using GameRig.Scripts.Systems.StoreReviewSystem;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEditor;
using UnityEngine;

namespace GameRig.Scripts.Utilities.Editor
{
	/// <summary>
	/// This class is used to offer a top bar option with shortcuts to all GameRig resources 
	/// </summary>
	public static class GameRigSettingsMenu
	{
		[MenuItem("GameRig/Settings/Pooling Settings")]
		private static void OpenPoolingSettings()
		{
			SelectSettings<PoolingSettings>(GameRigResourcesPaths.PoolingSettings);
		}

		[MenuItem("GameRig/Settings/Offline Earnings Settings")]
		private static void OpenOfflineEarningsSettings()
		{
			SelectSettings<OfflineSettings>(GameRigResourcesPaths.OfflineSettings);
		}

		[MenuItem("GameRig/Settings/Ranking Settings Folder")]
		private static void OpenRankingSettings()
		{
			RankSettings[] rankSettings = Resources.LoadAll<RankSettings>(GameRigResourcesPaths.Ranks);

			if (rankSettings.Length > 0)
			{
				Selection.activeObject = rankSettings[0];
			}
		}

		[MenuItem("GameRig/Settings/Screenshots Settings Folder")]
		private static void OpenScreenshotsSettings()
		{
			ScreenshotSettings[] rankSettings = Resources.LoadAll<ScreenshotSettings>(GameRigResourcesPaths.ScreenshotsSettings);

			if (rankSettings.Length > 0)
			{
				Selection.activeObject = rankSettings[0];
			}
		}

		[MenuItem("GameRig/Settings/Social Settings")]
		private static void OpenSocialSettings()
		{
			SelectSettings<SocialSettings>(GameRigResourcesPaths.SocialSettings);
		}

		[MenuItem("GameRig/Settings/Store Review Settings")]
		private static void OpenStoreReviewSettings()
		{
			SelectSettings<StoreReviewSettings>(GameRigResourcesPaths.StoreReviewSettings);
		}

		[MenuItem("GameRig/Settings/Currency Display Settings")]
		private static void OpenCurrencyDisplaySettings()
		{
			SelectSettings<GameCurrencySettings>(GameRigResourcesPaths.GameCurrencySettings);
		}

		[MenuItem("GameRig/Settings/Advertising Settings")]
		private static void OpenAdvertisingSettings()
		{
			SelectSettings<AdvertisingSettings>(GameRigResourcesPaths.AdvertisingSettings);
		}

		[MenuItem("GameRig/Settings/Game Fade Settings")]
		private static void OpenGameFadeSettings()
		{
			SelectSettings<GameFadeSettings>(GameRigResourcesPaths.AdvertisingSettings);
		}

		public static void SelectSettings<T>(string path) where T : ScriptableObject
		{
			T settings = Resources.Load<T>(path);

			if (!settings)
			{
				settings = ScriptableObject.CreateInstance<T>();

				EditorUtility.SetDirty(settings);

				AssetDatabase.CreateAsset(settings, "Assets/GameRig/Resources/" + path + ".asset");
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			Selection.activeObject = settings;
		}
	}
}