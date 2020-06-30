using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;
#if UNITY_IOS
using System;
using UnityEngine.iOS;
using System.Globalization;
using GameRig.Scripts.Systems.SaveSystem;

#endif

namespace GameRig.Scripts.Systems.StoreReviewSystem
{
	/// <summary>
	/// This class handles the store review logic, only working for iOS for now
	/// </summary>
	public static class StoreReviewManager
	{
		private static StoreReviewSettings settings;

		[InitializeOnLaunch]
		private static void Initialize()
		{
			LoadResources();
		}

		/// <summary>
		/// Opens the game store page link in default browser 
		/// </summary>
		public static void RequestStoreReviewFromLink()
		{
			Application.OpenURL(settings.AppStoreLink);
		}

		/// <summary>
		/// Displays a native popup with request to review the game.
		/// </summary>
		/// <remarks>
		/// Can be called anytime but the actual request will work only depending on <see cref="StoreReviewSettings"/>
		/// </remarks> 
		public static void RequestStoreReviewFromPopup()
		{
#if UNITY_IOS
			if (CanDisplayStoreReviewPopup())
			{
				bool isStoreReviewDisplayed = Device.RequestStoreReview();

				if (isStoreReviewDisplayed)
				{
					SaveManager.Save(GameRigSaveKeys.LastStoreReviewDisplayTime, DateTime.Now.ToString(CultureInfo.InvariantCulture));
				}
			}
#endif
		}

		private static void LoadResources()
		{
			settings = Resources.Load<StoreReviewSettings>(GameRigResourcesPaths.StoreReviewSettings);
		}

#if UNITY_IOS
		private static bool CanDisplayStoreReviewPopup()
		{
			if (settings.MinIntervalInHoursBetweenRequests == 0 && SaveManager.Exists(GameRigSaveKeys.LastStoreReviewDisplayTime))
			{
				DateTime lastStoreReviewRequestDateTime = DateTime.Parse(SaveManager.Load<string>(GameRigSaveKeys.LastStoreReviewDisplayTime, ""));

				TimeSpan timeDifference = DateTime.Now - lastStoreReviewRequestDateTime;

				return timeDifference.TotalHours > settings.MinIntervalInHoursBetweenRequests;
			}

			return true;
		}
#endif
	}
}