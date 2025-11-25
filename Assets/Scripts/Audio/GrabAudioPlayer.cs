using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabAudioPlayer : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private AudioSource audioSource;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();

        if (grabInteractable == null)
            Debug.LogError("Aucun XRGrabInteractable trouvé sur cet objet !");
        if (audioSource == null)
            Debug.LogError("Aucun AudioSource trouvé sur cet objet !");
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (audioSource != null)
            audioSource.Play();
    }
}
