using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class FetchableObject : MonoBehaviour
{
    [Header("References")]
    public CopperFetchManager copper; // assigner dans l'inspecteur

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Événements du XRGrabInteractable
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    /// <summary>
    /// Appelé quand l'objet est pris en main par le joueur
    /// </summary>
    /// <param name="args"></param>
    private bool hasBeenInHand = false;

    void OnGrabbed(SelectEnterEventArgs args)
    {
        hasBeenInHand = true;
        copper.ObjectTouchedHand(transform);
    }

    void OnReleased(SelectExitEventArgs args)
    {
        if (hasBeenInHand)
            copper.ObjectTouchedGround(transform);
    }

}
