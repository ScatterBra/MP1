using UnityEngine;
using UnityEngine.InputSystem;

/// 2.3 Light Switch - steps the point light through a list of colours, with a
/// particle burst (tinted to the new colour) and a 3D sound at the light.
[RequireComponent(typeof(Light))]
public class LightSwitch : MonoBehaviour
{
    public InputActionReference action;

    public Color[] colors =
    {
        new Color(1.00f, 0.96f, 0.88f),  // warm white (the starting colour)
        new Color(1.00f, 0.30f, 0.25f),  // red
        new Color(0.35f, 1.00f, 0.45f),  // green
        new Color(0.35f, 0.55f, 1.00f),  // blue
        new Color(1.00f, 0.85f, 0.20f),  // yellow
    };

    [Header("Feedback")]
    [Tooltip("Particle burst played at the light on each change. Tinted to the new colour.")]
    public ParticleSystem burstPrefab;

    [Tooltip("3D sound played from the light on each change.")]
    public AudioClip sound;

    [Tooltip("Distance within which the sound is at full volume. The light sits on the ceiling, far above the user, so this is large.")]
    public float soundMinDistance = 10f;

    Light pointLight;
    int index;

    // Awake, not Start: OnEnable runs before Start, so a button pressed on the
    // very first frame would otherwise hit a null reference.
    void Awake()
    {
        pointLight = GetComponent<Light>();
    }

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
        if (colors.Length == 0) return;
        index = (index + 1) % colors.Length;
        pointLight.color = colors[index];

        // Slightly below the light so the burst isn't hidden behind the ceiling.
        ParticleSystem burst = Feedback.Play(burstPrefab, sound, transform.position + Vector3.down * 0.5f, soundMinDistance);
        if (burst != null)
        {
            var main = burst.main;
            main.startColor = colors[index];
        }
    }
}
