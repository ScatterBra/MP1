using UnityEngine;

/// Object Shooter + Arbitrary Orbiter - moves by its own velocity every frame and,
/// when given an attractor, is pulled toward it with inverse-square gravity.
public class SpawnedBody : MonoBehaviour
{
    [Tooltip("Metres per second. Set by the spawner at launch.")]
    public Vector3 velocity;

    [Tooltip("Optional. When set, the body falls toward this object's position.")]
    public Transform attractor;

    [Tooltip("Strength of the attractor's pull (same meaning as CometOrbit.gravity).")]
    public float gravity = 20f;

    void Update()
    {
        float dt = Time.deltaTime;

        if (attractor != null)
        {
            // Same inverse-square pull as CometOrbit, but measured from the
            // attractor's position rather than the world origin.
            Vector3 offset = transform.position - attractor.position;
            float distance = offset.magnitude;
            if (distance > 0.001f)
                velocity += -gravity * offset / (distance * distance * distance) * dt;
        }

        // Semi-implicit Euler: velocity first, then position with the new velocity.
        transform.position += velocity * dt;
    }
}
