using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GameRig.Scripts.Utilities.Editor
{
	/// <summary>
	/// Scene switcher window, an editor window for switching between scenes.
	/// </summary>
	public class SceneSwitcherWindows : EditorWindow
	{
		internal enum ScenesSource
		{
			Assets,
			BuildSettings
		}

		internal struct SceneData
		{
			public string ScenePath; public string SceneName;
			public string AssetName;
		}

		internal class AssetButtonState
		{
			public AnimBool AnimBool;
			public bool isActive;
		}

		private Vector2 scrollPosition;
		private ScenesSource scenesSource = ScenesSource.Assets;
		private OpenSceneMode openSceneMode = OpenSceneMode.Single;

		private string[] scenesAssets;
		private HashSet<string> assetsNames;
		private List<string> assetsNamesList;
		private List<SceneData> sceneDataList;
		private HashSet<AssetButtonState> assetButtonStateHasSet;
		private Dictionary<string, List<SceneData>> sceneDataDictionary;

		[MenuItem("GameRig/Tools/Scene Switcher", priority = 111)]
		public static void Init()
		{
			SceneSwitcherWindows window = GetWindow<SceneSwitcherWindows>("Scene Switcher");
			window.minSize = new Vector2(250f, 200f);
			window.Show();
		}

		private void OnEnable()
		{
			scenesSource = (ScenesSource) EditorPrefs.GetInt("SceneSwitcher.scenesSource", (int) ScenesSource.Assets);
			openSceneMode = (OpenSceneMode) EditorPrefs.GetInt("SceneSwitcher.openSceneMode", (int) OpenSceneMode.Single);

			ResetData();

			LoadScenesData();
			SaveScenesFromAssets();
		}

		private void ResetData()
		{
			sceneDataDictionary = new Dictionary<string, List<SceneData>>();
			assetButtonStateHasSet = new HashSet<AssetButtonState>();
			assetsNames = new HashSet<string>();
			assetsNamesList = new List<string>();
			sceneDataList = new List<SceneData>();
		}

		private void OnDisable()
		{
			EditorPrefs.SetInt("SceneSwitcher.scenesSource", (int) scenesSource);
			EditorPrefs.SetInt("SceneSwitcher.openSceneMode", (int) openSceneMode);
		}

		private void OnGUI()
		{
			scenesSource = (ScenesSource) EditorGUILayout.EnumPopup("Scenes Source", scenesSource);
			openSceneMode = (OpenSceneMode) EditorGUILayout.EnumPopup("Open Scene Mode", openSceneMode);

			GUILayout.Space(5f);
			if (GUILayout.Button("Refresh", GUILayout.Width(150f)))
			{
				Refresh();
			}

			GUILayout.Label("Scenes", EditorStyles.boldLabel);

			float width = EditorGUIUtility.currentViewWidth;

			EditorGUILayout.BeginHorizontal();

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true, GUIStyle.none, "verticalscrollbar", GUIStyle.none);

			EditorGUILayout.BeginVertical(GUILayout.MaxWidth(width / 1.06f));

			DrawAssetsButtons(assetsNamesList);

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndScrollView();

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(20f);
			if (GUILayout.Button("Create New Scene"))
			{
				CreateNewScene();
				GUIUtility.ExitGUI();
			}

			GUILayout.Space(20f);

			GUILayout.FlexibleSpace();
			GUILayout.Label("Made with ❤ by GameRig", EditorStyles.centeredGreyMiniLabel);
		}

		private void Refresh()
		{
			ResetData();
			LoadScenesData();
			SaveScenesFromAssets();
		}

		private void LoadScenesData()
		{
			scenesAssets = AssetDatabase.FindAssets("t:Scene");

			if (scenesAssets.Length == 0)
			{
				GUILayout.Label("No Scenes Found", EditorStyles.centeredGreyMiniLabel);
				GUILayout.Label("Create New Scenes", EditorStyles.centeredGreyMiniLabel);
				GUILayout.Label("And Switch Between them here", EditorStyles.centeredGreyMiniLabel);
			}

			foreach (string scene in scenesAssets)
			{
				string path = AssetDatabase.GUIDToAssetPath(scene);
				SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

				string sceneName = sceneAsset.name;
				string scenePath = path;
				string assetName = path.Split('/')[1];

				SceneData newSceneData = new SceneData
				{
					ScenePath = scenePath,
					SceneName = sceneName,
					AssetName = assetName
				};

				assetsNames.Add(assetName);
				sceneDataList.Add(newSceneData);

				AssetButtonState newAssetButtonState = new AssetButtonState
				{
					isActive = false,
					AnimBool = new AnimBool(false)
				};
				newAssetButtonState.AnimBool.valueChanged.AddListener(Repaint);
				assetButtonStateHasSet.Add(newAssetButtonState);
			}
		}

		private void SaveScenesFromAssets()
		{
			assetsNamesList = assetsNames.ToList();

			foreach (string assetName in assetsNamesList)
			{
				List<SceneData> sceneDataFromAsset = sceneDataList.FindAll(currentAssetName => currentAssetName.AssetName == assetName);

				if (!sceneDataDictionary.ContainsKey(assetName))
				{
					sceneDataDictionary.Add(assetName, sceneDataFromAsset);
				}
			}
		}

		private void DrawAssetsButtons(List<string> assetsNamesList)
		{
			for (int i = 0; i < sceneDataDictionary.Count; i++)
			{
				string assetName = assetsNamesList[i];
				List<SceneData> allScenesFromAsset = sceneDataDictionary[assetName];
				List<AssetButtonState> assetButtonStateList = assetButtonStateHasSet.ToList();

				GUILayout.Space(5f);

				if (GUILayout.Button(assetName, EditorStyles.toolbarButton))
				{
					assetButtonStateList[i].isActive = !assetButtonStateList[i].isActive;
				}

				assetButtonStateList[i].AnimBool.target = assetButtonStateList[i].isActive;

				if (EditorGUILayout.BeginFadeGroup(assetButtonStateList[i].AnimBool.faded))
				{
					// float width = EditorGUIUtility.currentViewWidth / 2f;

					// EditorGUILayout.BeginHorizontal(/*"box", GUILayout.Width(width)*/);
					EditorGUILayout.BeginVertical("box");

					DrawScenesButtons(allScenesFromAsset);

					EditorGUILayout.EndVertical();
				}

				EditorGUILayout.EndFadeGroup();
			}
		}

		private void DrawScenesButtons(IEnumerable<SceneData> allScenesFromAsset)
		{
			List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

			GUILayout.Space(5f);

			foreach (SceneData sceneData in allScenesFromAsset)
			{
				EditorGUI.indentLevel++;

				string scenePath = sceneData.ScenePath;
				string sceneName = sceneData.SceneName;

				EditorBuildSettingsScene buildScene = buildScenes.Find((editorBuildScene) => editorBuildScene.path == scenePath);
				Scene scene = SceneManager.GetSceneByPath(scenePath);
				bool isOpen = scene.IsValid() && scene.isLoaded;
				GUI.enabled = !isOpen;

				GUILayout.BeginHorizontal();

				if (scenesSource == ScenesSource.Assets)
				{
					if (GUILayout.Button(sceneName, GUILayout.Width(170f)))
					{
						Open(scenePath);
					}
				}
				else
				{
					if (buildScene != null)
					{
						if (GUILayout.Button(sceneName, GUILayout.Width(170f)))
						{
							Open(scenePath);
						}
					}
				}

				GUILayout.FlexibleSpace();

				GUI.enabled = true;

				GUILayout.BeginHorizontal();

				if (GUILayout.Button("Show", GUILayout.Width(50f)))
				{
					Object sceneObject = AssetDatabase.LoadMainAssetAtPath(scenePath);
					ProjectWindowUtil.ShowCreatedAsset(sceneObject);
				}

				if (GUILayout.Button("Delete", GUILayout.Width(50f)))
				{
					const string titleString = "Delete selected scene?";
					string message = scenePath + Environment.NewLine + "You cannot undo this action.";
					const string okString = "Delete";
					const string cancelString = "Cancel";

					if (EditorUtility.DisplayDialog(titleString, message, okString, cancelString))
					{
						DeleteScene(scenePath);
					}
				}

				GUILayout.EndHorizontal();

				GUILayout.EndHorizontal();

				GUILayout.Space(5f);
				EditorGUI.indentLevel--;
			}
		}

		private void DeleteScene(string scenePath)
		{
			AssetDatabase.DeleteAsset(scenePath);
			Refresh();
		}

		public void Open(string path)
		{
			if (EditorSceneManager.EnsureUntitledSceneHasBeenSaved(
				"You don't have saved the Untitled Scene, Do you want to leave?"))
			{
				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
				EditorSceneManager.OpenScene(path, openSceneMode);
			}
		}

		private void CreateNewScene()
		{
			Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
			EditorSceneManager.SaveScene(newScene);
		}
	}
}