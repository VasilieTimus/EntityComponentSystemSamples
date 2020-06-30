using UnityEditor;
using UnityEngine;

namespace GameRig.Scripts.Utilities.Editor
{
	[CustomPropertyDrawer(typeof(ScriptableObject), true)]
	public class ScriptableObjectDrawer : PropertyDrawer
	{
		// Cached scriptable object editor
		private UnityEditor.Editor editor;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Draw label
			EditorGUI.PropertyField(position, property, label, true);

			// Draw foldout arrow
			if (property.objectReferenceValue != null)
			{
				property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
			}

			// Draw foldout properties
			if (property.isExpanded)
			{
				// Make child fields be indented
				EditorGUI.indentLevel++;

				// background
				GUILayout.BeginVertical("box");

				if (!editor)
					UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);

				// Draw object properties
				EditorGUI.BeginChangeCheck();
				if (editor) // catch empty property
				{
					editor.OnInspectorGUI();
				}

				if (EditorGUI.EndChangeCheck())
					property.serializedObject.ApplyModifiedProperties();

				GUILayout.EndVertical();

				// Set indent back to what it was
				EditorGUI.indentLevel--;
			}
		}
	}

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Object), true)]
	public class UnityObjectEditor : UnityEditor.Editor
	{
	}
}