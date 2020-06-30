using GameRig.Scripts.Systems.CurrencySystem;
using GameRig.Scripts.Systems.UpgradesSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameRig.Scripts.UI
{
	/// <summary>
	/// This class is used to display Upgrade related info in Interface
	/// </summary>
	public class UpgradeButton : MonoBehaviour
	{
		[SerializeField] private UpgradeSettings upgradeSettings;
		[SerializeField] private TextMeshProUGUI upgradeNameText;
		[SerializeField] private TextMeshProUGUI levelText;
		[SerializeField] private TextMeshProUGUI priceText;
		[SerializeField] private InterfaceUpgradeValue upgradeValuePrefab;
		[SerializeField] private Transform upgradeValuesParent;
		[SerializeField] private Button upgradeButton;
  
		private InterfaceUpgradeValue[] upgradeValues;

		private bool isInitialized;

		private void Awake()
		{
			if (upgradeSettings != null)
			{
				Initialize(upgradeSettings);
			}
		}

		private void Update()
		{
			UpdateButtonState();
		}

		public void Initialize(UpgradeSettings upgradeSettings)
		{
			if (isInitialized || upgradeSettings == null)
			{
				return;
			}

			this.upgradeSettings = upgradeSettings;

			upgradeSettings.onUpgradeIncrement += UpdateInterface;

			upgradeButton.onClick.AddListener(OnUpgradeClick);

			CreateUpgradeValues();
			UpdateInterface();

			isInitialized = true;
		}

		public void OnUpgradeClick()
		{
			GameCurrencyManager.CurrencyAmount -= upgradeSettings.UpgradePrice;

			upgradeSettings.IncrementUpgrade();
		}

		private void UpdateButtonState()
		{
			if (isInitialized)
			{
				upgradeButton.interactable = upgradeSettings.IsPurchasable(GameCurrencyManager.CurrencyAmount);
			}
		}

		private void CreateUpgradeValues()
		{
			upgradeValues = new InterfaceUpgradeValue[upgradeSettings.UpgradeValueProgressions.Length];

			for (int i = 0; i < upgradeValues.Length; i++)
			{
				upgradeValues[i] = Instantiate(upgradeValuePrefab.gameObject, upgradeValuesParent).GetComponent<InterfaceUpgradeValue>();
			}
		}

		private void UpdateInterface()
		{
			upgradeNameText.text = upgradeSettings.UpgradeName;
			levelText.text = "Level " + (upgradeSettings.CurrentLevel + 1);
			priceText.text = upgradeSettings.PricePrefix + GameCurrencyManager.GetDisplayString(upgradeSettings.UpgradePrice) + upgradeSettings.PriceSuffix;

			UpdateButtonState();
			UpdateUpgradeValues();
		}

		private void UpdateUpgradeValues()
		{
			for (int i = 0; i < upgradeValues.Length; i++)
			{
				upgradeValues[i].Setup(upgradeSettings.UpgradeValueProgressions[i]);
			}
		}
	}
}