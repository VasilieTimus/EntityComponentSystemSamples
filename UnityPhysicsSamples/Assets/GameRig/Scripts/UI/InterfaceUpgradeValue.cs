using GameRig.Scripts.Systems.UpgradesSystem;
using TMPro;
using UnityEngine;

namespace GameRig.Scripts.UI
{
	/// <summary>
	/// This class is used by Upgrade Button to display values settings
	/// </summary>
	public class InterfaceUpgradeValue : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI valueNameText;
		[SerializeField] private TextMeshProUGUI valueText;
		[SerializeField] private TextMeshProUGUI valueProgressText;

		public void Setup(UpgradeValueSettings settings)
		{
			valueNameText.text = settings.ValueName;
			valueText.text = settings.ValueString;
			valueProgressText.text = settings.ProgressString;
		}
	}
}