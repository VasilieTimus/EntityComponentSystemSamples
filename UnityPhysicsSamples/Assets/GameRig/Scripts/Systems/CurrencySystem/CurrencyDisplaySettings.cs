using UnityEngine;

namespace GameRig.Scripts.Systems.CurrencySystem
{
	/// <summary>
	/// This class is used to pair a currency amount with a string
	/// </summary>
	[System.Serializable]
	public class CurrencyDisplaySettings
	{
		[SerializeField] private string suffix;
		[SerializeField] private float amount;

		/// <summary>
		/// Gets the Suffix for the specific amount 
		/// </summary>
		public string Suffix => suffix;

		/// <summary>
		/// Gets the amount to which is paired the abbreviation 
		/// </summary>
		public float Amount => amount;
	}
}