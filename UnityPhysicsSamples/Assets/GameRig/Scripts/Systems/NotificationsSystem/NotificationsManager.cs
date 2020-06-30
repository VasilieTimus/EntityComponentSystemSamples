#if UNITY_IOS
using System;
using UnityEngine.iOS;

#endif

namespace GameRig.Scripts.Systems.NotificationsSystem
{
	/// <summary>
	/// This class handles the Local Notifications
	/// </summary>
	/// <remarks>
	/// Only available for iOS
	/// </remarks>
	public static class NotificationsManager
	{
		/// <summary>
		/// Triggers native popup which asks user for permission to create notifications
		/// </summary>
		public static void RegisterForNotifications()
		{
#if UNITY_IOS
			NotificationServices.RegisterForNotifications((NotificationType) 7);
#endif
		}

		/// <summary>
		/// Clears all of the created Local Notifications wherever they're showed or are still waiting to be triggered
		/// </summary>
		public static void ClearLocalNotifications()
		{
#if UNITY_IOS
			NotificationServices.ClearLocalNotifications();
			NotificationServices.CancelAllLocalNotifications();
#elif UNITY_ANDROID
#endif
		}

		/// <summary>
		/// Schedules a Local Notification based on given settings
		/// </summary>
		/// <param name="settings">Notification Settings used to create a Local Notification</param>
		public static void ScheduleNotification(NotificationSettings settings)
		{
#if UNITY_IOS
			if (NotificationServices.enabledNotificationTypes == NotificationType.None)
			{
				RegisterForNotifications();
			}

			DateTime dateTime = DateTime.Now;

			dateTime = dateTime.AddHours(settings.AdditionalHours);

			LocalNotification notification = new LocalNotification
			{
				alertAction = settings.ActionText,
				alertBody = settings.BodyText,
				fireDate = dateTime
			};

			NotificationServices.ScheduleLocalNotification(notification);
#elif UNITY_ANDROID
#endif
		}
	}
}