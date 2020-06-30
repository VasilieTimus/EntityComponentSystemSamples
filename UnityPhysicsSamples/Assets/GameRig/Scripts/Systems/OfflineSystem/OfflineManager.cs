using System;
using GameRig.Scripts.Systems.PubSubSystem;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;

namespace GameRig.Scripts.Systems.OfflineSystem
{
	/// <summary>
	/// This class manages offline time calculation
	/// </summary>
	public static class OfflineManager
	{
		private static OfflineSettings settings;
		private static double lastTimeOnline;

		/// <summary>
		/// Gets the state of offline reward, if it can be collected or not
		/// </summary>
		public static bool IsOfflineThresholdPassed { get; private set; }

		/// <summary>
		/// Gets the offline time rate which can be a value from 0 to 1
		/// </summary>
		public static float OfflineTimeRate { get; private set; }

		/// <summary>
		/// Gets the offline time in seconds
		/// </summary>
		public static float OfflineTime { get; private set; }

		public delegate void GoToOfflineDelegate();

		public delegate void OfflineThresholdPassedDelegate();

		public delegate void ReturnToOnlineDelegate();

		public static GoToOfflineDelegate OnGoToOffline = delegate { };
		public static OfflineThresholdPassedDelegate OnOfflineThresholdPassed = delegate { };
		public static ReturnToOnlineDelegate OnReturnToOnline = delegate { };

		[InitializeOnLaunch(typeof(PubSubManager))]
		private static void Initialize()
		{
			settings = Resources.Load<OfflineSettings>(GameRigResourcesPaths.OfflineSettings);

			OfflineManagerBehaviour offlineEarningsManagerBehaviour = GameRigCore.InitializeManagerBehaviour<OfflineManagerBehaviour>();

			offlineEarningsManagerBehaviour.goToOfflineAction = ProcessGoToOfflineAction;
			offlineEarningsManagerBehaviour.returnToOnlineAction = CheckOfflineThreshold;

			CheckOfflineThreshold();
		}

		/// <summary>
		/// Resets the offline counter, this is required if you want to start the timer on next application pause/unfocus
		/// </summary>
		public static void ResetLastTimeOnline()
		{
			IsOfflineThresholdPassed = false;

			SaveManager.Delete(GameRigSaveKeys.LastTimeOnline);
		}

		private static void ProcessGoToOfflineAction()
		{
			double totalSeconds = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;

			SaveManager.Save(GameRigSaveKeys.LastTimeOnline, totalSeconds);

			OnGoToOffline();
		}

		private static void CheckOfflineThreshold()
		{
			double currentTime = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;

			lastTimeOnline = SaveManager.Load(GameRigSaveKeys.LastTimeOnline, currentTime);

			OfflineTime = (float) (currentTime - lastTimeOnline);

			if (OfflineTime >= settings.MinDeltaSeconds)
			{
				IsOfflineThresholdPassed = true;
			}

			OfflineTimeRate = Mathf.Clamp(OfflineTime, 0f, settings.MaxDeltaSeconds) / settings.MaxDeltaSeconds;

			OnReturnToOnline();

			if (settings.MaxDeltaSeconds > 0 && IsOfflineThresholdPassed)
			{
				OnOfflineThresholdPassed();
			}
		}
	}
}