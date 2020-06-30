using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the trigger stay events
	/// </summary>
	public class TriggerStayListener : TriggerEventListener
	{
		private void OnTriggerStay(Collider other)
		{
			TriggerAction(other);
		}
	}
}