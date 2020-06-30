using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the 2D trigger enter events
	/// </summary>
	public class TriggerEnterListener2D : TriggerEventListener2D
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			TriggerAction2D(other);
		}
	}
}