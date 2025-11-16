using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class MagazinePickup : MonoBehaviour
{
    public event Action OnPickedUp;

    private void Awake()
    {
        var grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        OnPickedUp?.Invoke();
    }
}
