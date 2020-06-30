using System;
using System.Collections.Generic;
using System.Linq;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GameRig.Scripts.Systems.PoolingSystem
{
	/// <summary>
	/// This class manages objects pooling.<para></para>
	/// If an object is disabled then it can be pooled.<para></para>
	/// If there are no disabled objects then this manager will create a new one.<para></para>
	/// In order to return the object to pool you simply have to disable it.<para></para>
	/// Take care of object reinitialization after getting it from the pool as it can contain some older field values.<para></para>
	/// Always initialize pooled objects in OnEnable method.
	/// <code>private void OnEnable()
	/// {
	/// 	// Initialize object here
	/// }</code>
	/// </summary>
	/// <example>
	/// Use Pooling Pattern only when an object is instantiated at least every second, 
	/// otherwise don't waste your time setting up the pooling settings
	/// </example>
	[Obsolete("This system is subject to change in the upcoming updates")]
	public static class PoolingManager
	{
		private static GameObject pooledObjectsParent;
		private static Dictionary<GameObject, List<PooledObject>> pool;
		private static List<PooledObject> pooledObjects;
		private static PooledObjectSettings[] objectsToPool;
		private static bool skipFirstTime = true;

		public static Transform PoolContainer => pooledObjectsParent.transform;

		[InitializeOnLaunch]
		private static void Initialize()
		{
			InitializePool();

			SceneManager.sceneLoaded += ResetPool;
		}

		/// <summary>
		/// Returns a pooled object
		/// </summary>
		/// <param name="gameObject">Pooled object that is ready to be used</param>
		/// <returns></returns>
		public static GameObject GetPooledObject(GameObject gameObject)
		{
			if (!pool.ContainsKey(gameObject))
			{
				pool.Add(gameObject, new List<PooledObject>());
			}

			pooledObjects = pool[gameObject];

			foreach (PooledObject pooledObject in pooledObjects.Where(pooledObject => !pooledObject.PooledGameObject.activeInHierarchy))
			{
				if (pooledObject.AvailableFrame == -1)
				{
					pooledObject.AvailableFrame = Time.frameCount + 1;
				}
				else if (Time.frameCount > pooledObject.AvailableFrame)
				{
					pooledObject.AvailableFrame = -1;

					return pooledObject.PooledGameObject;
				}
			}

			return AddObjectToPool(gameObject);
		}

		private static void InitializePool()
		{
			objectsToPool = Resources.Load<PoolingSettings>(GameRigResourcesPaths.PoolingSettings).ObjectsToPool;

			pooledObjectsParent = new GameObject("Pooled GameObjects");

			pool = new Dictionary<GameObject, List<PooledObject>>();

			foreach (PooledObjectSettings pooledObject in objectsToPool)
			{
				if (pooledObject == null)
				{
					continue;
				}

				pool.Add(pooledObject.ObjectToPool, new List<PooledObject>(pooledObject.MinimumPooledAmount));

				for (int j = 0; j < pooledObject.MinimumPooledAmount; j++)
				{
					AddObjectToPool(pooledObject.ObjectToPool);
				}
			}
		}

		private static void ResetPool(Scene scene, LoadSceneMode loadSceneMode)
		{
			if (skipFirstTime)
			{
				skipFirstTime = false;

				return;
			}

			if (loadSceneMode == LoadSceneMode.Additive)
			{
				return;
			}

			InitializePool();
		}

		private static GameObject AddObjectToPool(GameObject prefab)
		{
			prefab.SetActive(false);

			GameObject objectToPool = Object.Instantiate(prefab);

			pool[prefab].Add(new PooledObject(objectToPool));
			objectToPool.transform.SetParent(pooledObjectsParent.transform);

			prefab.SetActive(true);

			return objectToPool;
		}
	}
}