using UnityEngine;

namespace GameRig.Scripts.Systems.OfflineSystem
{
	/// <summary>
	/// This settings are used by <see cref="OfflineManager"/> 
	/// </summary>
	/// <remarks>
	/// Offline reward is counted only if <see cref="MinDeltaSeconds"/> was passed after going to background.
	/// </remarks>
	/// <para></para>
	/// <remarks>
	/// Offline time is caped to <see cref="MaxDeltaSeconds"/> 
	/// </remarks>
	public class OfflineSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("Minimum amount of seconds")]
		private int minDeltaSeconds;

		[SerializeField] [Tooltip("Maximum amount of seconds")]
		private int maxDeltaSeconds;

		/// <summary>
		/// Gets the minimum amount of seconds
		/// </summary>
		public int MinDeltaSeconds => minDeltaSeconds;

		/// <summary>
		/// Gets the maximum amount of seconds
		/// </summary>
		public int MaxDeltaSeconds => maxDeltaSeconds;
	}
}