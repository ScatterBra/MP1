using UnityEngine;
using UnityEngine.InputSystem;

/// Object Spawning + Particle Bursts + Spatial Sound + Object Shooter + Perfect Orbits -
/// spawns a prefab in front of the controller with a particle burst and a 3D sound at
/// the same spot, then launches it straight along the controller or into a stable orbit.
public class BallSpawner : MonoBehaviour
{
    public InputActionReference action;

    [Tooltip("The object to create. Give it an AudioSource (Spatial Blend = 1) for sound and a SpawnedBody to move.")]
    public GameObject objectPrefab;

    [Tooltip("A one-shot Particle System played where the object appears.")]
    public ParticleSystem burstPrefab;

    [Tooltip("Where objects appear - usually the Right Controller.")]
    public Transform spawnPoint;

    [Tooltip("How far in front of the spawn point the object appears.")]
    public float forwardOffset = 0.3f;

    [Header("Launch")]
    [Tooltip("On: launch into a stable orbit around the attractor (Perfect Orbits). Off: fly straight along the controller (Object Shooter).")]
    public bool perfectOrbit = true;

    [Tooltip("Speed of straight shots, in metres per second.")]
    public float shootSpeed = 3f;

    [Tooltip("The body launched objects orbit - e.g. the planet.")]
    public Transform attractor;

    [Tooltip("Strength of the attractor's pull (same meaning as CometOrbit.gravity).")]
    public float gravity = 20f;

    void OnEnable()
    {
        if (action == null) return;
        action.action.Enable();
        action.action.performed += OnPerformed;
    }

    void OnDisable()
    {
        if (action == null) return;
        action.action.performed -= OnPerformed;
    }

    void OnPerformed(InputAction.CallbackContext ctx)
    {
        Transform origin = spawnPoint != null ? spawnPoint : transform;
        Vector3 position = origin.position + origin.forward * forwardOffset;
        GameObject spawned = Instantiate(objectPrefab, position, origin.rotation);

        // Burst at the same spot, so the user's eye is drawn to the new object.
        // The prefab's Stop Action is Destroy, so it cleans itself up.
        if (burstPrefab != null)
        {
            ParticleSystem burst = Instantiate(burstPrefab, position, Quaternion.identity);
            burst.Play();
        }

        // The AudioSource lives on the spawned object, so the sound comes from
        // exactly where it appeared (and follows it as it flies).
        AudioSource sound = spawned.GetComponent<AudioSource>();
        if (sound != null) sound.Play();

        SpawnedBody body = spawned.GetComponent<SpawnedBody>();
        if (body != null) Launch(body, position, origin.forward);
    }

    void Launch(SpawnedBody body, Vector3 position, Vector3 aim)
    {
        if (!perfectOrbit || attractor == null)
        {
            body.attractor = null;
            body.velocity = aim * shootSpeed;
            return;
        }

        Vector3 offset = position - attractor.position;
        float distance = offset.magnitude;
        if (distance < 0.001f) return;
        Vector3 radial = offset / distance;

        // Strip the part of the aim that points toward (or away from) the attractor.
        // What's left is tangential - exactly what a circular orbit needs.
        Vector3 tangent = aim - Vector3.Dot(aim, radial) * radial;

        // Aiming dead at the attractor leaves nothing tangential; pick any perpendicular.
        if (tangent.sqrMagnitude < 1e-6f) tangent = Vector3.Cross(radial, Vector3.up);
        if (tangent.sqrMagnitude < 1e-6f) tangent = Vector3.Cross(radial, Vector3.right);

        body.attractor = attractor;
        body.gravity = gravity;
        body.velocity = tangent.normalized * Mathf.Sqrt(gravity / distance);
    }
}
