using UnityEngine;
using UnityEngine.InputSystem;

/// 2.2 Quit Key - ends the session when the bound button is pressed.
public class Quit : MonoBehaviour
{
    public InputActionReference action;

    void OnEnable()
    {
        if (action == null) return;
        action.action.Enable();
        action.action.performed += OnPerformed;
    }

    // Entering and leaving play mode repeatedly would stack up duplicate
    // subscriptions if we never unsubscribed.
    void OnDisable()
    {
        if (action == null) return;
        action.action.performed -= OnPerformed;
    }

    void OnPerformed(InputAction.CallbackContext ctx)
    {
#if UNITY_EDITOR
        // Application.Quit() does nothing while running inside the editor.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
