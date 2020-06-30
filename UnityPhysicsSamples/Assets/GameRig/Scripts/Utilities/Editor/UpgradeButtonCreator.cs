using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameRig.Scripts.Utilities.Editor
{
	public class UpgradeButtonCreator : MonoBehaviour
	{
		[MenuItem("GameObject/UI/Upgrade Button", priority = 1)]
		public static void Create()
		{
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

			Transform selectedTransform = Selection.activeTransform;

			Selection.activeObject = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>(GameRigResourcesPaths.UpgradeButtonPrefab));

			if (selectedTransform != null)
			{
				Selection.activeTransform.SetParent(selectedTransform);
				Selection.activeGameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Undo.RegisterCreatedObjectUndo(Selection.activeObject, "Create Upgrade Button");
		}
	}
}