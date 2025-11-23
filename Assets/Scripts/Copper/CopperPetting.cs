using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRBaseInteractable))]
public class CopperPetting : MonoBehaviour
{
    [Header("References")]
    public Animator dogAnimator;
    public AudioSource barkSource;
    public AudioClip happyClip;             // Son quand Copper est caressé
    public string tailWagTrigger = "TailWag";

    [Header("Debug")]
    public bool debugLogs = true;

    private XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
    }

    void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnPet);  // Déclenche quand une main entre en hover
        if (debugLogs) Debug.Log("[CopperPetting] Enabled");
    }

    void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnPet);
        if (debugLogs) Debug.Log("[CopperPetting] Disabled");
    }

    private void OnPet(HoverEnterEventArgs args)
    {
        // Vérifie que la main est bien sur le layer PokeOnly
        if (((1 << args.interactorObject.transform.gameObject.layer) & LayerMask.GetMask("PokeOnly")) == 0)
        {
            if (debugLogs) Debug.Log("[CopperPetting] Interactor not on PokeOnly layer");
            return;
        }

        if (debugLogs) Debug.Log($"[CopperPetting] Copper petted by {args.interactorObject.transform.name}");

        // Animation happy
        if (dogAnimator != null && !string.IsNullOrEmpty(tailWagTrigger))
        {
            dogAnimator.SetTrigger(tailWagTrigger);
        }

        // Son
        if (barkSource != null && happyClip != null)
        {
            barkSource.PlayOneShot(happyClip);
        }
    }
}
