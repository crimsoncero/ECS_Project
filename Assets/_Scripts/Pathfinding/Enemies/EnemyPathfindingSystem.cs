using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemyPathfindingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        RefRO<LocalTransform> playerTransform = default;
        foreach (var p in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>())
        {
            playerTransform = p;
        }
        
        foreach (var (enemy, enemyTransform) in SystemAPI.Query<RefRO<Enemy>, RefRW<LocalTransform>>())
        {
            float3 moveDirection = playerTransform.ValueRO.Position - enemyTransform.ValueRO.Position;
            moveDirection = math.normalize(moveDirection);
            enemyTransform.ValueRW.Position += moveDirection * enemy.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;

        }
        
    }

}
