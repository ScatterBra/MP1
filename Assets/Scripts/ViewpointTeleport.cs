using UnityEngine;
using UnityEngine.InputSystem;

/// Camera Teleport - each press moves the XR rig (and with it the tracked camera and
/// controllers) to the next viewpoint in the list, with particle and sound feedback
/// where the user leaves and where they arrive.
public class ViewpointTeleport : MonoBehaviour
{
    public InputActionReference action;

    [Tooltip("The XR Origin to move. The headset camera rides along with it.")]
    public Transform rig;

    [Tooltip("Places to jump between, in order. Position = where the rig's floor point lands; Y rotation = which way it faces.")]
    public Transform[] viewpoints;

    [Header("Feedback")]
    [Tooltip("The headset camera. Feedback plays where it leaves from and just in front of where it arrives.")]
    public Transform head;

    public ParticleSystem burstPrefab;

    public AudioClip sound;

    [Tooltip("How far in front of the eyes the arrival feedback appears.")]
    public float arrivalDistance = 1f;

    int index = -1;

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
        if (rig == null || viewpoints == null || viewpoints.Length == 0) return;
        index = (index + 1) % viewpoints.Length;
        Transform target = viewpoints[index];

        Vector3 departure = HeadPosition();

        // The rig's CharacterController tracks its own position and would pull the
        // rig straight back, so it is switched off for the jump.
        CharacterController cc = rig.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        rig.SetPositionAndRotation(target.position, Quaternion.Euler(0f, target.eulerAngles.y, 0f));
        if (cc != null) cc.enabled = true;

        // One burst marks the spot just left (visible when looking back); the other
        // goes in front of the eyes so the arrival is noticed immediately.
        Feedback.Play(burstPrefab, sound, departure);
        Vector3 forward = head != null ? head.forward : rig.forward;
        Feedback.Play(burstPrefab, sound, HeadPosition() + forward * arrivalDistance);
    }

    Vector3 HeadPosition()
    {
        return head != null ? head.position : rig.position + Vector3.up * 1.5f;
    }
}
