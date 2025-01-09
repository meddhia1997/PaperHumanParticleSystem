using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PaperHumanParticleSystem : MonoBehaviour
{
    [Header("Skinned Mesh Settings")]
    public SkinnedMeshRenderer skinnedMeshRenderer; // Reference to the skinned mesh
    public int maxParticles = 20000;                // Number of particles to emit
    public float detachSpeed = 2f;                  // Speed of particle detachment
    public float returnSpeed = 3f;                  // Speed of particle reformation
    public float flutterSpeed = 1f;                 // Speed of fluttering effect
    public float flutterAmplitude = 0.02f;          // Amplitude of fluttering effect

    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;
    private Vector3[] detachedPositions;            // Positions for detached particles
    private bool[] isDetached;                      // Flags to indicate detachment status
    private Mesh bakedMesh;

    private void Start()
    {
        // Initialize particle system
        particleSystem = GetComponent<ParticleSystem>();
        var main = particleSystem.main;
        main.maxParticles = maxParticles;
        main.simulationSpace = ParticleSystemSimulationSpace.Local; // Use World space

        // Bake the skinned mesh and prepare particle data
        bakedMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(bakedMesh);

        Vector3[] vertices = bakedMesh.vertices;
        int vertexCount = vertices.Length;

        // Ensure particle arrays match the maxParticles count
        particles = new ParticleSystem.Particle[maxParticles];
        detachedPositions = new Vector3[maxParticles];
        isDetached = new bool[maxParticles];

        // Emit particles and assign initial positions
        particleSystem.Emit(maxParticles);
        particleSystem.GetParticles(particles);

        for (int i = 0; i < maxParticles; i++)
        {
            // Reuse vertex positions cyclically for particles exceeding vertex count
            int vertexIndex = i % vertexCount;
            Vector3 worldPosition = skinnedMeshRenderer.transform.TransformPoint(vertices[vertexIndex]);

            particles[i].position = worldPosition;
            detachedPositions[i] = worldPosition;
            isDetached[i] = false;

            // Optional: Randomize particle rotation for better paper effect
            particles[i].rotation3D = new Vector3(
                Random.Range(0, 360f),
                Random.Range(0, 360f),
                Random.Range(0, 360f)
            );
        }

        particleSystem.SetParticles(particles, particles.Length);
    }

    private void Update()
    {
        // Bake the skinned mesh to get updated vertex positions during animation
        skinnedMeshRenderer.BakeMesh(bakedMesh);
        Vector3[] vertices = bakedMesh.vertices;
        int vertexCount = vertices.Length;

        for (int i = 0; i < particles.Length; i++)
        {
            if (isDetached[i])
            {
                // Detachment logic: particles move outward randomly
                detachedPositions[i] += Random.insideUnitSphere * detachSpeed * Time.deltaTime;
                particles[i].position = detachedPositions[i];
            }
            else
            {
                // Reformation logic: particles follow skinned mesh vertices relative to parent movement
                int vertexIndex = i % vertexCount;
                Vector3 targetWorldPosition = skinnedMeshRenderer.transform.TransformPoint(vertices[vertexIndex]);

                Vector3 flutterOffset = new Vector3(
                    Mathf.Sin(Time.time * flutterSpeed + i) * flutterAmplitude,
                    Mathf.Cos(Time.time * flutterSpeed + i) * flutterAmplitude,
                    Mathf.Sin(Time.time * flutterSpeed + i * 0.5f) * flutterAmplitude
                );

                particles[i].position = Vector3.Lerp(particles[i].position, targetWorldPosition, Time.deltaTime * returnSpeed) + flutterOffset;
                detachedPositions[i] = particles[i].position;
            }
        }

        particleSystem.SetParticles(particles, particles.Length);

        // Trigger detachment and reformation with keys
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetachParticles();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReformParticles();
        }
    }

    private void DetachParticles()
    {
        for (int i = 0; i < isDetached.Length; i++)
        {
            isDetached[i] = true;
        }
    }

    private void ReformParticles()
    {
        for (int i = 0; i < isDetached.Length; i++)
        {
            isDetached[i] = false;
        }
    }
}
