using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Transforms;

partial struct FireworkSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var particle in SystemAPI.Query<RefRW<LocalTransform>,RefRW<FireworkParticle>>())
        {
            particle.Item1.ValueRW.Position += particle.Item2.ValueRO.Velocity * SystemAPI.Time.DeltaTime;

            // Shrink the particle
            particle.Item1.ValueRW.Scale -= (1 / particle.Item2.ValueRO.Lifetime) * SystemAPI.Time.DeltaTime;
            
            // Lower Velocity due to drag.
            particle.Item2.ValueRW.Velocity -= particle.Item2.ValueRO.VelocityDrag * SystemAPI.Time.DeltaTime;

            // Increase downward Velocity due to gravity
            particle.Item2.ValueRW.Velocity.y -= particle.Item2.ValueRO.Gravity * SystemAPI.Time.DeltaTime;
        }

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        
        
        foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<FireworkParticle>().WithEntityAccess())
        {
            if (transform.ValueRO.Scale <= 0f)
            {
                ecb.DestroyEntity(entity);
            }
        }
    }

    
    
}
