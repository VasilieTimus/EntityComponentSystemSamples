using UnityEngine;

namespace GameRig.Scripts.Systems.SocialSystem
{
	/// <summary>
	/// This settings are used by <see cref="SocialManager"/>
	/// </summary>
	public class SocialSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("Leaderboard name")]
		private string leaderboardName;

		/// <summary>
		/// Gets the leaderboard name
		/// </summary>
		public string LeaderboardName => leaderboardName;
	}
}