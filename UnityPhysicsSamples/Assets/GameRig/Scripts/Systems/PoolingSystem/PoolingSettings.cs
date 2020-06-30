using UnityEngine;

namespace GameRig.Scripts.Systems.PoolingSystem
{
	/// <summary>
	/// This class contains the main settings for <see cref="PoolingManager"/>
	/// </summary>
	public class PoolingSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("The list of pullable objects settings")]
		private PooledObjectSettings[] objectsToPool;

		/// <summary>
		/// Gets the list of pullable objects settings
		/// </summary>
		public PooledObjectSettings[] ObjectsToPool => objectsToPool;
	}
}