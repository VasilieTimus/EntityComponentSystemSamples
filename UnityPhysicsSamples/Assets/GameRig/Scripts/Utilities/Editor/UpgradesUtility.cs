using System.IO;
using System.Linq;
using GameRig.Scripts.Systems.UpgradesSystem;
using GameRig.Scripts.Utilities.Progressions;
using SRF.Helpers;
using UnityEditor;
using UnityEngine;

namespace GameRig.Scripts.Utilities.Editor
{
	public static class UpgradesUtility
	{
		private static string upgradePath;
		private static string valuePath;
		private static string pricePath;
		private static string valueProgressionPath;
		private static string priceProgressionPath;

		private static bool isInitialized;
		private static UpgradeSettings selectedSetting;

		public static UpgradeSettings[] UpgradeSettingsList { get; private set; }

		public static UpgradeSettings SelectedSetting
		{
			get
			{
				if (selectedSetting == null)
				{
					if (UpgradeSettingsList.Length > 0)
						selectedSetting = UpgradeSettingsList[0];
				}

				return selectedSetting;
			}
			set => selectedSetting = value;
		}

		private static string GetProjectName()
		{
			string[] s = Application.dataPath.Split('/');
			string projectName = s[s.Length - 2];
			projectName = projectName.Replace('_', ' ');
			return projectName;
		}

		private static void PathsValidation()
		{
			string projectName = GetProjectName();

			upgradePath = "Assets/" + projectName + "/Resources/Upgrade Settings";
			valuePath = "Assets/" + projectName + "/Resources/Upgrade Settings/Value Settings";
			pricePath = "Assets/" + projectName + "/Resources/Upgrade Settings/Price Settings";
			valueProgressionPath = "Assets/" + projectName + "/Resources/Upgrade Settings/Value Progressions";
			priceProgressionPath = "Assets/" + projectName + "/Resources/Upgrade Settings/Price Progressions";
		}

		private static void FoldersValidation()
		{
			if (!Directory.Exists(upgradePath)) Directory.CreateDirectory(upgradePath);
			if (!Directory.Exists(valuePath)) Directory.CreateDirectory(valuePath);
			if (!Directory.Exists(pricePath)) Directory.CreateDirectory(pricePath);
			if (!Directory.Exists(valueProgressionPath)) Directory.CreateDirectory(valueProgressionPath);
			if (!Directory.Exists(priceProgressionPath)) Directory.CreateDirectory(priceProgressionPath);
		}

		private static bool NameValidation(string name)
		{
			return UpgradeSettingsList.All(upgrade => upgrade.UpgradeName != name);
		}

		public static void UpdateUpgradeSettingsList()
		{
			UpgradeSettingsList = Resources.LoadAll<UpgradeSettings>("Upgrade Settings");
		}

		public static void Initialize()
		{
			if (isInitialized)
				return;

			PathsValidation();

			isInitialized = true;
		}

		public static void CreateUpgradeSetting(string upgradeName)
		{
			if (string.IsNullOrEmpty(upgradeName))
			{
				Debug.Log("Enter upgrade name first!");
				return;
			}

			if (!NameValidation(upgradeName))
			{
				Debug.Log("Upgrade Settings with name " + upgradeName + " already exists!");
				return;
			}

			FoldersValidation();

			UpgradeSettings upgrade = AssetUtil.CreateAsset<UpgradeSettings>(upgradePath, "Upgrade " + upgradeName);
			UpgradeValueSettings value = AssetUtil.CreateAsset<UpgradeValueSettings>(valuePath, "Value " + upgradeName);
			UpgradePriceSettings price = AssetUtil.CreateAsset<UpgradePriceSettings>(pricePath, "Price " + upgradeName);
			ArithmeticProgression valueProgression = AssetUtil.CreateAsset<ArithmeticProgression>(valueProgressionPath, "Value Progression " + upgradeName);
			GeometricProgression priceProgression = AssetUtil.CreateAsset<GeometricProgression>(priceProgressionPath, "Price Progression " + upgradeName);

			EditorUtility.SetDirty(upgrade);
			EditorUtility.SetDirty(value);
			EditorUtility.SetDirty(price);
			EditorUtility.SetDirty(valueProgression);
			EditorUtility.SetDirty(priceProgression);

			upgrade.UpgradeName = upgradeName;
			upgrade.UpgradeValueProgressions = new[] {value};
			upgrade.PriceSettings = price;
			upgrade.UpgradeSaveKey = upgradeName.GetHashCode().ToString("X");

			value.ValueProgression = valueProgression;
			price.PriceProgression = priceProgression;

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			UpdateUpgradeSettingsList();
		}

		public static void DeleteSelectedUpgrade()
		{
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(SelectedSetting.PriceSettings.PriceProgression));
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(SelectedSetting.PriceSettings));
			foreach (var selectedSettingUpgradeValueProgression in SelectedSetting.UpgradeValueProgressions)
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedSettingUpgradeValueProgression.ValueProgression));
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedSettingUpgradeValueProgression));
			}

			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(SelectedSetting));

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			UpdateUpgradeSettingsList();
		}

		public static void ChangeProgressionType(UpgradeValueSettings upgradeValueSettings, ProgressionType progressionType)
		{
			string previousName = upgradeValueSettings.ValueProgression.name;
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(upgradeValueSettings.ValueProgression));

			BaseProgression valueProgression;
			if (progressionType == ProgressionType.Arithmetic)
				valueProgression = AssetUtil.CreateAsset<ArithmeticProgression>(valueProgressionPath, previousName);
			else
				valueProgression = AssetUtil.CreateAsset<GeometricProgression>(valueProgressionPath, previousName);

			EditorUtility.SetDirty(upgradeValueSettings);

			upgradeValueSettings.ValueProgression = valueProgression;

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static void ChangeProgressionType(UpgradePriceSettings upgradePriceSettings, ProgressionType progressionType)
		{
			string previousName = upgradePriceSettings.PriceProgression.name;

			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(upgradePriceSettings.PriceProgression));
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			BaseProgression valueProgression;
			if (progressionType == ProgressionType.Arithmetic)
				valueProgression = AssetUtil.CreateAsset<ArithmeticProgression>(valueProgressionPath, previousName);
			else
				valueProgression = AssetUtil.CreateAsset<GeometricProgression>(valueProgressionPath, previousName);

			EditorUtility.SetDirty(upgradePriceSettings);

			upgradePriceSettings.PriceProgression = valueProgression;

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static void AddValueSettings(UpgradeSettings upgradeSettings)
		{
			EditorUtility.SetDirty(upgradeSettings);

			ArithmeticProgression valueProgression = AssetUtil.CreateAsset<ArithmeticProgression>
				(valueProgressionPath, "Value Progression " + upgradeSettings.UpgradeName);

			UpgradeValueSettings value = AssetUtil.CreateAsset<UpgradeValueSettings>
				(valuePath, "Value " + upgradeSettings.UpgradeName);

			EditorUtility.SetDirty(value);
			EditorUtility.SetDirty(upgradeSettings);

			value.ValueProgression = valueProgression;

			var valueSettingsList = upgradeSettings.UpgradeValueProgressions.ToList();
			valueSettingsList.Add(value);
			upgradeSettings.UpgradeValueProgressions = valueSettingsList.ToArray();

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static void RemoveMissingValueSettings()
		{
			var valueSettingsList = SelectedSetting.UpgradeValueProgressions.Where(x => x != null);

			SelectedSetting.UpgradeValueProgressions = valueSettingsList.ToArray();
		}

		public static void RemoveValueSettings(UpgradeValueSettings upgradeValueSettings)
		{
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(upgradeValueSettings.ValueProgression));
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(upgradeValueSettings));

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}