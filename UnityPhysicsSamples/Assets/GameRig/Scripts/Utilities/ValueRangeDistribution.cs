using UnityEngine;

namespace GameRig.Scripts.Utilities
{
	/// <summary>
	/// This class is used to create value distributions defined by the given curve
	/// </summary>
	[System.Serializable]
	public class ValueRangeDistribution
	{
		[SerializeField] [Tooltip("Value range")]
		private Vector2 valueRange;

		[SerializeField] [Tooltip("Value distribution curve")]
		private AnimationCurve valueDistribution;

		[SerializeField] [Tooltip("Random range added to calculated value")]
		private float randomRange;

		/// <summary>
		/// Evaluates the value by the given position in curve
		/// </summary>
		/// <param name="position">The position in curve</param>
		/// <returns>Value</returns>
		public float Evaluate(float position)
		{
			return Mathf.Clamp
			(
				valueRange.x + valueDistribution.Evaluate(position) * (valueRange.y - valueRange.x) + Random.Range(-randomRange, randomRange),
				valueRange.x,
				valueRange.y
			);
		}
	}
}