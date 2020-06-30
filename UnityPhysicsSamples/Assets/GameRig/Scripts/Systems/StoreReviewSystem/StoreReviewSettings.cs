using UnityEngine;

namespace GameRig.Scripts.Systems.StoreReviewSystem
{
	/// <summary>
	/// This settings are used by <see cref="StoreReviewManager"/> in order to handle the review requests
	/// </summary>
	public class StoreReviewSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("Minimal interval in hours between store review requests, zero means disabled requests")]
		private float minIntervalInHoursBetweenRequests;

		[SerializeField] [Tooltip("Game store link")]
		private string appStoreLink;

		/// <summary>
		/// Minimal interval in hours between store review requests, zero means disabled requests
		/// </summary>
		public float MinIntervalInHoursBetweenRequests => minIntervalInHoursBetweenRequests;

		/// <summary>
		/// Game store link
		/// </summary>
		public string AppStoreLink => appStoreLink;
	}
}