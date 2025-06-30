using Unity.Entities;
using UnityEngine;

class FireworkParticleAuthoring : MonoBehaviour
{
    public float Gravity;
    public float LifeTime;
}

class FireworkParticleAuthoringBaker : Baker<FireworkParticleAuthoring>
{
    public override void Bake(FireworkParticleAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new FireworkParticle
        {
            Gravity = authoring.Gravity,
            Lifetime = authoring.LifeTime,
        });
        
    }
}
