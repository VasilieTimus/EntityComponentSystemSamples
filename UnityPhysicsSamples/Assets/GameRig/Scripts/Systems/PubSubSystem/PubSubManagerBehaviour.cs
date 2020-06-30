using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRig.Scripts.Systems.PubSubSystem
{
	/// <summary>
	/// This class handles coroutine event listeners.
	/// <para>
	/// It is handled by <see cref="PubSubManager"/> and should not be handled manually
	/// </para>
	/// </summary>
	public class PubSubManagerBehaviour : MonoBehaviour
	{
		private readonly Dictionary<Type, int> events = new Dictionary<Type, int>();

		internal void PublishEvent<T>()
		{
			if (!events.ContainsKey(typeof(T)))
			{
				return;
			}

			events[typeof(T)] = 0;
		}

		internal Coroutine RegisterListener<T>()
		{
			if (!events.ContainsKey(typeof(T)))
			{
				events.Add(typeof(T), 1);
			}
			else
			{
				events[typeof(T)]++;
			}

			return StartCoroutine(WaitForEventCoroutine<T>());
		}

		private IEnumerator WaitForEventCoroutine<T>()
		{
			while (!events.ContainsKey(typeof(T)) || events[typeof(T)] > 0)
			{
				yield return null;
			}
		}
	}
}