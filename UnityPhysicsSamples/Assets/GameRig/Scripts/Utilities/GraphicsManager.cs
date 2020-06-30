using UnityEngine;

namespace GameRig.Scripts.Utilities
{
	/// <summary>
	/// This class is used to override current quality settings to the most optimized one
	/// </summary>
	public static class GraphicsManager
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
#if !UNITY_EDITOR
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Application.targetFrameRate = 60;
			QualitySettings.vSyncCount = 0;
#endif
		}
	}
}