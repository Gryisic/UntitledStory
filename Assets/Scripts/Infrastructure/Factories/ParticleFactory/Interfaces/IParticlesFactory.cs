using UnityEngine;

namespace Infrastructure.Factories.ParticleFactory.Interfaces
{
    public interface IParticlesFactory
    {
        ParticleSystem Create(ParticleSystem original, Transform parent);
    }
}