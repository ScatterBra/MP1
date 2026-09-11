using UnityEngine;
using UnityEngine.InputSystem;

/// Camera Teleport - each press moves the XR rig (and with it the tracked camera and
/// controllers) to the next viewpoint in the list.
public class ViewpointTeleport : MonoBehaviour
{
    public InputActionReference action;

    [Tooltip("The XR Origin to move. The headset camera rides along with it.")]
    public Transform rig;

    [Tooltip("Places to jump between, in order. Position = where the rig's floor point lands; Y rotation = which way it faces.")]
    public Transform[] viewpoints;

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

        // The rig's CharacterController tracks its own position and would pull the
        // rig straight back, so it is switched off for the jump.
        CharacterController cc = rig.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        rig.SetPositionAndRotation(target.position, Quaternion.Euler(0f, target.eulerAngles.y, 0f));
        if (cc != null) cc.enabled = true;
    }
}
