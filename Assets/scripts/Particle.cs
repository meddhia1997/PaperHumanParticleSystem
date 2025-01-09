using UnityEngine;
using Unity.Burst;

[BurstCompile]
public struct Particle
{
    public Vector3 position;
    public Vector3 velocity;
    public Color color;
    public float size;
}
