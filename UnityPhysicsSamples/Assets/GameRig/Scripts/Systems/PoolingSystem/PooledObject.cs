using UnityEngine;

namespace GameRig.Scripts.Systems.PoolingSystem
{
	/// <summary>
	/// This class is used by <see cref="PoolingManager"/> to manage what objects should be pulled next 
	/// </summary>
	public class PooledObject
	{
		/// <summary>
		/// The pulled object
		/// </summary>
		public GameObject PooledGameObject { get; }

		/// <summary>
		/// The frame when the <see cref="PooledGameObject"/> will become pullable
		/// </summary>
		public int AvailableFrame { get; set; }

		/// <summary>
		/// This is the class constructor
		/// </summary>
		/// <param name="gameObject">Object to pool</param>
		public PooledObject(GameObject gameObject)
		{
			PooledGameObject = gameObject;
			AvailableFrame = 0;
		}
	}
}