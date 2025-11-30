using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using MikeNspired.XRIStarterKit;

public class AmmoSocketHandler : MonoBehaviour
{
    public PlayerAmmoManager ammoManager;
    public AudioSource refuelAmmoSound;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        socket.selectEntered.AddListener(OnPackInserted);
    }

    private void OnPackInserted(SelectEnterEventArgs args)
    {
        var pack = args.interactableObject.transform.GetComponent<AmmoPack>();
        if (pack != null)
        {
            if (refuelAmmoSound != null)
                refuelAmmoSound.Play();

            ammoManager.AddAmmo(pack.ammoCount);
            // Désactiver l’objet pack
            args.interactableObject.transform.gameObject.SetActive(false);
            // Libérer le socket pour qu’il puisse recevoir un nouveau pack
            socket.interactionManager.SelectExit(socket, args.interactableObject);
        }
    }
}
