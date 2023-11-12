using Infrastructure.Factories.ParticleFactory.Interfaces;
using UnityEngine;

namespace Infrastructure.Factories.ParticleFactory
{
    public class ParticlesFactory : IParticlesFactory
    {
        public ParticleSystem Create(ParticleSystem original, Transform parent) => Object.Instantiate(original, parent);
    }
}