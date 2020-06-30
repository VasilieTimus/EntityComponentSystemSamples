using UnityEngine;

namespace GameRig.Scripts.Systems.PoolingSystem
{
	/// <summary>
	/// This settings are used to store the information about objects to pool
	/// </summary>
	[System.Serializable]
	public class PooledObjectSettings
	{
		[SerializeField] [Tooltip("Object that will be pooled at every scene load")]
		private GameObject objectToPool;

		[SerializeField] [Tooltip("Minimum copies of objects to instantiate")]
		private int minimumPooledAmount;

		/// <summary>
		/// Gets the pullable object
		/// </summary>
		public GameObject ObjectToPool => objectToPool;

		/// <summary>
		/// Gets the minimum amount of <see cref="ObjectToPool"/> that will be created at each scene load
		/// </summary>
		public int MinimumPooledAmount => minimumPooledAmount;
	}
}