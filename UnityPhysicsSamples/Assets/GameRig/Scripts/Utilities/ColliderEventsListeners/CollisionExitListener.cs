using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the collision exit events
	/// </summary>
	public class CollisionExitListener : CollisionEventListener
	{
		private void OnCollisionExit(Collision other)
		{
			CollisionAction(other);
		}
	}
}