using UnityEngine;

namespace GameRig.Scripts.Systems.NotificationsSystem
{
	/// <summary>
	/// This settings are used to setup a notification that is handled by the <see cref="NotificationsManager"/>
	/// </summary>
	public class NotificationSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("The notification title text")]
		private string actionText;

		[SerializeField] [Tooltip("The notification body text")]
		private string bodyText;

		[SerializeField] [Tooltip("Notification delay in hours")]
		private int additionalHours;

		/// <summary>
		/// Gets the notification title text
		/// </summary>
		public string ActionText => actionText;

		/// <summary>
		/// Gets the notification body text
		/// </summary>
		public string BodyText => bodyText;

		/// <summary>
		/// Gets the notification delay in hours
		/// </summary>
		public int AdditionalHours => additionalHours;
	}
}