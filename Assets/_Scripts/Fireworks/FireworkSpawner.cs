using Unity.Entities;
using Unity.Mathematics;

public struct FireworkSpawner : IComponentData
{
    public Entity FireworkPrefab;
    public float3 SpawnPosition;
    public int SpawnCount;
    public float NextSpawnTime;
    public float SpawnRate;
    public float3 MaxVelocity;
}
