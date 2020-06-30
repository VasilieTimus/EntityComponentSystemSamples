using System;
using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This abstract class is used for trigger events creation
	/// </summary>
	public abstract class TriggerEventListener : MonoBehaviour
	{
		protected Action<Collider> TriggerAction = delegate { };

		/// <summary>
		/// Subscribes to trigger event
		/// </summary>
		/// <param name="action">Action to invoke on trigger event</param>
		public void Subscribe(Action<Collider> action)
		{
			TriggerAction += action;
		}

		/// <summary>
		/// Unsubscribes from trigger event 
		/// </summary>
		/// <param name="action">Action to unsubscribe</param>
		public void Unsubscribe(Action<Collider> action)
		{
			TriggerAction -= action;
		}
	}
}