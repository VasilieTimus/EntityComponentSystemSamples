using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the 2D collision stay events
	/// </summary>
	public class CollisionStayListener2D : CollisionEventListener2D
	{
		private void OnCollisionStay2D(Collision2D other)
		{
			CollisionAction2D(other);
		}
	}
}