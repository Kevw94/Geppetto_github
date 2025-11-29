using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CopperPetting : MonoBehaviour
{
    [Header("Petting FX")]
    public ParticleSystem petParticles;
    public AudioSource petSound;
    public float cooldown = 1f;

    private bool canPet = true;
    private XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(OnHandHover);
    }

    private void OnDestroy()
    {
        interactable.hoverEntered.RemoveListener(OnHandHover);
    }

    private void OnHandHover(HoverEnterEventArgs args)
    {
        if (!canPet) return;
        if (!(args.interactorObject is XRDirectInteractor)) return;

        PlayParticles();
    }

    private void PlayParticles()
    {
        if (petParticles != null)
            petParticles.Play();

        if (petSound != null)
            petSound.Play();

        StartCoroutine(PetCooldown());
    }

    private System.Collections.IEnumerator PetCooldown()
    {
        canPet = false;
        yield return new WaitForSeconds(cooldown);
        canPet = true;
    }
}
