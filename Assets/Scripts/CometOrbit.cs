using UnityEngine;

/// 2.5 Orbiting Comet - integrates its own velocity under an inverse-square pull
/// toward the planet, rather than using Unity's physics.
public class CometOrbit : MonoBehaviour
{
    [Tooltip("The body the comet falls toward. Its position is the centre of gravity.")]
    public Transform planet;

    [Tooltip("Strength of the pull. Larger = faster orbit.")]
    public float gravity = 20f;

    [Tooltip("Distance from the planet at which the comet starts.")]
    public float startRadius = 5f;

    [Tooltip("Tilt of the orbital plane away from horizontal, in degrees.")]
    public float inclination = 30f;

    [Tooltip("1 = circular orbit. Below 1 falls inward, above 1 swings outward.")]
    public float speedScale = 1f;

    Vector3 velocity;

    void Start()
    {
        if (planet == null)
        {
            Debug.LogError("CometOrbit: no planet assigned.", this);
            enabled = false;
            return;
        }

        // Start out along +X from the planet.
        transform.position = planet.position + new Vector3(startRadius, 0f, 0f);

        // A circular orbit needs speed sqrt(gravity / radius), aimed perpendicular
        // to the radius. Any direction in the YZ plane is perpendicular to +X, so
        // tilting within that plane inclines the orbit without breaking the circle.
        float rad = inclination * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(0f, Mathf.Sin(rad), Mathf.Cos(rad));
        velocity = dir * Mathf.Sqrt(gravity / startRadius) * speedScale;
    }

    void Update()
    {
        Step(Time.deltaTime);
    }

    /// Split out so the orbit can be stepped from an editor test as well.
    public void Step(float dt)
    {
        Vector3 offset = transform.position - planet.position;
        float distance = offset.magnitude;
        if (distance < 0.001f) return;

        // offset / distance is the unit direction; dividing by distance^2 again
        // gives the inverse-square falloff. Together: offset / distance^3.
        Vector3 acceleration = -gravity * offset / (distance * distance * distance);

        // Updating velocity first and then using the new value (semi-implicit
        // Euler) keeps the orbit from spiralling out the way plain Euler does.
        velocity += acceleration * dt;
        transform.position += velocity * dt;
    }
}
