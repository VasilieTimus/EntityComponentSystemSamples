using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameRig.Scripts.Utilities.Editor
{
	// ReSharper disable once UnusedType.Global
	public static class GameRigSetup
	{
		private const string FilesPaths =
			"Art,Art/Animations,Art/Animations/Animation Clips,Art/Animations/Animators,Art/Fonts,Art/Materials,Art/Models,Art/Sprites,Art/Sprites/Game,Art/Sprites/UI,Art/Textures,Art/VFX,Prefabs,Presets,Resources,Scenes,Scripts,Scripts/Game,Scripts/UI";

		public static void SetupAssetsFolders()
		{
			string[] args = Environment.GetCommandLineArgs();

			PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, args[7]);
			PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, args[7]);
			PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, args[7]);

			PlayerSettings.productName = args[8].Replace("_", " ");
			PlayerSettings.companyName = args[9].Replace("_", " ");

			string[] foldersPaths = FilesPaths.Split(',');

			string[] splittedDataPath = Application.dataPath.Split("/"[0]);
			string projectName = splittedDataPath[splittedDataPath.Length - 2].Replace('_', ' ');

			foreach (string folder in foldersPaths)
			{
				string folderPath = Application.dataPath + "/" + projectName + "/" + folder;
				Directory.CreateDirectory(folderPath);
				File.Create(folderPath + "/.gitkeep");
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

			EditorSceneManager.SaveScene(newScene, Application.dataPath + "/" + projectName + "/Scenes" + "/Main Scene.unity");

			EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[1];

			scenes[0] = new EditorBuildSettingsScene("Assets/" + projectName + "/Scenes" + "/Main Scene.unity", true);

			EditorBuildSettings.scenes = scenes;

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}