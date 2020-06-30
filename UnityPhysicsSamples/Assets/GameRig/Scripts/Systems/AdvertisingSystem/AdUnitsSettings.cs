using UnityEngine;

namespace GameRig.Scripts.Systems.AdvertisingSystem
{
	/// <summary>
	/// These settings are used to load respective Ads 
	/// </summary>
	[CreateAssetMenu(menuName = "GameRig/Advertising System/Ad Units Settings")]
	public class AdUnitsSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("Banner Ad Unit ID")]
		private string bannerAdUnit;

		[SerializeField] [Tooltip("Interstitial Ad Unit ID")]
		private string interstitialAdUnit;

		[SerializeField] [Tooltip("Rewarded Video Ad Unit ID")]
		private string rewardedAdUnit;

		/// <summary>
		/// Gets the Banner Ad Unit ID
		/// </summary>
		public string BannerAdUnit => bannerAdUnit;

		/// <summary>
		/// Gets the Interstitial Ad Unit ID
		/// </summary>
		public string InterstitialAdUnit => interstitialAdUnit;

		/// <summary>
		/// Gets the Rewarded Video Ad Unit ID
		/// </summary>
		public string RewardedAdUnit => rewardedAdUnit;
	}
}