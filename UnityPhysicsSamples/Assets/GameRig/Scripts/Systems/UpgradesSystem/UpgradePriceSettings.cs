using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.Progressions;
using UnityEngine;

namespace GameRig.Scripts.Systems.UpgradesSystem
{
	/// <summary>
	/// This class is used to create price settings for a certain or multiple upgrades
	/// </summary>
	[CreateAssetMenu(menuName = "GameRig/Upgrades System/Price Settings")]
	public class UpgradePriceSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("Price prefix")]
		private string pricePrefix;

		[SerializeField] [Tooltip("Price suffix")]
		private string priceSuffix;

		[SerializeField] [Expandable] [Tooltip("Progression used for price")]
		private BaseProgression priceProgression;

		/// <summary>
		/// Gets the price prefix
		/// </summary>
		public string PricePrefix => pricePrefix;

		/// <summary>
		/// Gets the price suffix
		/// </summary>
		public string PriceSuffix => priceSuffix;

		/// <summary>
		/// Gets the price progression
		/// </summary>
		public BaseProgression PriceProgression
		{
			get => priceProgression;
			set => priceProgression = value;
		}
	}
}