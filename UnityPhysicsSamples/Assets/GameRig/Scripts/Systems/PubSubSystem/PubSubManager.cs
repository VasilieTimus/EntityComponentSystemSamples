using System;
using System.Collections.Generic;
using GameRig.Scripts.Utilities.Attributes;
using UnityEngine;

namespace GameRig.Scripts.Systems.PubSubSystem
{
	/// <summary>
	/// This class implements Publish Subscribe design pattern which allows loose coupling between the applications components.<para></para>
	/// Here senders of messages, called publishers, do not program the messages to be sent directly to specific receivers, called subscribers.<para></para>
	/// Messages are published without the knowledge of what or if any subscriber of that knowledge exists. 
	/// </summary>
	public static class PubSubManager
	{
		private static readonly Dictionary<Type, Delegate> ParameterizedSubscribers = new Dictionary<Type, Delegate>();
		private static readonly Dictionary<Type, Action> NonParameterizedSubscribers = new Dictionary<Type, Action>();

		private static PubSubManagerBehaviour coroutinesManager;

		/// <summary>
		/// This delegate is used to create parameterized subscribers
		/// </summary>
		/// <param name="data">Data to be shared</param>
		/// <typeparam name="T">Event type</typeparam>
		public delegate void ParameterizedAction<in T>(T data);

		[InitializeOnLaunch]
		private static void Initialize()
		{
			coroutinesManager = GameRigCore.InitializeManagerBehaviour<PubSubManagerBehaviour>();
		}

		/// <summary>
		/// Registers a listener parameterless action for the given event type
		/// </summary>
		/// <param name="action">Action to register</param>
		/// <typeparam name="T">Event type</typeparam>
		public static void RegisterListener<T>(Action action)
		{
			Type type = typeof(T);

			if (!NonParameterizedSubscribers.ContainsKey(type))
			{
				NonParameterizedSubscribers.Add(type, action);
			}
			else
			{
				NonParameterizedSubscribers[type] += action;
			}
		}

		/// <summary>
		/// Registers a listener parameterized action for the given event type
		/// </summary>
		/// <param name="action">Action to register</param>
		/// <typeparam name="T">Event type</typeparam>
		public static void RegisterListener<T>(ParameterizedAction<T> action)
		{
			Type type = typeof(T);

			if (ParameterizedSubscribers.TryGetValue(type, out var currentDelegate))
			{
				ParameterizedSubscribers[type] = Delegate.Combine(currentDelegate, action);
			}
			else
			{
				ParameterizedSubscribers.Add(type, action);
			}
		}

		/// <summary>
		/// Registers a coroutine event listener
		/// </summary>
		/// <remarks>
		/// Coroutine events should be only empty structs
		/// </remarks>
		/// <example>
		/// It can be used to yield a coroutine until the specified event type is triggered
		/// <code>
		/// private IEnumerator ExampleCoroutine()
		/// {
		/// 	// Some logic
		///
		/// 	....
		///
		/// 	// Wait until the event is triggered
		/// 	yield PubSubManager.RegisterCoroutineListener&lt;ExampleEvent&gt;();
		///
		/// 	....
		/// 
		/// 	// Some logic
		/// }
		/// </code>
		/// </example>
		/// <typeparam name="T">Event type</typeparam>
		/// <returns>Returns the coroutine itself</returns>
		public static Coroutine RegisterCoroutineListener<T>()
		{
			return coroutinesManager.RegisterListener<T>();
		}

		/// <summary>
		/// Unregisters the listener parameterless action for the given event type
		/// </summary>
		/// <param name="action">Action to unregister</param>
		/// <typeparam name="T">Event type</typeparam>
		public static void UnregisterListener<T>(Action action)
		{
			Type type = typeof(T);

			if (NonParameterizedSubscribers.ContainsKey(type))
			{
				NonParameterizedSubscribers[type] -= action;

				if (NonParameterizedSubscribers[type] == null)
				{
					NonParameterizedSubscribers.Remove(type);
				}
			}
		}

		/// <summary>
		/// Unregisters the listener parameterized action for the given event type
		/// </summary>
		/// <param name="action">Action to unregister</param>
		/// <typeparam name="T">Event type</typeparam>
		public static void UnregisterListener<T>(ParameterizedAction<T> action)
		{
			Type type = typeof(T);

			if (ParameterizedSubscribers.TryGetValue(type, out var currentDelegate))
			{
				ParameterizedSubscribers[type] = Delegate.Remove(currentDelegate, action);

				if (ParameterizedSubscribers[type] == null)
				{
					ParameterizedSubscribers.Remove(type);
				}
			}
		}

		/// <summary>
		/// Publishes the specified event type
		/// </summary>
		/// <param name="eventToPublish">The event itself with possibility to add shareable data in it</param>
		/// <typeparam name="T">Event type</typeparam>
		public static void Publish<T>(in T eventToPublish)
		{
			Type type = typeof(T);

			if (ParameterizedSubscribers.ContainsKey(type))
			{
				((ParameterizedAction<T>) ParameterizedSubscribers[type])(eventToPublish);
			}

			if (NonParameterizedSubscribers.ContainsKey(type))
			{
				NonParameterizedSubscribers[type]();
			}
		}

		/// <summary>
		/// Publishes coroutine event type
		/// </summary>
		/// <typeparam name="T">Event type</typeparam>
		public static void PublishCoroutineEvent<T>()
		{
			coroutinesManager.PublishEvent<T>();
		}
	}
}