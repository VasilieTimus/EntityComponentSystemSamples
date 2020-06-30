using UnityEngine;

namespace GameRig.Scripts.Utilities.Progressions
{
	/// <summary>
	/// This class is used to create derived types of progressions which have a base value
	/// </summary>
	public abstract class SimpleProgression : BaseProgression
	{
		[SerializeField] [Tooltip("The value for the first term")]
		private float baseValue;

		/// <summary>
		/// The value for the first term
		/// </summary>
		protected float BaseValue => baseValue;
	}
}