using System;
using GameRig.Scripts.Systems.PubSubSystem;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;

namespace GameRig.Scripts.Systems.CurrencySystem
{
	/// <summary>
	/// This class have basic functionality on Game Currency
	/// </summary>
	/// <remarks>
	/// It handles the Save/Load logic automatically and calculates the delta amount during the last game session 
	/// </remarks>
	public static class GameCurrencyManager
	{
		private static GameCurrencySettings settings;

		private static float currencyAmount;
		private static string displayString;

		public delegate void GameCurrencyChangeDelegate(float amount, float deltaAmount);

		public static GameCurrencyChangeDelegate OnGameCurrencyChange = delegate { };

		/// <summary>
		/// Gets or sets the the Game Currency amount
		/// </summary>
		public static float CurrencyAmount
		{
			get => (float) Math.Truncate(currencyAmount);
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}

				float deltaAmount = value - currencyAmount;

				if (deltaAmount > 0f)
				{
					deltaAmount *= CurrencyMultiplier;
				}

				SessionCurrencyAmount += deltaAmount;
				currencyAmount += deltaAmount;

				SaveManager.Save(GameRigSaveKeys.CurrencyAmount, currencyAmount);

				OnGameCurrencyChange(currencyAmount, deltaAmount);
			}
		}

		/// <summary>
		/// Gets or sets the game currency multiplier that is applied only on game currency addition 
		/// </summary>
		public static float CurrencyMultiplier { get; set; }

		/// <summary>
		/// Gets the delta currency amount during the game session
		/// </summary>
		public static float SessionCurrencyAmount { get; private set; }

		[InitializeOnLaunch(typeof(PubSubManager))]
		private static void Initialize()
		{
			settings = Resources.Load<GameCurrencySettings>(GameRigResourcesPaths.GameCurrencySettings);

			CurrencyMultiplier = 1f;

			CurrencyAmount = SaveManager.Load(GameRigSaveKeys.CurrencyAmount, 0f);
		}

		/// <summary>
		/// Returns a string that represents a shortened version of a number using suffixes
		/// </summary>
		/// <param name="number">Number to shorten</param>
		/// <returns>Shortened number with suffix</returns>
		public static string GetDisplayString(float number)
		{
			displayString = Mathf.Abs(number) < 1f && number >= 0.1f ? $"{number:0.0}" : $"{number:0}";

			for (int i = settings.DisplaySettings.Length - 1; i >= 0; i--)
			{
				CurrencyDisplaySettings displaySettings = settings.DisplaySettings[i];

				if (number < displaySettings.Amount)
				{
					continue;
				}

				displayString = $"{number / displaySettings.Amount:0.0}" + displaySettings.Suffix;

				break;
			}

			return displayString;
		}

		public static void ResetSessionCurrencyAmount()
		{
			SessionCurrencyAmount = 0f;
		}
	}
}