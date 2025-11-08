using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class FixSocketDetachPhysics : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // On s'assure que la physique est réactivée quand on quitte un socket ou qu'on lâche la main
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void OnDestroy()
    {
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }
}
