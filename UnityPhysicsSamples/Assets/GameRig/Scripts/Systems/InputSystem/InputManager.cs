using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GameRig.Scripts.Utilities.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameRig.Scripts.Systems.InputSystem
{
	/// <summary>
	/// This class handles the most common Input features
	/// </summary>
	public static class InputManager
	{
		private static Dictionary<KeyEventType, Dictionary<KeyCode, Action>> inputSubscribers;
		private static Dictionary<KeyEventType, Dictionary<KeyCode, Action>> unregistrableInputSubscribers;
		private static Vector2 currentAxesInput;
		private static Vector2 previousAxesInput;
		private static Vector2 touchStartPosition;
		private static Touch touch;
		private static Vector2 joystickInput;

#if !UNITY_EDITOR && !UNITY_STANDALONE
		private static int lastJoystickTouchFingerId = -1;
		private static int lastAxesTouchFingerId = -1;
#endif

#if !UNITY_EDITOR && !UNITY_STANDALONE
		public static bool IsTouching => Input.touchCount > 0;
#else
		public static bool IsTouching => Input.GetMouseButton(0);
#endif

		[InitializeOnLaunch]
		private static void Initialize()
		{
			InitializeFields();

			InputManagerBehaviour inputManagerBehaviour = GameRigCore.InitializeManagerBehaviour<InputManagerBehaviour>();
			inputManagerBehaviour.RegisterInputManager(Check);

			SceneManager.sceneUnloaded += OnSceneUnload;
		}

		/// <summary>
		/// Subscribes the listener for a given key event
		/// </summary>
		/// <param name="type">Key event type to listen to</param>
		/// <param name="keyCode">Key code to listen to</param>
		/// <param name="subscriberAction">Action called when the given key event is used</param>
		/// <param name="unregisterOnSceneUnload">Automatically unregister when current active scene is unloaded?</param>
		public static void Subscribe(KeyEventType type, KeyCode keyCode, Action subscriberAction, bool unregisterOnSceneUnload = true)
		{
			if (subscriberAction == null)
			{
				return;
			}

			if (inputSubscribers[type].ContainsKey(keyCode))
			{
				inputSubscribers[type][keyCode] += subscriberAction;
			}
			else
			{
				inputSubscribers[type].Add(keyCode, subscriberAction);
			}

			if (!unregisterOnSceneUnload)
			{
				if (unregistrableInputSubscribers[type].ContainsKey(keyCode))
				{
					unregistrableInputSubscribers[type][keyCode] += subscriberAction;
				}
				else
				{
					unregistrableInputSubscribers[type].Add(keyCode, subscriberAction);
				}
			}
		}

		/// <summary>
		/// Unsubscribes the listener from the given key event 
		/// </summary>
		/// <param name="type">Key event type to unsubscribe from</param>
		/// <param name="keyCode">Key code to unsubscribe from</param>
		/// <param name="subscriberAction">Action to unsubscribe from the given key event</param>
		public static void Unsubscribe(KeyEventType type, KeyCode keyCode, Action subscriberAction)
		{
			if (subscriberAction == null)
			{
				return;
			}

			if (inputSubscribers[type].ContainsKey(keyCode))
			{
				inputSubscribers[type][keyCode] -= subscriberAction;

				if (inputSubscribers[type][keyCode] == null)
				{
					inputSubscribers[type].Remove(keyCode);
				}
			}

			if (unregistrableInputSubscribers[type].ContainsKey(keyCode))
			{
				unregistrableInputSubscribers[type][keyCode] -= subscriberAction;

				if (unregistrableInputSubscribers[type][keyCode] == null)
				{
					unregistrableInputSubscribers[type].Remove(keyCode);
				}
			}
		}

		/// <summary>
		/// Simulates a key event
		/// </summary>
		/// <param name="type">Key event type</param>
		/// <param name="keyCode">Key event code</param>
		public static void SimulateKeyPress(KeyEventType type, KeyCode keyCode)
		{
			if (inputSubscribers[type].ContainsKey(keyCode))
			{
				inputSubscribers[type].Single(s => s.Key == keyCode).Value();
			}
		}

		/// <summary>
		/// Returns the current normalized joystick direction
		/// </summary>
		/// <remarks>
		/// Joystick position is automatically set on touch Start state
		/// and the direction is calculated from the start position until
		/// the touch state is changed to End
		/// </remarks>
		/// <returns>Normalized Vector2 of current vector direction</returns>
		public static Vector2 GetJoystickInput()
		{
			return joystickInput;
		}

		/// <summary>
		/// Returns the input delta since the previous frame
		/// </summary>
		/// <remarks>
		/// This method returns values from -1 to 1 which means
		/// -1 - the touch moved from right/top margin to left/bottom margin in one frame and
		/// 1 - the touch moved from left/bottom margin to right/top margin in one frame
		/// </remarks>
		/// <returns>Returns a Vector2 that represents how much last touch moved since previous frame in Viewport space</returns>
		public static Vector2 GetAxesDelta()
		{
			return (currentAxesInput - previousAxesInput) * 100f;
		}

		/// <summary>
		/// Returns the current touch horizontal position
		/// </summary>
		/// <remarks>
		/// This method returns values from -1 to 1 which means -1 - left margin and 1 - right margin
		/// </remarks>
		/// <returns>Returns a float that represents the touch horizontal position</returns>
		public static float GetCenteredHorizontalAxisValue()
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			return Input.GetAxis("Horizontal");
#else
			return Input.touchCount > 0 ? 2f * (Input.GetTouch(0).position.x / Screen.width) - 1f : 0f;
#endif
		}

		/// <summary>
		/// Returns the current touch vertical position
		/// </summary>
		/// <remarks>
		/// This method returns values from -1 to 1 which means -1 - bottom margin and 1 - top margin
		/// </remarks>
		/// <returns>Returns a float that represents the touch vertical position</returns>
		public static float GetCenteredVerticalAxisValue()
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			return Input.GetAxis("Vertical");
#else
			return Input.touchCount > 0 ? 2f * (Input.GetTouch(0).position.y / Screen.height) - 1f : 0f;
#endif
		}

		/// <summary>
		/// Returns the current touch position in viewport space
		/// </summary>
		/// <remarks>
		/// This method returns values from 0 to 1 which means 0 - the left/bottom margins and 1 - right/top margins
		/// </remarks>
		/// <returns>Returns a Vector2 that represents the current touch position in Viewport space</returns>
		public static Vector2 GetScreenTouchPosition()
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			return new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height) * 100f;
#else
			return Input.touchCount > 0 ? new Vector2(Input.GetTouch(0).position.x / Screen.width, Input.GetTouch(0).position.y / Screen.height) * 100f : Vector2.zero;
#endif
		}

		private static void Check()
		{
			CheckJoystickInput();
			CheckAxesInput();
			CheckActionsList();
		}

		private static void CheckAxesInput()
		{
			previousAxesInput = currentAxesInput;

#if UNITY_EDITOR || UNITY_STANDALONE
			if (Input.GetMouseButtonDown(0))
			{
				previousAxesInput = currentAxesInput = GetScreenTouchPosition();
			}

			if (Input.GetMouseButton(0))
			{
				currentAxesInput = GetScreenTouchPosition();
			}
#else
			if (Input.touchCount==0)
			{
				return;
			}

			Touch currentTouch = Input.GetTouch(0);

			if (lastAxesTouchFingerId != currentTouch.fingerId)
			{
				lastAxesTouchFingerId = currentTouch.fingerId;

				previousAxesInput = currentAxesInput = GetScreenTouchPosition();
			}

			switch (currentTouch.phase)
			{
				case TouchPhase.Began:
					previousAxesInput = currentAxesInput = GetScreenTouchPosition();

					break;
				case TouchPhase.Moved:
					currentAxesInput = GetScreenTouchPosition();

					break;
			}
#endif
		}

		private static void CheckJoystickInput()
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			joystickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#else
			joystickInput = Vector2.zero;

			if (Input.touchCount > 0)
			{
				if (lastJoystickTouchFingerId == -1)
				{
					touch = Input.GetTouch(0);

					lastJoystickTouchFingerId = touch.fingerId;

					touchStartPosition = touch.position;
				}
				else
				{
					for (int i = 0; i < Input.touches.Length; i++)
					{
						if (Input.GetTouch(i).fingerId == lastJoystickTouchFingerId)
						{
							touch = Input.GetTouch(i);
						}
					}

					if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
					{
						lastJoystickTouchFingerId = -1;

						return;
					}

					joystickInput = new Vector2
					(
						(touch.position.x - touchStartPosition.x) / Screen.width,
						(touch.position.y - touchStartPosition.y) / Screen.height
					);

					joystickInput.x *= (float) Screen.width / Screen.height;

					if (joystickInput.magnitude > 0.2f)
					{
						joystickInput *= 0.2f / joystickInput.magnitude;
					}
				}
			}
#endif
		}

		[SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
		private static void CheckActionsList()
		{
			Action actionsList = delegate { };

			foreach (KeyValuePair<KeyEventType, Dictionary<KeyCode, Action>> subscribers in inputSubscribers)
			{
				if (subscribers.Value.Count > 0)
				{
					foreach (KeyValuePair<KeyCode, Action> inputSubscriber in subscribers.Value)
					{
						if (subscribers.Key == KeyEventType.KeyDown && !Input.GetKeyDown(inputSubscriber.Key) ||
						    subscribers.Key == KeyEventType.KeyHold && !Input.GetKey(inputSubscriber.Key) ||
						    subscribers.Key == KeyEventType.KeyUp && !Input.GetKeyUp(inputSubscriber.Key))
						{
							continue;
						}

						actionsList += inputSubscriber.Value;
					}
				}
			}

			actionsList();
		}

		private static void OnSceneUnload(Scene arg0)
		{
			InitializeFields();
		}

		private static void InitializeFields()
		{
			bool initializeUnregistrableSubscribers = unregistrableInputSubscribers == null;

			IEnumerable<KeyEventType> inputEventTypes;

			if (initializeUnregistrableSubscribers)
			{
				if (initializeUnregistrableSubscribers)
				{
					unregistrableInputSubscribers = new Dictionary<KeyEventType, Dictionary<KeyCode, Action>>();
				}

				inputEventTypes = Enum.GetValues(typeof(KeyEventType)).Cast<KeyEventType>();

				foreach (KeyEventType inputEventType in inputEventTypes)
				{
					unregistrableInputSubscribers.Add(inputEventType, new Dictionary<KeyCode, Action>());
				}
			}

			inputEventTypes = Enum.GetValues(typeof(KeyEventType)).Cast<KeyEventType>();

			inputSubscribers = new Dictionary<KeyEventType, Dictionary<KeyCode, Action>>();

			foreach (KeyEventType inputEventType in inputEventTypes)
			{
				inputSubscribers.Add(inputEventType, new Dictionary<KeyCode, Action>());
			}

			foreach (KeyValuePair<KeyEventType, Dictionary<KeyCode, Action>> unregistrableInputSubscriber in unregistrableInputSubscribers)
			{
				foreach (KeyValuePair<KeyCode, Action> keyValuePair in unregistrableInputSubscriber.Value)
				{
					inputSubscribers[unregistrableInputSubscriber.Key][keyValuePair.Key] = keyValuePair.Value;
				}
			}

#if !UNITY_EDITOR&& !UNITY_STANDALONE
			lastJoystickTouchFingerId = -1;
			lastAxesTouchFingerId = -1;
#endif

			joystickInput = previousAxesInput = currentAxesInput = Vector2.zero;
		}
	}
}