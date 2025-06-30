using Unity.Entities;
using UnityEngine;

class EnemyAuthoring : MonoBehaviour
{
    public float MoveSpeed;
}

class EnemyAuthoringBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new Enemy
        {
            MoveSpeed = authoring.MoveSpeed,
        });
    }
}
