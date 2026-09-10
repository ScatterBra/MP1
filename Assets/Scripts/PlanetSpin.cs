using UnityEngine;

/// 2.4 Orbiting Moon - spins the planet about Y. Any child (the moon) is carried
/// around with it, so the moon orbits without needing a script of its own.
public class PlanetSpin : MonoBehaviour
{
    public float degreesPerSecond = 30f;

    void Update()
    {
        // Multiplying by deltaTime turns "degrees per frame" into "degrees per
        // second", so the speed no longer depends on the framerate.
        transform.Rotate(Vector3.up, degreesPerSecond * Time.deltaTime, Space.World);
    }
}
