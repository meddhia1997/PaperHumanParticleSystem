using UnityEngine;

[RequireComponent(typeof(ParticleSystem), typeof(Rigidbody), typeof(Animator))]
public class PaperHumanParticleSystem : MonoBehaviour
{
    [Header("Skinned Mesh Settings")]
    public SkinnedMeshRenderer skinnedMeshRenderer; // Reference to the skinned mesh
    public int maxParticles = 20000;                // Number of particles to emit
    public float detachSpeed = 2f;                  // Speed of particle detachment
    public float returnSpeed = 3f;                  // Speed of particle reformation
    public float flutterSpeed = 1f;                 // Speed of fluttering effect
    public float flutterAmplitude = 0.02f;          // Amplitude of fluttering effect

    [Header("Movement Settings")]
    public float moveSpeed = 5f;                    // Speed of movement
    public float rotationSpeed = 720f;              // Speed of rotation

    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;
    private Vector3[] detachedPositions;            // Positions for detached particles
    private bool[] isDetached;                      // Flags to indicate detachment status
    private Mesh bakedMesh;
    private Rigidbody rb;                           // Reference to Rigidbody
    private Animator animator;                      // Reference to Animator

    private void Start()
    {
        // Initialize particle system
        particleSystem = GetComponent<ParticleSystem>();
        var main = particleSystem.main;
        main.maxParticles = maxParticles;
        main.simulationSpace = ParticleSystemSimulationSpace.World; // Use World Space

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
            // Map particles to vertices (cyclically for overflow)
            int vertexIndex = i % vertexCount;
            Vector3 worldPosition = skinnedMeshRenderer.transform.TransformPoint(vertices[vertexIndex]);

            particles[i].position = worldPosition;
            detachedPositions[i] = worldPosition;
            isDetached[i] = false;

            // Randomize particle rotation
            particles[i].rotation3D = new Vector3(
                Random.Range(0, 360f),
                Random.Range(0, 360f),
                Random.Range(0, 360f)
            );
        }

        particleSystem.SetParticles(particles, particles.Length);

        // Get Rigidbody and Animator components
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator is missing!");
        }
    }

    private void Update()
    {
        // Trigger detachment and reformation with keys
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetachParticles();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReformParticles();
        }

        // Handle movement
        HandleMovement();
    }

    private void LateUpdate()
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
                // Reformation logic: particles follow skinned mesh vertices in world space
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
    }

    private void DetachParticles()
    {
        for (int i = 0; i < isDetached.Length; i++)
        {
            isDetached[i] = true;
            var main = particleSystem.main;

            main.simulationSpace = ParticleSystemSimulationSpace.Local; // Use World Space
        }
    }

    private void ReformParticles()
    {
        for (int i = 0; i < isDetached.Length; i++)
        {
            isDetached[i] = false;
            var main = particleSystem.main;

            main.simulationSpace = ParticleSystemSimulationSpace.World; // Use World Space

        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Calculate target angle for rotation
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, Time.deltaTime);

            // Rotate the character
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Move the character
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
        }

        // Update Animator speed parameter
        animator.SetFloat("speed", moveDirection.magnitude);
    }
}
