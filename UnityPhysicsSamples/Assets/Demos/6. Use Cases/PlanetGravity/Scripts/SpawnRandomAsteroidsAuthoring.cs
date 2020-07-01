﻿using System;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

class SpawnRandomAsteroidsAuthoring : SpawnRandomObjectsAuthoringBase<AsteroidSpawnSettings>
{
    public float massFactor = 1;

    internal override void Configure(ref AsteroidSpawnSettings spawnSettings) => spawnSettings.MassFactor = massFactor;
}

struct AsteroidSpawnSettings : IComponentData, ISpawnSettings
{
    public Entity Prefab { get; set; }
    public float3 Position { get; set; }
    public float3 Range { get; set; }
    public int Count { get; set; }
    public float MassFactor;
    public float RotationMultiplier { get; set; }
}

class SpawnRandomAsteroidsSystem : SpawnRandomObjectsSystemBase<AsteroidSpawnSettings>
{

    Random m_RandomMass;

    internal override int GetRandomSeed(AsteroidSpawnSettings spawnSettings)
    {
        var seed = base.GetRandomSeed(spawnSettings);
        seed = (seed * 397) ^ spawnSettings.Prefab.GetHashCode();
        seed = (seed * 397) ^ (int)(spawnSettings.MassFactor * 1000);
        return seed;
    }

    internal override void OnBeforeInstantiatePrefab(AsteroidSpawnSettings spawnSettings)
    {
        m_RandomMass = new Random();
        m_RandomMass.InitState(10);
    }

    internal override void ConfigureInstance(Entity instance, AsteroidSpawnSettings spawnSettings)
    {
        var mass = EntityManager.GetComponentData<PhysicsMass>(instance);
        var halfMassFactor = spawnSettings.MassFactor * 0.5f;
        mass.InverseMass = m_RandomMass.NextFloat(mass.InverseMass * math.rcp(halfMassFactor), mass.InverseMass * halfMassFactor);
        EntityManager.SetComponentData(instance, mass);
        
        var planetGravity = EntityManager.GetComponentData<PlanetGravity>(instance);
        planetGravity.RotationMultiplier = spawnSettings.RotationMultiplier;
        planetGravity.timer = 5f; 
        
        EntityManager.SetComponentData(instance, planetGravity);
    }
}
