using Unity.Entities;
using Unity.Mathematics;

public struct FireworkParticle : IComponentData
{
    public float3 Velocity;
    public float Gravity;
    public float Lifetime;

    public float3 VelocityDrag;
}
