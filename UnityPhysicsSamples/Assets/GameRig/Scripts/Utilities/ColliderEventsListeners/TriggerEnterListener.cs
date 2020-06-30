using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the trigger enter events
	/// </summary>
	public class TriggerEnterListener : TriggerEventListener
	{
		private void OnTriggerEnter(Collider other)
		{
			TriggerAction(other);
		}
	}
}