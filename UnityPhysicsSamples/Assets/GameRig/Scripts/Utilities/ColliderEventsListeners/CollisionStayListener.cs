using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the collision stay events
	/// </summary>
	public class CollisionStayListener : CollisionEventListener
	{
		private void OnCollisionStay(Collision other)
		{
			CollisionAction(other);
		}
	}
}