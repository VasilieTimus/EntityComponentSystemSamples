using System;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameRig.Scripts.Systems.UpgradesSystem
{
	/// <summary>
	/// These settings are used to define an upgrade
	/// </summary>
	[CreateAssetMenu(menuName = "GameRig/Upgrades System/Upgrade Settings")]
	public class UpgradeSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("The key used to save this Upgrade")]
		private string upgradeSaveKey;

		[SerializeField] [Tooltip("The name of this upgrade")]
		private string upgradeName;

		[SerializeField] [Tooltip("Maximum level this upgrade can be incremented to, 0 means no level cap")]
		private int maxLevel;

		[SerializeField] [Expandable] [Tooltip("Value progressions related to this upgrade")]
		private UpgradeValueSettings[] valueProgressions;

		[SerializeField] [Expandable] [Tooltip("Price settings related to this upgrade")]
		private UpgradePriceSettings priceSettings;

		[SerializeField] [Tooltip("Is the upgrade locked by default")]
		private bool isLockedFromTheStart;

		private bool isLocked;

		/// <summary>
		/// Gets the current upgrade level
		/// </summary>
		public int CurrentLevel { get; private set; }

		/// <summary>
		/// Gets the name of this upgrade
		/// </summary>
		public string UpgradeName
		{
			get => upgradeName;
			set => upgradeName = value;
		}

		/// <summary>
		/// Gets the upgrade key
		/// </summary>
		public string UpgradeSaveKey
		{
			get => upgradeSaveKey;
			set => upgradeSaveKey = value;
		}

		/// <summary>
		/// Gets the current upgrade values settings
		/// </summary>
		public UpgradeValueSettings[] UpgradeValueProgressions
		{
			get => valueProgressions;
			set => valueProgressions = value;
		}

		/// <summary>
		/// Gets the price prefix
		/// </summary>
		public string PricePrefix => PriceSettings.PricePrefix;

		/// <summary>
		/// Gets the price suffix
		/// </summary>
		public string PriceSuffix => PriceSettings.PriceSuffix;

		/// <summary>
		/// Gets the current upgrade price
		/// </summary>
		public float UpgradePrice => PriceSettings.PriceProgression.Evaluate(CurrentLevel);

		/// <summary>
		/// Used to check if this Upgrade is maxed out
		/// </summary>
		public bool IsMaxed => maxLevel != 0 && CurrentLevel >= maxLevel - 1;

		/// <summary>
		/// Used to lock the upgrade until a certain action is performed
		/// </summary>
		public bool IsLocked
		{
			get => isLocked;
			set
			{
				isLocked = value;

				SaveManager.Save(GameRigSaveKeys.UpgradeLockedStatePrefix + UpgradeSaveKey, isLocked);
			}
		}

		public UpgradePriceSettings PriceSettings
		{
			get => priceSettings;
			set => priceSettings = value;
		}

		/// <summary>
		/// Action invoked when an upgrade increment occurs
		/// </summary>
		public Action onUpgradeIncrement = delegate { };

		/// <summary>
		/// Initializes the upgrade, automatically called by the <see cref="UpgradesManager"/>
		/// </summary>
		public void InitializeUpgrade()
		{
			CurrentLevel = SaveManager.Load(GameRigSaveKeys.UpgradePrefix + UpgradeSaveKey, 0);
			isLocked = SaveManager.Load(GameRigSaveKeys.UpgradeLockedStatePrefix + UpgradeSaveKey, isLockedFromTheStart);

			foreach (UpgradeValueSettings progression in valueProgressions)
			{
				progression.Initialize(this);
			}

			SceneManager.sceneUnloaded += OnSceneUnload;
		}

		/// <summary>
		/// Gets the purchasable state of the Upgrade
		/// </summary>
		public bool IsPurchasable(float currencyAmount)
		{
			return !IsMaxed && !IsLocked && UpgradePrice <= currencyAmount;
		}

		/// <summary>
		/// Increments the upgrade to the next level
		/// </summary>
		public void IncrementUpgrade()
		{
			if (IsMaxed)
			{
				return;
			}

			CurrentLevel++;

			SaveManager.Save(GameRigSaveKeys.UpgradePrefix + UpgradeSaveKey, CurrentLevel);

			onUpgradeIncrement();
		}

		/// <summary>
		/// Resets the upgrade to the first level
		/// </summary>
		public void ResetUpgrade()
		{
			CurrentLevel = 0;

			SaveManager.Save(GameRigSaveKeys.UpgradePrefix + UpgradeSaveKey, CurrentLevel);

			onUpgradeIncrement();
		}

		private void OnSceneUnload(Scene unloadedScene)
		{
			onUpgradeIncrement = delegate { };
		}
	}
}