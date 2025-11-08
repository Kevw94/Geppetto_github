using MikeNspired.XRIStarterKit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FlashlightColorButton : MonoBehaviour
{
    [SerializeField] private FlashLight flashlight;

    private XRSimpleInteractable simple;

    void Awake()
    {
        simple = GetComponent<XRSimpleInteractable>();

        if (simple == null)
            Debug.LogError("XRSimpleInteractable manquant sur le bouton !");
    }

    void OnEnable()
    {
        if (simple != null)
            simple.selectEntered.AddListener(OnPoke);
    }

    void OnDisable()
    {
        if (simple != null)
            simple.selectEntered.RemoveListener(OnPoke);
    }

    private void OnPoke(SelectEnterEventArgs args)
    {
        if (flashlight != null)
        {
            flashlight.SetNextColor();
            Debug.Log("Bouton rouge poké → couleur suivante !");

            // joue le son de la lampe
            AudioSource source = flashlight.GetComponent<AudioSource>();
            if (source != null)
                source.Play();
        }
        else
        {
            Debug.LogWarning("Aucun FlashLight assigné au bouton rouge !");
        }
    }
}
