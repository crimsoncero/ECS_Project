using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct SpawnerSystem : ISystem
{
    private Random rnd;
    
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        rnd = new Random(1);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var spawner in SystemAPI.Query<RefRW<SpawnerComponent>>())
        {
            ProcessSpawner(ref state, spawner);
        }
    }

    private void ProcessSpawner(ref SystemState state, RefRW<SpawnerComponent> spawner)
    {
        
        
        if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            var newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
            state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(spawner.ValueRO.SpawnPosition));
            spawner.ValueRW.NextSpawnTime = (float) SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate;
            state.EntityManager.SetComponentData(newEntity, MoveForward.FromSeed(rnd.NextUInt()));
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
