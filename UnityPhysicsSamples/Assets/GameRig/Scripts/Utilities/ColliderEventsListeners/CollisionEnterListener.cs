using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the collision enter events
	/// </summary>
	public class CollisionEnterListener : CollisionEventListener
	{
		private void OnCollisionEnter(Collision other)
		{
			CollisionAction(other);
		}
	}
}