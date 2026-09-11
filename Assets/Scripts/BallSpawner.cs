using UnityEngine;
using UnityEngine.InputSystem;

/// Object Spawning + Particle Bursts + Spatial Sound - spawns a prefab in front of
/// the controller, with a particle burst and a 3D sound at the same spot.
public class BallSpawner : MonoBehaviour
{
    public InputActionReference action;

    [Tooltip("The object to create. Give it an AudioSource (Spatial Blend = 1) for sound.")]
    public GameObject objectPrefab;

    [Tooltip("A one-shot Particle System played where the object appears.")]
    public ParticleSystem burstPrefab;

    [Tooltip("Where objects appear - usually the Right Controller.")]
    public Transform spawnPoint;

    [Tooltip("How far in front of the spawn point the object appears.")]
    public float forwardOffset = 0.3f;

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
        // exactly where it appeared (and follows it if it moves later).
        AudioSource sound = spawned.GetComponent<AudioSource>();
        if (sound != null) sound.Play();
    }
}
