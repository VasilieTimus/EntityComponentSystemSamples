using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the 2D trigger exit events
	/// </summary>
	public class TriggerExitListener2D : TriggerEventListener2D
	{
		private void OnTriggerExit2D(Collider2D other)
		{
			TriggerAction2D(other);
		}
	}
}