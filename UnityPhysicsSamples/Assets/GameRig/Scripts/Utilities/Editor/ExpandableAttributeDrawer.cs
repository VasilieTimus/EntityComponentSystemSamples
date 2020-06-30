#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using GameRig.Scripts.Utilities.Attributes;
using UnityEditor;
using UnityEngine;

namespace GameRig.Scripts.Utilities.Editor
{
	/// <summary>
	/// Draws the property field for any field marked with ExpandableAttribute.
	/// </summary>
	[CustomPropertyDrawer(typeof(ExpandableAttribute), true)]
	public class ExpandableAttributeDrawer : PropertyDrawer
	{
		// Use the following area to change the style of the expandable ScriptableObject drawers;

		#region Style Setup

		/// <summary>
		/// The spacing on the inside of the background rect.
		/// </summary>
		private const float InnerSpacing = 6.0f;

		/// <summary>
		/// The spacing on the outside of the background rect.
		/// </summary>
		private const float OuterSpacing = 4.0f;

		#endregion

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float totalHeight = 0.0f;

			totalHeight += EditorGUIUtility.singleLineHeight;

			if (property.objectReferenceValue == null)
				return totalHeight;

			if (!property.isExpanded)
				return totalHeight;

			SerializedObject targetObject = new SerializedObject(property.objectReferenceValue);

			SerializedProperty field = targetObject.GetIterator();

			field.NextVisible(true);

			while (field.NextVisible(false))
			{
				totalHeight += EditorGUI.GetPropertyHeight(field, true) + EditorGUIUtility.standardVerticalSpacing;
			}

			totalHeight += InnerSpacing * 2;
			totalHeight += OuterSpacing * 2;

			return totalHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Rect fieldRect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};

			EditorGUI.PropertyField(fieldRect, property, label, true);

			if (property.objectReferenceValue == null)
				return;

			property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, GUIContent.none, true);

			if (!property.isExpanded)
				return;

			SerializedObject targetObject = new SerializedObject(property.objectReferenceValue);

			#region Format Field Rects

			List<Rect> propertyRects = new List<Rect>();
			Rect marchingRect = new Rect(fieldRect);

			Rect bodyRect = new Rect(fieldRect);
			bodyRect.xMin += EditorGUI.indentLevel * 14;
			bodyRect.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + OuterSpacing;

			SerializedProperty field = targetObject.GetIterator();
			field.NextVisible(true);

			marchingRect.y += InnerSpacing + OuterSpacing;

			while (field.NextVisible(false))
			{
				marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;
				marchingRect.height = EditorGUI.GetPropertyHeight(field, true);
				propertyRects.Add(marchingRect);
			}

			marchingRect.y += InnerSpacing;

			bodyRect.yMax = marchingRect.yMax;

			#endregion

			DrawBackground(bodyRect);

			#region Draw Fields

			EditorGUI.indentLevel++;

			int index = 0;
			field = targetObject.GetIterator();
			field.NextVisible(true);

//			Replacement for "editor.OnInspectorGUI ();" so we have more control on how we draw the editor
			while (field.NextVisible(false))
			{
				try
				{
					EditorGUI.PropertyField(propertyRects[index], field, true);
				}
				catch (StackOverflowException)
				{
					field.objectReferenceValue = null;
					Debug.LogError("Detected self-nesting cauisng a StackOverflowException, avoid using the same " +
					               "object iside a nested structure.");
				}

				index++;
			}

			targetObject.ApplyModifiedProperties();

			EditorGUI.indentLevel--;

			#endregion
		}

		/// <summary>
		/// Draws the Background
		/// </summary>
		/// <param name="rect">The Rect where the background is drawn.</param>
		private void DrawBackground(Rect rect)
		{
			EditorGUI.HelpBox(rect, "", MessageType.None);
		}
	}
}
#endif