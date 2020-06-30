using System;
using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This abstract class is used for 2D collision events creation
	/// </summary>
	public abstract class CollisionEventListener2D : MonoBehaviour
	{
		protected Action<Collision2D> CollisionAction2D = delegate { };

		/// <summary>
		/// Subscribes to collision event
		/// </summary>
		/// <param name="action">Action to invoke on collision event</param>
		public void Subscribe(Action<Collision2D> action)
		{
			CollisionAction2D += action;
		}

		/// <summary>
		/// Unsubscribes from collision event 
		/// </summary>
		/// <param name="action">Action to unsubscribe</param>
		public void Unsubscribe(Action<Collision2D> action)
		{
			CollisionAction2D -= action;
		}
	}
}