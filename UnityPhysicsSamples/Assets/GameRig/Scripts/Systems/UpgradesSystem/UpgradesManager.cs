using System.Collections.Generic;
using System.Linq;
using GameRig.Scripts.Utilities.Attributes;
using UnityEngine;

namespace GameRig.Scripts.Systems.UpgradesSystem
{
	/// <summary>
	/// This class manages some basic upgrade system functionality
	/// </summary>
	public static class UpgradesManager
	{
		/// <summary>
		/// Gets the list of available upgrades
		/// </summary>
		public static List<UpgradeSettings> Upgrades { get; private set; }

		[InitializeOnLaunch]
		private static void Initialize()
		{
			LoadResources();
			InitializeResources();
		}

		private static void LoadResources()
		{
			Upgrades = Resources.LoadAll<UpgradeSettings>("").ToList();
		}

		private static void InitializeResources()
		{
			List<string> duplicates = Upgrades.GroupBy(s => s.UpgradeSaveKey)
				.Where(g => g.Count() > 1)
				.Select(g => g.Key).ToList();

			if (duplicates.Count > 0)
			{
				Debug.LogError("There are duplicate upgrade keys, Upgrades System not initialized");
			}

			Upgrades.ForEach(upgrade => upgrade.InitializeUpgrade());
		}
	}
}