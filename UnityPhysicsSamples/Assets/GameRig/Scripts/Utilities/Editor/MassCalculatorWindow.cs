using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameRig.Scripts.Utilities.Editor
{
	/// <summary>
	/// This class is used to override all rigidbodies mass by calculating it based on density and meshes attached to rigidbody game object
	/// </summary>
	public sealed class MassCalculatorWindow : EditorWindow
	{
		private static float density;

		[MenuItem("GameRig/Tools/Mass Calculator", priority = 111)]
		public static void ShowWindow()
		{
			GetWindow<MassCalculatorWindow>("Mass Calculator");
		}

		private void OnGUI()
		{
			GUILayout.Space(6f);

			density = EditorGUILayout.FloatField(new GUIContent("Density"), density);

			if (GUILayout.Button("Calculate Masses", GUILayout.Height(25)))
			{
				CalculateMasses();
			}
		}

		private static void CalculateMasses()
		{
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

			Rigidbody[] bodies = (Rigidbody[]) FindObjectsOfType(typeof(Rigidbody));

			int massesChangedCount = 0;

			foreach (Rigidbody body in bodies)
			{
				massesChangedCount++;

				EditorUtility.SetDirty(body);

				body.SetDensity(density);

				body.mass = body.mass;
			}

			Debug.Log("Updated mass for " + massesChangedCount + " rigidbodies");

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
		}
	}
}