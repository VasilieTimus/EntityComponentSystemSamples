using System;
using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This abstract class is used for 3D collision events creation
	/// </summary>
	public abstract class CollisionEventListener : MonoBehaviour
	{
		protected Action<Collision> CollisionAction = delegate { };

		/// <summary>
		/// Subscribes to collision event
		/// </summary>
		/// <param name="action">Action to invoke on collision event</param>
		public void Subscribe(Action<Collision> action)
		{
			CollisionAction += action;
		}

		/// <summary>
		/// Unsubscribes from collision event 
		/// </summary>
		/// <param name="action">Action to unsubscribe</param>
		public void Unsubscribe(Action<Collision> action)
		{
			CollisionAction -= action;
		}
	}
}