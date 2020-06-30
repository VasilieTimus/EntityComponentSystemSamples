using System;
using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This abstract class is used for 2D trigger events creation
	/// </summary>
	public abstract class TriggerEventListener2D : MonoBehaviour
	{
		protected Action<Collider2D> TriggerAction2D = delegate { };

		/// <summary>
		/// Subscribes to trigger event
		/// </summary>
		/// <param name="action">Action to invoke on trigger event</param>
		public void Subscribe(Action<Collider2D> action)
		{
			TriggerAction2D += action;
		}

		/// <summary>
		/// Unsubscribes from trigger event 
		/// </summary>
		/// <param name="action">Action to unsubscribe</param>
		public void Unsubscribe(Action<Collider2D> action)
		{
			TriggerAction2D -= action;
		}
	}
}