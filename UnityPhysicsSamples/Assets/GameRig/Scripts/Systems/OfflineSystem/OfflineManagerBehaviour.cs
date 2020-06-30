using System;
using UnityEngine;

namespace GameRig.Scripts.Systems.OfflineSystem
{
	/// <summary>
	/// This class notifies <see cref="OfflineManager"/> when the application goes on background
	/// </summary>
	public class OfflineManagerBehaviour : MonoBehaviour
	{
		public Action goToOfflineAction = delegate { };
		public Action returnToOnlineAction = delegate { };

		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				returnToOnlineAction();
			}
			else
			{
				goToOfflineAction();
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				goToOfflineAction();
			}
			else
			{
				returnToOnlineAction();
			}
		}
	}
}