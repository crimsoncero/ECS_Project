using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct MoveForwardSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        
        foreach (var (move, transform) in SystemAPI.Query<RefRO<MoveForward>, RefRW<LocalTransform>>())
        {
            transform.ValueRW.Position += move.ValueRO.Direction * move.ValueRO.Speed * deltaTime;
        }

    }
}