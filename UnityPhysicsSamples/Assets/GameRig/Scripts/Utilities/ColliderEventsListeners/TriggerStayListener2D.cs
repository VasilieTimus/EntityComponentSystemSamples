using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the 2D trigger stay events
	/// </summary>
	public class TriggerStayListener2D : TriggerEventListener2D
	{
		private void OnTriggerStay2D(Collider2D other)
		{
			TriggerAction2D(other);
		}
	}
}