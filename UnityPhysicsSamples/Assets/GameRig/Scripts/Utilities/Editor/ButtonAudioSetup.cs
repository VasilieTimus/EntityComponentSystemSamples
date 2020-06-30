using GameRig.Scripts.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameRig.Scripts.Utilities.Editor
{
	/// <summary>
	/// This class is used to override all buttons with new <see cref="ButtonAudioController"></see> components with given <see cref="AudioClip"></see>
	/// </summary>
	public sealed class ButtonAudioSetup : EditorWindow
	{
		private static AudioClip buttonAudioClip;

		[MenuItem("GameRig/Tools/Buttons Audio Setter", priority = 111)]
		public static void ShowWindow()
		{
			GetWindow<ButtonAudioSetup>("Button Audio Setter");
		}

		private void OnGUI()
		{
			GUILayout.Space(6f);

			buttonAudioClip = (AudioClip) EditorGUILayout.ObjectField(new GUIContent("Audio Clip"), buttonAudioClip, typeof(AudioClip), false);

			if (GUILayout.Button("Update Buttons", GUILayout.Height(25)))
			{
				UpdateButtons();
			}
		}

		private static void UpdateButtons()
		{
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

			ButtonAudioController[] allButtonAudioControllers = (ButtonAudioController[]) FindObjectsOfType(typeof(ButtonAudioController));

			foreach (ButtonAudioController currentButtonAudioController in allButtonAudioControllers)
			{
				DestroyImmediate(currentButtonAudioController);
			}

			Button[] allButtons = (Button[]) FindObjectsOfType(typeof(Button));

			foreach (Button button in allButtons)
			{
				button.gameObject.AddComponent<ButtonAudioController>().SetAudioClip(buttonAudioClip);
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

			Debug.Log("Buttons updated succesfull");
		}
	}
}