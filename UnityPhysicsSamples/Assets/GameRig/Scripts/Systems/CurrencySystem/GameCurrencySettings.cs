using UnityEngine;

namespace GameRig.Scripts.Systems.CurrencySystem
{
	/// <summary>
	/// This class contains the main settings for <see cref="GameCurrencyManager"/>
	/// </summary>
	public class GameCurrencySettings : ScriptableObject
	{
		[SerializeField] private CurrencyDisplaySettings[] currencyDisplaySettings;

		/// <summary>
		/// Returns the display settings
		/// </summary>
		public CurrencyDisplaySettings[] DisplaySettings => currencyDisplaySettings;
	}
}