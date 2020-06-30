using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameRig.Scripts.Utilities.Editor
{
	public class StaticObjectWindow : EditorWindow
	{
		private static FindAllStatic findStatic;
		private static string[] allProjectPaths;
		private static IEnumerable<Type> staticObjects;
		private static bool windowWasDrawn;

		private Vector2 scrollPosition = Vector2.zero;

		[MenuItem("GameRig/Tools/Static Objects Viewer")]
		public static void CreateObserverWindow()
		{
			GetWindow<StaticObjectWindow>();
			findStatic = new FindAllStatic();
		}

		public void OnGUI()
		{
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
			EditorGUIUtility.wideMode = true;

			foreach (FindAllStatic.StaticClassInfo st in findStatic.GetAllStaticClassesInfo())
			{
				DrawStaticClasses(st);
				DrawFoldedStaticVariables(st);
				DrawUILine(Color.gray);
			}

			GUILayout.EndScrollView();
		}

		private void DrawStaticClasses(FindAllStatic.StaticClassInfo st)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label($"[{st.ClassType}]: is a static class.", EditorStyles.largeLabel);
			if (!string.IsNullOrEmpty(st.Path))
			{
				if (GUILayout.Button($"Show: [{st.ClassType}]"))
				{
					Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(st.Path);
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		private void DrawFoldedStaticVariables(FindAllStatic.StaticClassInfo st)
		{
			if (st.AllStaticFields.Count > 0)
			{
				st.Folded = EditorGUILayout.Foldout(st.Folded, $"Show static variables of {st.ClassType.Name}", true,
					EditorStyles.foldout);

				if (st.Folded)
				{
					foreach (FieldInfo v in st.AllStaticFields)
					{
						GUILayout.Label($"[{v.Name}]: is a static variable = {v.GetValue(null)}");
					}
				}
			}
		}

		public void OnInspectorUpdate()
		{
			Repaint();
		}

		public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
		{
			Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
			r.height = thickness;
			r.y += padding / 2;
			r.x -= 2;
			r.width += 6;
			EditorGUI.DrawRect(r, color);
		}
	}
}