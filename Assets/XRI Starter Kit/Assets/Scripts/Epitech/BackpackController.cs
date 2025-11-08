using MikeNspired.XRIStarterKit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


namespace MikeNspired.XRIStarterKit
{ 
    public class BackpackInventoryTrigger : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventoryManager;

        private XRGrabInteractable grabInteractable;

        private void Awake()
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
        }

        private void OnEnable()
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }

        private void OnDisable()
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }

        private void OnGrabbed(SelectEnterEventArgs args)
        {
            if (inventoryManager != null)
            {
                inventoryManager.TurnOnInventory();
            }
        }

        private void OnReleased(SelectExitEventArgs args)
        {
            if (inventoryManager != null)
            {
                inventoryManager.TurnOnInventory();
            }
        }
    }
}
