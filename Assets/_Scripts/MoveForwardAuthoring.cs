using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class MoveForwardAuthoring : MonoBehaviour
{
    public uint Seed;
    
    private class Baker : Baker<MoveForwardAuthoring>
    {
        public override void Bake(MoveForwardAuthoring authoring)
        {
            Random rnd = new Random(authoring.Seed);

            float speed = rnd.NextFloat();
            float3 direction = rnd.NextFloat3Direction();
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveForward
            {
                Direction = direction,
                Speed = speed
            });
            
        }
    }
}