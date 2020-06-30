using UnityEngine;

namespace GameRig.Scripts.Utilities.Progressions
{
	/// <summary>
	/// This class is used to create geometric progressions
	/// </summary>
	[CreateAssetMenu(menuName = "GameRig/Progression/Geometric Progression", fileName = "Geometric Progression Settings")]
	public class GeometricProgression : SimpleProgression
	{
		[SerializeField] [Tooltip("Ratio of progression")]
		private float ratio;

		/// <inheritdoc />
		public override float Evaluate(int termIndex)
		{
			return BaseValue * Mathf.Pow(ratio, termIndex);
		}

		/// <inheritdoc />
		public override string ProgressString => $"{(ratio - 1) * 100:0.##}" + "%";
	}
}