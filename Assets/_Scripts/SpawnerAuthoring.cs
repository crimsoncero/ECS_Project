using Unity.Entities;
using UnityEngine;

class SpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float SpawnRate;
}

class SpawnerAuthoringBaker : Baker<SpawnerAuthoring>
{
    public override void Bake(SpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new SpawnerComponent
        {
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
            SpawnPosition = authoring.transform.position,
            NextSpawnTime = 0.0f,
            SpawnRate = authoring.SpawnRate
        });
    }
}
