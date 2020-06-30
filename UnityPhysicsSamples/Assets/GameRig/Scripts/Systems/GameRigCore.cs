using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using GameRig.Scripts.Utilities.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameRig.Scripts.Systems
{
	/// <summary>
	/// This class handles all of the game systems and their initialization order
	/// </summary>
	public static class GameRigCore
	{
		private const string ManagersBehavioursParentName = "Systems Managers Behaviours";
		private static Transform behavioursHolder;

		/// <summary>
		/// Instantiates an undestroyable on load <see cref="GameObject"/> which has the specified component attached to it 
		/// </summary>
		/// <typeparam name="T">Component type</typeparam>
		/// <returns>The attached component</returns>
		public static T InitializeManagerBehaviour<T>() where T : MonoBehaviour
		{
			string managerBehaviourName = Regex.Replace(typeof(T).Name, "(\\B[A-Z])", " $1");

			GameObject newBehaviourGameObject = new GameObject(managerBehaviourName);
			newBehaviourGameObject.transform.SetParent(behavioursHolder);

			return newBehaviourGameObject.AddComponent<T>();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			CreateManagersBehavioursHolder();
			InitializeManagers();
		}

		private static void CreateManagersBehavioursHolder()
		{
			behavioursHolder = new GameObject(ManagersBehavioursParentName).transform;
			Object.DontDestroyOnLoad(behavioursHolder.gameObject);
		}

		private static void InitializeManagers()
		{
			Assembly defaultAssembly = Assembly.Load("Assembly-CSharp");

			MethodInfo[] methods = defaultAssembly.GetTypes()
				.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static))
				.Where(m => m.GetCustomAttributes(typeof(InitializeOnLaunch), false).Length > 0)
				.ToArray();

			for (int i = 0; i < methods.Length - 1; i++)
			{
				MethodInfo currentMethodInfo = methods[i];

				for (int j = methods.Length - 1; j > i; j--)
				{
					MethodInfo otherMethodInfo = methods[j];

					Type[] dependencies = currentMethodInfo.GetCustomAttributes(typeof(InitializeOnLaunch), false).OfType<InitializeOnLaunch>().First().Dependencies;

					if (dependencies.Contains(otherMethodInfo.DeclaringType))
					{
						// Check if Reciprocal Interdependence is present xD
						if (otherMethodInfo.GetCustomAttributes(typeof(InitializeOnLaunch), false).OfType<InitializeOnLaunch>().First().Dependencies.Contains(currentMethodInfo.DeclaringType))
						{
							Debug.LogError("Reciprocal Interdependence between " + currentMethodInfo.DeclaringType?.Name + " and " + otherMethodInfo.DeclaringType?.Name);
						}
						else
						{
							methods[i] = otherMethodInfo;
							methods[j] = currentMethodInfo;

							i--;

							break;
						}
					}
				}
			}

			foreach (MethodInfo methodInfo in methods)
			{
				methodInfo.Invoke(null, null);
			}
		}
	}
}