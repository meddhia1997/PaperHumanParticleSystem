using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct ParticlePhysicsJob : IJobParallelFor
{
    public NativeArray<Particle> particles;
    public float deltaTime;

    public void Execute(int index)
    {
        Particle particle = particles[index];

        // Apply gravity
        particle.velocity += Vector3.down * 9.81f * deltaTime;

        // Update position
        particle.position += particle.velocity * deltaTime;

        // Bounce off ground
        if (particle.position.y < 0)
        {
            particle.position.y = 0;
            particle.velocity.y *= -0.5f; // Lose velocity on bounce
        }

        particles[index] = particle;
    }
}
