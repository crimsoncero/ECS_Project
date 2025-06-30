using Unity.Entities;
using UnityEngine;

class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed;
}

class PlayerAuthoringBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new Player
        {
            MoveSpeed = authoring.MoveSpeed,
        });
    }
}
