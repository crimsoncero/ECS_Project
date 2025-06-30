using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var input = new float3(horizontal, vertical, 0);

        if (input.Equals(float3.zero))
            return;

        foreach (var (playerTransform, player) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Player>>())
        {
            playerTransform.ValueRW.Position += input * player.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
