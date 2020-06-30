using System;
using UnityEngine;

namespace GameRig.Scripts.Systems.InputSystem
{
	/// <summary>
	/// This class handles the Update loop of Input Manager
	/// </summary>
	public class InputManagerBehaviour : MonoBehaviour
	{
		private Action onUpdate = delegate { };

		private void Update()
		{
			onUpdate();
		}

		public void RegisterInputManager(Action action)
		{
			onUpdate += action;
		}
	}
}