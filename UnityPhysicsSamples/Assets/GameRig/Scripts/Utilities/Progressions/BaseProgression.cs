using UnityEngine;

namespace GameRig.Scripts.Utilities.Progressions
{
	/// <summary>
	/// This class is used to create derived progression types
	/// </summary>
	public abstract class BaseProgression : ScriptableObject
	{
		/// <summary>
		/// This method evaluates the value for the given term index
		/// </summary>
		/// <param name="termIndex">Term index</param>
		/// <returns>Returns the value</returns>
		public abstract float Evaluate(int termIndex);

		/// <summary>
		/// Gets the progress change to next term
		/// </summary>
		public abstract string ProgressString { get; }
	}
}