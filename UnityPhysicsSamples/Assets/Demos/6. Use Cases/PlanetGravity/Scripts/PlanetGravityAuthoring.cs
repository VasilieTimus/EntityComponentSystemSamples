using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public struct PlanetGravity : IComponentData
{
	public float3 GravitationalCenter;
	public float GravitationalMass;
	public float GravitationalConstant;
	public float EventHorizonDistance;
	public float RotationMultiplier;

	public float timer;
	public bool canUpdate;
}

public class PlanetGravityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
	public float GravitationalMass;
	public float GravitationalConstant;
	public float EventHorizonDistance;
	public float RotationMultiplier;

	void IConvertGameObjectToEntity.Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		var component = new PlanetGravity
		{
			GravitationalCenter = transform.position,
			GravitationalMass = GravitationalMass,
			GravitationalConstant = GravitationalConstant,
			EventHorizonDistance = EventHorizonDistance,
			RotationMultiplier = RotationMultiplier
		};
		dstManager.AddComponentData(entity, component);

		if (dstManager.HasComponent<PhysicsMass>(entity))
		{
			var bodyMass = dstManager.GetComponentData<PhysicsMass>(entity);
			var random = new Random();
			random.InitState(10);
			bodyMass.InverseMass = random.NextFloat(bodyMass.InverseMass, bodyMass.InverseMass * 4f);

			dstManager.SetComponentData(entity, bodyMass);
		}
	}
}

[UpdateBefore(typeof(BuildPhysicsWorld))]
public class PlanetGravitySystem : SystemBase
{
	static readonly quaternion k_GravityOrientation = quaternion.RotateY(math.PI / 2f);

	protected override void OnUpdate()
	{
		var dt = UnityEngine.Time.fixedDeltaTime;
		var dtSimple = UnityEngine.Time.deltaTime;

		Entities
			.WithName("ApplyGravityFromPlanet")
			.WithBurst()
			.ForEach((ref PhysicsMass bodyMass, ref PhysicsVelocity bodyVelocity, ref PlanetGravity gravity, in Translation pos) =>
			{
				float mass = math.rcp(bodyMass.InverseMass);

				float3 dir = (gravity.GravitationalCenter - pos.Value);
				float dist = math.length(dir);
				float invDist = 1.0f / dist;
				dir = math.normalize(dir);
				
				float3 xtraGravity = (gravity.RotationMultiplier * gravity.GravitationalConstant * gravity.GravitationalMass * dir) * invDist;

				if (dist < gravity.EventHorizonDistance)
				{
					if (gravity.canUpdate)
					{
						bodyVelocity.Linear += math.rotate(k_GravityOrientation, xtraGravity) * gravity.RotationMultiplier * dt;

						gravity.timer -= dtSimple;
						
						if (gravity.timer <= 0)
						{
							gravity.canUpdate = false;
						}
					}
					else
					{
						gravity.timer += dtSimple;
						xtraGravity = (gravity.GravitationalConstant * (gravity.GravitationalMass * mass) * dir) * invDist * invDist;
						bodyVelocity.Linear += xtraGravity * dt * math.sin(gravity.timer * 10f);

						if (gravity.timer >= 2f)
						{
							gravity.canUpdate = true;
						}
					}
				}
				else
				{
					bodyVelocity.Linear += xtraGravity * dt;	
				}

			}).Schedule();
	}
}