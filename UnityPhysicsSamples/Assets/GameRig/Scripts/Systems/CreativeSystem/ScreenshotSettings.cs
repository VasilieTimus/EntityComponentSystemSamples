using UnityEngine;

namespace GameRig.Scripts.Systems.CreativeSystem
{
	[CreateAssetMenu(fileName = "Screenshot Settings", menuName = "GameRig/Creative System/Screenshot Settings")]
	public class ScreenshotSettings : ScriptableObject
	{
		[SerializeField] private Vector2Int resolution;
		[SerializeField] private string screenshotKey;

		public Vector2Int Resolution => resolution;
		public string ScreenshotKey => screenshotKey;
	}
}