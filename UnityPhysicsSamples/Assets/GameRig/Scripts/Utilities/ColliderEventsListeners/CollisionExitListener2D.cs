using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the 2D collision exit events
	/// </summary>
	public class CollisionExitListener2D : CollisionEventListener2D
	{
		private void OnCollisionExit2D(Collision2D other)
		{
			CollisionAction2D(other);
		}
	}
}