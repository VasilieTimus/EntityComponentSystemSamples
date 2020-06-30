using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GameRig.Scripts.Utilities.Editor
{
	internal static class LightningSettingsUtility
	{
		private static SerializedObject sourceLightmapSettings;
		private static SerializedObject sourceRenderSettings;

		private const string CopySettingsMenuPath = "GameRig/Tools/Lightning Utility/Copy Lighting Settings";
		private const string PasteSettingsMenuPath = "GameRig/Tools/Lightning Utility/Paste Lighting Settings";
		private const string PasteSettingsAllMenuPath = "GameRig/Tools/Lightning Utility/Paste Lighting Settings in opened Scenes";

		[MenuItem(CopySettingsMenuPath, priority = 200)]
		private static void CopySettings()
		{
			if (!TryGetSettings(typeof(LightmapEditorSettings), "GetLightmapSettings", out Object lightmapSettings))
				return;

			if (!TryGetSettings(typeof(RenderSettings), "GetRenderSettings", out Object renderSettings))
				return;

			sourceLightmapSettings = new SerializedObject(lightmapSettings);
			sourceRenderSettings = new SerializedObject(renderSettings);
		}

		[MenuItem(PasteSettingsMenuPath, priority = 201)]
		private static void PasteSettings()
		{
			if (!TryGetSettings(typeof(LightmapEditorSettings), "GetLightmapSettings", out Object lightmapSettings))
				return;

			if (!TryGetSettings(typeof(RenderSettings), "GetRenderSettings", out Object renderSettings))
				return;

			CopyInternal(sourceLightmapSettings, new SerializedObject(lightmapSettings));
			CopyInternal(sourceRenderSettings, new SerializedObject(renderSettings));

			InternalEditorUtility.RepaintAllViews();
		}

		[MenuItem(PasteSettingsAllMenuPath, priority = 202)]
		private static void PasteSettingsAll()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			try
			{
				for (int n = 0; n < SceneManager.sceneCount; ++n)
				{
					Scene scene = SceneManager.GetSceneAt(n);
					if (!scene.IsValid() || !scene.isLoaded)
						continue;

					SceneManager.SetActiveScene(scene);

					PasteSettings();
				}
			}
			finally
			{
				SceneManager.SetActiveScene(activeScene);
			}
		}

		[MenuItem(PasteSettingsAllMenuPath, validate = true)]
		[MenuItem(PasteSettingsMenuPath, validate = true)]
		private static bool PasteValidate()
		{
			return sourceLightmapSettings != null && sourceRenderSettings != null;
		}

		private static void CopyInternal(SerializedObject source, SerializedObject dest)
		{
			SerializedProperty prop = source.GetIterator();
			while (prop.Next(true))
			{
				bool copyProperty = new[] {"m_Sun", "m_FileID", "m_PathID", "m_ObjectHideFlags"}.All(propertyName => !string.Equals(prop.name, propertyName, StringComparison.Ordinal));

				if (copyProperty)
					dest.CopyFromSerializedProperty(prop);
			}

			dest.ApplyModifiedProperties();
		}

		private static bool TryGetSettings(Type type, string methodName, out Object settings)
		{
			settings = null;

			MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
			if (method == null)
			{
				Debug.LogErrorFormat("CopyLightingSettings: Could not find {0}.{1}", type.Name, methodName);

				return false;
			}

			Object value = method.Invoke(null, null) as Object;
			if (value == null)
			{
				Debug.LogErrorFormat("CopyLightingSettings: Could get data from {0}.{1}", type.Name, methodName);

				return false;
			}

			settings = value;

			return true;
		}
	}
}