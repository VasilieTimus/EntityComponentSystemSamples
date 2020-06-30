using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Collider = Unity.Physics.Collider;

namespace HelloCube._1._ForEach
{
	public class SpawnRandomPhysicsBodies : MonoBehaviour
	{
		public GameObject prefab;
		public float3 range;
		public int count;

		void OnEnable() { }

		public static void RandomPointsOnCircle(float3 center, float3 range, ref NativeArray<float3> positions, ref NativeArray<quaternion> rotations)
		{
			var count = positions.Length;
			// initialize the seed of the random number generator 
			Unity.Mathematics.Random random = new Unity.Mathematics.Random();
			random.InitState(10);
			for (int i = 0; i < count; i++)
			{
				positions[i] = center + random.NextFloat3(-range, range);
				rotations[i] = random.NextQuaternionRotation();
			}
		}

		void Start()
		{
			if (!enabled) return;

			// Create entity prefab from the game object hierarchy once
			Entity sourceEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, World.Active);
			var entityManager = World.Active.EntityManager;

			var positions = new NativeArray<float3>(count, Allocator.Temp);
			var rotations = new NativeArray<quaternion>(count, Allocator.Temp);
			RandomPointsOnCircle(transform.position, range, ref positions, ref rotations);

			BlobAssetReference<Collider> sourceCollider = entityManager.GetComponentData<PhysicsB>(sourceEntity).Value;
			for (int i = 0; i < count; i++)
			{
				var instance = entityManager.Instantiate(sourceEntity);
				entityManager.SetComponentData(instance, new Translation { Value = positions[i] });
				entityManager.SetComponentData(instance, new Rotation { Value = rotations[i] });
				entityManager.SetComponentData(instance, new PhysicsCollider { Value = sourceCollider });
			}

			positions.Dispose();
			rotations.Dispose();
		}
	}
}
