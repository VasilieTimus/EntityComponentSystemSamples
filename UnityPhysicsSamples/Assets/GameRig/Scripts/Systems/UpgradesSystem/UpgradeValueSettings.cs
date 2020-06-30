using GameRig.Scripts.Systems.CurrencySystem;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.Progressions;
using UnityEngine;

namespace GameRig.Scripts.Systems.UpgradesSystem
{
	/// <summary>
	/// This settings are used to define the value settings for the selected <see cref="UpgradeSettings"/>
	/// </summary>
	[CreateAssetMenu(menuName = "GameRig/Upgrades System/Value Settings")]
	public class UpgradeValueSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("The upgrade value name")]
		private string valueName;

		[SerializeField] [Tooltip("The prefix used before the upgrade value")]
		private string prefix;

		[SerializeField] [Tooltip("The suffix used after the upgrade value")]
		private string suffix;

		[SerializeField] [Expandable] [Tooltip("The progression used for this value")]
		private BaseProgression valueProgression;

		/// <summary>
		/// Gets the <see cref="UpgradeSettings"/> this Value Settings are linked to
		/// </summary>
		public UpgradeSettings UpgradeSettings { get; private set; }

		/// <summary>
		/// Gets the current Upgrade Value settings value
		/// </summary>
		public float Value => ValueProgression.Evaluate(UpgradeSettings.CurrentLevel);

		/// <summary>
		/// Gets the value name
		/// </summary>
		public string ValueName => valueName;

		/// <summary>
		/// Gets the value string which is a concatenation of <see cref="prefix"/> + Value + <see cref="suffix"/> strings  
		/// </summary>
		public string ValueString => prefix + GameCurrencyManager.GetDisplayString(ValueProgression.Evaluate(UpgradeSettings.CurrentLevel)) + suffix;

		/// <inheritdoc cref="BaseProgression.ProgressString"/>
		public string ProgressString => ValueProgression.ProgressString;

		public BaseProgression ValueProgression
		{
			get => valueProgression;
			set => valueProgression = value;
		}

		/// <summary>
		/// Initialization is done automatically by <see cref="UpgradeSettings"/>
		/// </summary>
		/// <param name="upgradeSettings"></param>
		public void Initialize(UpgradeSettings upgradeSettings)
		{
			UpgradeSettings = upgradeSettings;
		}
	}
}