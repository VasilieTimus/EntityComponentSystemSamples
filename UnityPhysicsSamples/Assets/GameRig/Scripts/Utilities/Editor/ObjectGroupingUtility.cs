using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameRig.Scripts.Utilities.Editor
{
	[InitializeOnLoad]
	public static class ObjectGroupingUtility
	{
		private static double renameTime;
		private static Object groupObject;

		static ObjectGroupingUtility()
		{
			System.Reflection.FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

			EditorApplication.CallbackFunction value = (EditorApplication.CallbackFunction) info?.GetValue(null);

			value += EditorGlobalKeyPress;

			info?.SetValue(null, value);
		}

		private static void EditorGlobalKeyPress()
		{
			if (Event.current.control && Event.current.keyCode == KeyCode.G && Event.current.type == EventType.KeyDown)
			{
				GroupSelectedObjects();
			}
		}

		private static void EngageRenameMode()
		{
			if (EditorApplication.timeSinceStartup >= renameTime)
			{
				EditorApplication.update -= EngageRenameMode;
				TriggerRenameEvent();
			}
		}

		public static void TriggerRenameEvent()
		{
			Selection.activeObject = groupObject;

			Event e = new Event {keyCode = KeyCode.F2, type = EventType.KeyDown};
			EditorWindow.focusedWindow.SendEvent(e);
		}

		private static void GroupSelectedObjects()
		{
			switch (EditorWindow.focusedWindow.titleContent.text)
			{
				case "Hierarchy":
					GroupInHierarchy();
					break;
				
				case "Project":
					GroupInProject();
					break;
			}
		}

		private static void GroupInHierarchy()
		{
			GameObject[] gameObjects = Selection.gameObjects;

			if (gameObjects.Length < 1)
				return;

			groupObject = new GameObject("New Group");
			Undo.RegisterCreatedObjectUndo(groupObject, "Grouping");

			foreach (GameObject gameObject in gameObjects)
			{
				Undo.SetTransformParent(gameObject.transform, ((GameObject) groupObject).transform, "Grouping");
			}

			Selection.activeObject = groupObject;

			renameTime = EditorApplication.timeSinceStartup + 0.2;
			EditorApplication.update += EngageRenameMode;
		}

		private static void GroupInProject()
		{
			if (Selection.objects.Length == 0)
				return;

			string[] selectedPathArray = AssetDatabase.GetAssetPath(Selection.objects[0]).Split('/');
			string selectedObjectName = selectedPathArray[selectedPathArray.Length - 1];
			string selectedPath = AssetDatabase.GetAssetPath(Selection.objects[0]).Replace("/" + selectedObjectName, "");
			string newFolderPath = selectedPath + "/New Folder";

			Directory.CreateDirectory(newFolderPath);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Object[] selectedObjects = Selection.objects;
			foreach (Object obj in selectedObjects)
			{
				string[] objectPathArray = AssetDatabase.GetAssetPath(obj).Split('/');
				string objectName = objectPathArray[selectedPathArray.Length - 1];
				AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(obj), newFolderPath + "/" + objectName);
			}

			AssetDatabase.SaveAssets();

			Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(selectedPath + "/New Folder");

			AssetDatabase.Refresh();

			renameTime = EditorApplication.timeSinceStartup + 0.2;
			EditorApplication.update += EngageRenameMode;
		}
	}
}