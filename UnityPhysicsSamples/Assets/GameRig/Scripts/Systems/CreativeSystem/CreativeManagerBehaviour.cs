#if UNITY_STANDALONE
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameRig.Scripts.Systems.CreativeSystem
{
	public class CreativeManagerBehaviour : MonoBehaviour
	{
		private static MethodInfo getGroup;

		public void SaveScreenshots(IEnumerable<ScreenshotSettings> screenshotsSettings)
		{
			StartCoroutine(ScreenshotsSaveCoroutine(screenshotsSettings));
		}

		private IEnumerator ScreenshotsSaveCoroutine(IEnumerable<ScreenshotSettings> screenshotsSettings)
		{
			Vector2Int currentResolution = new Vector2Int(Screen.width, Screen.height);

			string applicationName = Application.productName;

			applicationName = ExtractInvalidFileNameChars(applicationName);
			applicationName = ExtractInvalidPathChars(applicationName);

			float currentTimeScale = Time.timeScale;

			Time.timeScale = 0f;

			foreach (ScreenshotSettings screenshotSettings in screenshotsSettings)
			{
				yield return StartCoroutine(SaveScreenshot(applicationName, screenshotSettings));
			}

			Time.timeScale = currentTimeScale;

			Screen.SetResolution(currentResolution.x, currentResolution.y, true);
		}

		private IEnumerator SaveScreenshot(string applicationName, ScreenshotSettings screenshotSettings)
		{
			string screenshotFolderName = ExtractInvalidPathChars(screenshotSettings.ScreenshotKey);
			string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + applicationName + " Screenshots\\" + screenshotFolderName;
			string fileName = applicationName.Replace(' ', '_')
			                  + "_"
			                  + screenshotSettings.ScreenshotKey
			                  + "_"
			                  + screenshotSettings.Resolution.x
			                  + "x"
			                  + screenshotSettings.Resolution.y;

			int captureId = GetScreenCapturesCount(directoryPath, fileName) + 1;

			fileName += "_" + captureId;
			fileName = ExtractInvalidPathChars(fileName);
			fileName = ExtractInvalidFileNameChars(fileName);

			Screen.SetResolution(screenshotSettings.Resolution.x, screenshotSettings.Resolution.y, false);

			yield return null;

			ScreenCapture.CaptureScreenshot(directoryPath + "\\" + fileName + ".png");
		}

		private static string ExtractInvalidFileNameChars(string fileName)
		{
			return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, invalidFileNameChar) => current.Replace(invalidFileNameChar.ToString(), ""));
		}

		private static string ExtractInvalidPathChars(string directoryName)
		{
			return Path.GetInvalidPathChars().Aggregate(directoryName, (current, invalidPathChar) => current.Replace(invalidPathChar.ToString(), ""));
		}

		private int GetScreenCapturesCount(string directoryPath, string fileName)
		{
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);

				return 0;
			}

			string[] files = Directory.GetFiles(directoryPath);

			return files.Count(file => file.Contains(fileName));
		}
	}
}
#endif