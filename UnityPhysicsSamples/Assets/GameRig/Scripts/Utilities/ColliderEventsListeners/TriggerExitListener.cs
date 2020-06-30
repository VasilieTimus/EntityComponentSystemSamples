using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the trigger exit events
	/// </summary>
	public class TriggerExitListener : TriggerEventListener
	{
		private void OnTriggerExit(Collider other)
		{
			TriggerAction(other);
		}
	}
}