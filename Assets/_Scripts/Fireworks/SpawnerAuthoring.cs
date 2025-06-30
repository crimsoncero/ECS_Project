using Unity.Entities;
using UnityEngine;

class SpawnerAuthoring : MonoBehaviour
{
    public FireworkParticleAuthoring Prefab;
    public float SpawnRate;
    public int SpawnCount;
    public Vector3 MaxVelocity;
}

class SpawnerAuthoringBaker : Baker<SpawnerAuthoring>
{
    public override void Bake(SpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new FireworkSpawner
        {
            FireworkPrefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
            SpawnPosition = authoring.transform.position,
            NextSpawnTime = 0.0f,
            SpawnRate = authoring.SpawnRate,
            SpawnCount = authoring.SpawnCount,
            MaxVelocity = authoring.MaxVelocity,
        });
    }
}
