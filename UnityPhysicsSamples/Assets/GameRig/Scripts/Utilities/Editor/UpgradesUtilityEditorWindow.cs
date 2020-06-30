using GameRig.Scripts.Systems.UpgradesSystem;
using GameRig.Scripts.Utilities.Progressions;
using UnityEditor;
using UnityEngine;

namespace GameRig.Scripts.Utilities.Editor
{
	public class UpgradeUtilityEditorWindow : EditorWindow
	{
		private ProgressionType progressionType;

		private string newWeaponName;
		private bool drawDefaultInspector;
		private Vector3 scrollPositionUpgradesList;
		private Vector3 scrollPositionUpgradeWindow;

		[MenuItem("GameRig/Tools/Upgrades Editor", priority = 111)]
		public static void OpenWindow()
		{
			GetWindow(typeof(UpgradeUtilityEditorWindow), false, "Upgrades Utility");
		}

		private void OnFocus()
		{
			UpgradesUtility.Initialize();
			UpgradesUtility.UpdateUpgradeSettingsList();
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(200), GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));
			DrawAddUpgradeButton();
			DrawUpgradesButtonList();
			EditorGUILayout.EndVertical();

			if (UpgradesUtility.SelectedSetting != null)
			{
				EditorGUILayout.BeginVertical();
				scrollPositionUpgradeWindow = EditorGUILayout.BeginScrollView(scrollPositionUpgradeWindow);

				if (drawDefaultInspector)
				{
					DrawDefaultSelectedUpgrade();
				}
				else
				{
					DrawCustomSelectedUpgrade();
				}

				EditorGUILayout.EndScrollView();

				DrawUpgradeBottomPanel();

				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.EndHorizontal();
		}

		private void DrawDefaultSelectedUpgrade()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Default View");
			drawDefaultInspector = GUILayout.Toggle(drawDefaultInspector, "Use Legacy", GUILayout.MaxWidth(90));
			EditorGUILayout.EndHorizontal();
			SerializedObject serializedProgression = new SerializedObject(UpgradesUtility.SelectedSetting);
			DrawDefaultInspector(serializedProgression);
		}

		private void DrawAddUpgradeButton()
		{
			EditorGUILayout.BeginVertical(GUILayout.MinHeight(70));
			newWeaponName = GUILayout.TextField(newWeaponName);

			if (GUILayout.Button("Add Upgrade", GUILayout.MinHeight(30)))
			{
				UpgradesUtility.CreateUpgradeSetting(newWeaponName);
			}

			EditorGUILayout.EndVertical();
		}

		private void DrawUpgradesButtonList()
		{
			scrollPositionUpgradesList = EditorGUILayout.BeginScrollView(scrollPositionUpgradesList);

			foreach (var upgradeSettings in UpgradesUtility.UpgradeSettingsList)
			{
				bool isSelected = upgradeSettings == UpgradesUtility.SelectedSetting;

				if (GUILayout.Toggle(isSelected, upgradeSettings.UpgradeName, "Button", GUILayout.MinHeight(isSelected ? 35 : 25)))
				{
					if (!isSelected)
					{
						UpgradesUtility.SelectedSetting = upgradeSettings;
					}
				}
			}

			EditorGUILayout.EndScrollView();
		}

		private void DrawDefaultInspector(SerializedObject serializedObject)
		{
			serializedObject.Update();

			SerializedProperty prop = serializedObject.GetIterator();
			if (prop.NextVisible(true))
			{
				prop.NextVisible(false);

				do
				{
					EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
				} while (prop.NextVisible(false));
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawCustomSelectedUpgrade()
		{
			SerializedObject serializedObject = new SerializedObject(UpgradesUtility.SelectedSetting);
			serializedObject.Update();

			DrawMainUpgradeSettings(serializedObject);
			DrawUpgradeValuesSettings();
			DrawUpgradePricesSettings();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawMainUpgradeSettings(SerializedObject serializedObject)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Main Settings");
			drawDefaultInspector = GUILayout.Toggle(drawDefaultInspector, "Use Legacy", GUILayout.MaxWidth(90));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.PropertyField(serializedObject.FindProperty("upgradeName"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("upgradeSaveKey"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxLevel"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("isLockedFromTheStart"));
			EditorGUILayout.EndVertical();
		}

		private void DrawUpgradeValuesSettings()
		{
			EditorGUILayout.LabelField("Value Settings");
			EditorGUILayout.BeginVertical("box");
			for (int i = 0; i < UpgradesUtility.SelectedSetting.UpgradeValueProgressions.Length; i++)
			{
				DrawValueSettings(UpgradesUtility.SelectedSetting.UpgradeValueProgressions[i], i);
			}

			if (GUILayout.Button("Add Value"))
			{
				UpgradesUtility.AddValueSettings(UpgradesUtility.SelectedSetting);
			}

			EditorGUILayout.EndVertical();
		}

		private void DrawUpgradePricesSettings()
		{
			EditorGUILayout.LabelField("Price Settings");
			EditorGUILayout.BeginVertical("box");
			DrawPriceSettings(UpgradesUtility.SelectedSetting.PriceSettings);
			EditorGUILayout.EndVertical();
		}

		private void DrawValueSettings(UpgradeValueSettings valueSettings, int index)
		{
			if (valueSettings == null)
				return;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(index.ToString());
			if (GUILayout.Button("X", GUILayout.MaxWidth(24)))
			{
				UpgradesUtility.RemoveValueSettings(valueSettings);
				UpgradesUtility.RemoveMissingValueSettings();
				return;
			}

			EditorGUILayout.EndHorizontal();

			SerializedObject serializedObject = new SerializedObject(valueSettings);
			serializedObject.Update();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("valueName"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("prefix"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("suffix"));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			var existingType = valueSettings.ValueProgression.GetType() == typeof(ArithmeticProgression) ? ProgressionType.Arithmetic : ProgressionType.Geometric;
			progressionType = existingType;

			SerializedObject serializedProgression = new SerializedObject(valueSettings.ValueProgression);
			serializedProgression.Update();
			DrawDefaultInspector(serializedProgression);

			serializedProgression.ApplyModifiedProperties();

			progressionType = (ProgressionType) EditorGUILayout.EnumPopup(progressionType);
			if (existingType != progressionType)
			{
				UpgradesUtility.ChangeProgressionType(valueSettings, progressionType);
			}

			EditorGUILayout.EndHorizontal();
			serializedObject.ApplyModifiedProperties();
		}

		private void DrawPriceSettings(UpgradePriceSettings priceSettings)
		{
			SerializedObject serializedObject = new SerializedObject(priceSettings);
			serializedObject.Update();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("pricePrefix"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("priceSuffix"));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			var existingType = priceSettings.PriceProgression.GetType() == typeof(ArithmeticProgression) ? ProgressionType.Arithmetic : ProgressionType.Geometric;
			progressionType = existingType;

			SerializedObject serializedProgression = new SerializedObject(priceSettings.PriceProgression);
			serializedProgression.Update();
			DrawDefaultInspector(serializedProgression);

			serializedProgression.ApplyModifiedProperties();

			progressionType = (ProgressionType) EditorGUILayout.EnumPopup(progressionType);
			if (existingType != progressionType)
			{
				UpgradesUtility.ChangeProgressionType(priceSettings, progressionType);
			}

			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawUpgradeBottomPanel()
		{
			if (GUILayout.Button("Delete", GUILayout.MinHeight(30)))
			{
				UpgradesUtility.DeleteSelectedUpgrade();
			}
		}
	}

	public enum ProgressionType
	{
		Arithmetic,
		Geometric
	}
}