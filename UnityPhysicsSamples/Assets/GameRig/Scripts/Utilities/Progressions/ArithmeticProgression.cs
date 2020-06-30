using UnityEngine;

namespace GameRig.Scripts.Utilities.Progressions
{
	/// <summary>
	/// This class is used to create arithmetic progressions
	/// </summary>
	[CreateAssetMenu(menuName = "GameRig/Progression/Arithmetic Progression", fileName = "Arithmetic Progression Settings")]
	public class ArithmeticProgression : SimpleProgression
	{
		[SerializeField] [Tooltip("Difference between terms")]
		private float difference;

		/// <inheritdoc />
		public override float Evaluate(int termIndex)
		{
			return BaseValue + difference * termIndex;
		}

		/// <inheritdoc />
		public override string ProgressString => difference.ToString();
	}
}