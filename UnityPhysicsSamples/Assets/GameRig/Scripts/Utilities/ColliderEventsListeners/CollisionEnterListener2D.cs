using UnityEngine;

namespace GameRig.Scripts.Utilities.ColliderEventsListeners
{
	/// <summary>
	/// This class is used to handle the 2D collision enter events
	/// </summary>
	public class CollisionEnterListener2D : CollisionEventListener2D
	{
		private void OnCollisionEnter2D(Collision2D other)
		{
			CollisionAction2D(other);
		}
	}
}