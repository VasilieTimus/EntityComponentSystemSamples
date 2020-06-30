using System.Collections.Generic;
using System.Linq;
using GameRig.Scripts.Systems.PubSubSystem;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;

namespace GameRig.Scripts.Systems.RankingSystem
{
	/// <summary>
	/// This class handles the ranking system, automatically saves/loads ranks settings, trigger rank up events and much more
	/// </summary>
	public static class RankingManager
	{
		private static List<RankSettings> ranks;

		private static int currentRankPoints;
		private static int currentRankIndex;

		private static bool IsRankIncrement => currentRankIndex < ranks.Count - 1 && currentRankPoints >= ranks[currentRankIndex].TargetPoints;

		/// <summary>
		/// Gets the current rank settings
		/// </summary>
		public static RankSettings CurrentRankSettings { get; private set; }

		/// <summary>
		/// Gets or sets current rank points that can lead to a rank increment
		/// </summary>
		/// <remarks>
		/// If added rank points are enough to increment more than one rank then the rank up event is triggered for each increment
		/// </remarks>
		public static int CurrentRankPoints
		{
			get => currentRankPoints;
			set
			{
				if (value < currentRankPoints)
				{
					return;
				}

				if (currentRankPoints != value)
				{
					SaveManager.Save(GameRigSaveKeys.CurrentRankPoints, value);
				}

				currentRankPoints = value;

				while (IsRankIncrement)
				{
					CurrentRankIndex++;

					CurrentRankSettings = ranks[CurrentRankIndex];

					OnRankUp(CurrentRankSettings);

					CurrentRankPoints = currentRankPoints;
				}
			}
		}

		public delegate void RankUpDelegate(RankSettings rankSettings);

		public static RankUpDelegate OnRankUp = delegate { };

		/// <summary>
		/// Gets the current rank index
		/// </summary>
		public static int CurrentRankIndex
		{
			get => currentRankIndex;
			private set
			{
				currentRankIndex = value;

				SaveManager.Save(GameRigSaveKeys.CurrentRankIndex, currentRankIndex);
			}
		}

		/// <summary>
		/// Gets the available ranks amount
		/// </summary>
		public static int RanksCount => ranks.Count;

		/// <summary>
		/// Gets the total ranks completion rate which is current rank index divided by total ranks count 
		/// </summary>
		public static float RanksTotalCompletionRate => CurrentRankIndex / (float) RanksCount;

		/// <summary>
		/// Gets the current rank completion rate
		/// </summary>
		public static float CurrentRankCompletionRate =>
			currentRankIndex >= ranks.Count - 1 ? 1f :
			currentRankIndex == 0 ? currentRankPoints / (float) CurrentRankSettings.TargetPoints :
			(currentRankPoints - ranks[currentRankIndex - 1].TargetPoints) /
			(float) (CurrentRankSettings.TargetPoints - ranks[currentRankIndex - 1].TargetPoints);

		/// <summary>
		/// Gets the info on whatever the current rank is the last rank
		/// </summary>
		public static bool CurrentRankIsLastRank => currentRankIndex >= ranks.Count - 1;

		[InitializeOnLaunch(typeof(PubSubManager))]
		private static void Initialize()
		{
			ranks = Resources.LoadAll<RankSettings>(GameRigResourcesPaths.Ranks).ToList();

			CurrentRankPoints = SaveManager.Load(GameRigSaveKeys.CurrentRankPoints, 0);
			CurrentRankIndex = SaveManager.Load(GameRigSaveKeys.CurrentRankIndex, 0);

			CurrentRankSettings = ranks[CurrentRankIndex];
		}

		/// <summary>
		/// Returns rank settings for given rank index
		/// </summary>
		/// <param name="index">Rank index</param>
		/// <returns>Rank settings</returns>
		public static RankSettings GetRankSettingsByIndex(int index)
		{
			return ranks[Mathf.Clamp(index, 0, ranks.Count - 1)];
		}
	}
}