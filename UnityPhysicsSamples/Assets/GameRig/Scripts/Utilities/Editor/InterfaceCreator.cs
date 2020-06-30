using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameRig.Scripts.Utilities.Editor
{
	public class InterfaceCreator : MonoBehaviour
	{
		[MenuItem("GameObject/UI/UI Template", priority = 0)]
		public static void Create()
		{
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

			Selection.activeObject = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>(GameRigResourcesPaths.InterfaceTemplatePrefab));

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Undo.RegisterCreatedObjectUndo(Selection.activeObject, "Create Interface Template");
		}
	}
}