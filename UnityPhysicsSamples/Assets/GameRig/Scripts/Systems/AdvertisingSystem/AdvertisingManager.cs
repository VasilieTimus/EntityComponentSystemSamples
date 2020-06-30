using System;

namespace GameRig.Scripts.Systems.AdvertisingSystem
{
	/// <summary>
	/// This class is used to manage Ads
	/// </summary>
	public static class AdvertisingManager
	{
		/// <summary>
		/// Gets the Rewarded Video Ad load state
		/// </summary>
		public static bool IsRewardedAdAvailable => true;

		/// <summary>
		/// Shows a Rewarded Video Ad
		/// </summary>
		/// <param name="onRewardedAdEnd">Callback when the rewarded ad is closed</param>
		public static void ShowRewardedAd(Action<bool> onRewardedAdEnd)
		{
			onRewardedAdEnd?.Invoke(true);
		}

		/// <summary>
		/// Shows an Interstitial Ad
		/// </summary>
		public static void ShowInterstitial()
		{
		}
	}
}