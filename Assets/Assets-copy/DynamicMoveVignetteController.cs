using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;
using XR.Interaction.Toolkit.Samples;

public class VignetteJoystickController : MonoBehaviour
{
    [SerializeField] public TunnelingVignetteController vignette;   // ton vignette
    [SerializeField] public InputActionReference moveAction;         // l’Input Action du stick gauche

    private bool wasMoving = false;
    public float moveThreshold = 0.01f;

    void OnEnable()
    {
        moveAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {
        if (vignette == null || moveAction == null)
            return;

        Vector2 input = moveAction.action.ReadValue<Vector2>();
        bool isMoving = input.sqrMagnitude > moveThreshold;

        if (isMoving && !wasMoving)
            vignette.currentParameters.featheringEffect = 0.5f;
        else if (!isMoving && wasMoving)
            vignette.currentParameters.featheringEffect = 1.0f;

        wasMoving = isMoving;
    }
}
