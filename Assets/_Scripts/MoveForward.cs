using Unity.Entities;
using Unity.Mathematics;

public struct MoveForward : IComponentData
{
    public uint Seed;
    public float3 Direction;
    public float Speed;

    public static MoveForward FromSeed(uint seed)
    {
        MoveForward moveForward = new MoveForward();
        moveForward.Seed = seed;
        return moveForward;
    }
}
