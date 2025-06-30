using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct SpawnerSystem : ISystem
{
    private Random _random;
    
    public void OnCreate(ref SystemState state)
    {
        _random = new Random((uint)System.DateTime.Now.Millisecond);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(RefRW<FireworkSpawner> spawner in SystemAPI.Query<RefRW<FireworkSpawner>>())
        {
            ProcessSpawner(ref state, spawner);
        }
    }

    private void ProcessSpawner(ref SystemState state, RefRW<FireworkSpawner> spawner)
    {
        if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            for (int i = 0; i < spawner.ValueRO.SpawnCount; i++)
            {
                Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.FireworkPrefab);
                
                state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(spawner.ValueRO.SpawnPosition));
                var fireworkParticle = state.EntityManager.GetComponentData<FireworkParticle>(newEntity);
                fireworkParticle.Velocity =
                    _random.NextFloat3(-spawner.ValueRO.MaxVelocity, spawner.ValueRO.MaxVelocity);
                fireworkParticle.VelocityDrag = fireworkParticle.Velocity / fireworkParticle.Lifetime;
                
                state.EntityManager.SetComponentData(newEntity, fireworkParticle);

                spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate;
            }
        
        }
    }
    
}
