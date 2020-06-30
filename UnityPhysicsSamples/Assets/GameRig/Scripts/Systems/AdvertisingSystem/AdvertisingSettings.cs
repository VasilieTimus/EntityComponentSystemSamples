using GameRig.Scripts.Utilities.Attributes;
using UnityEngine;

namespace GameRig.Scripts.Systems.AdvertisingSystem
{
	/// <summary>
	/// These settings are used by <see cref="AdvertisingManager"/>
	/// </summary>
	public class AdvertisingSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("Enables Advertising System functionality")]
		private bool useAdvertisingSystem;

		[SerializeField] [Tooltip("Application ID")]
		private string appId;

		[SerializeField] [Expandable] [Tooltip("Add units settings")]
		private AdUnitsSettings adUnitsSettings;

		[SerializeField] [Tooltip("First Interstitial show delay after first game start")]
		private int firstInterstitialShowDelay;

		[SerializeField] [Tooltip("Interstitial show frequency")]
		private int interstitialShowCooldown;

		[SerializeField] [Tooltip("Interstitial/Rewarded Ad load delay in case of load fail")]
		private float adLoadDelay;

		/// <summary>
		/// Gets the Advertising System functionality state
		/// </summary>
		public bool UseAdvertisingSystem => useAdvertisingSystem;

		/// <summary>
		/// Gets the Application ID
		/// </summary>
		public string AppId => appId;

		/// <summary>
		/// Gets the Ad units Ids
		/// </summary>
		public AdUnitsSettings UnitsSettings => adUnitsSettings;

		/// <summary>
		/// Gets the first Interstitial show delay after first game start
		/// </summary>
		public int FirstInterstitialShowDelay => firstInterstitialShowDelay;

		/// <summary>
		/// Gets the Interstitial show frequency
		/// </summary>
		public int InterstitialShowCooldown => interstitialShowCooldown;

		/// <summary>
		/// Gets the Interstitial/Rewarded Ad load delay in case of load fail
		/// </summary>
		public float AdLoadDelay => adLoadDelay;
	}
}