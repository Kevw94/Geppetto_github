//==============================================================
//  VERSION COMPLETE MODIFIÉE AVEC GESTION Backpack/Weapon
//==============================================================

using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MikeNspired.XRIStarterKit
{
    public class InventorySlotItemHandler : MonoBehaviour
    {
        [Header("Visual Slot Displays")]
        [SerializeField] private GameObject slotDisplayWhenContainsItem;
        [SerializeField] private GameObject slotDisplayToAddItem;

        [Header("Transforms & Colliders")]
        [SerializeField] private Transform itemModelHolder;
        [SerializeField] private Transform backImagesThatRotate;
        [SerializeField] private BoxCollider inventorySize;

        [Header("Audio")]
        [SerializeField] private AudioSource grabAudio;
        [SerializeField] private AudioSource releaseAudio;

        public GameObject SlotDisplayWhenContainsItem => slotDisplayWhenContainsItem;
        public GameObject SlotDisplayToAddItem => slotDisplayToAddItem;

        public XRBaseInteractable CurrentSlotItem { get; private set; }

        private TransformStruct itemStartingTransform;
        private Transform boundCenterTransform, itemSlotMeshClone;
        private Vector3 goalSizeToFitInSlot;

        public float AnimationLengthItemToSlot = 0.15f;
        private Coroutine animateItemToSlotCoroutine;
        private XRInteractionManager interactionManager;
        private bool isBusy;

        private InventoryManager inventoryManager; // ← ajout important


        private void OnEnable()
        {
            isBusy = false;

            // On récupère l'InventoryManager parent
            inventoryManager = GetComponentInParent<InventoryManager>();
        }

        public void Setup(XRBaseInteractable prefab)
        {
            interactionManager = FindFirstObjectByType<XRInteractionManager>();

            if (!boundCenterTransform)
            {
                boundCenterTransform = new GameObject("Bound Center Transform").transform;
                boundCenterTransform.SetParent(itemModelHolder);
            }

            if (prefab)
            {
                CurrentSlotItem = Instantiate(prefab);
                CurrentSlotItem.transform.SetParent(transform);
                CurrentSlotItem.transform.localPosition = Vector3.zero;
                CurrentSlotItem.transform.localEulerAngles = Vector3.zero;

                SetupNewMeshClone(CurrentSlotItem);
                CurrentSlotItem.gameObject.SetActive(false);
                SnapItemToSlot();
            }
        }

        #region Slot Displays

        public void SetSlotDisplayInstant()
        {
            if (CurrentSlotItem)
            {
                SlotDisplayWhenContainsItem?.SetActive(true);
                SlotDisplayToAddItem?.SetActive(false);
            }
            else
            {
                SlotDisplayWhenContainsItem?.SetActive(false);
                SlotDisplayToAddItem?.SetActive(true);
            }
        }

        private IEnumerator AnimateIcon()
        {
            if (CurrentSlotItem)
            {
                slotDisplayWhenContainsItem.gameObject.SetActive(true);
                yield return null;
                slotDisplayToAddItem.gameObject.SetActive(false);
            }
            else
            {
                slotDisplayToAddItem.gameObject.SetActive(true);
                slotDisplayWhenContainsItem.gameObject.SetActive(false);
            }
            isBusy = false;
        }

        public IEnumerator AnimateMeshModelOpenOrClose(bool toOne, float duration)
        {
            float timer = 0f;
            Vector3 initialScale = toOne ? Vector3.zero : Vector3.one;
            Vector3 targetScale = toOne ? Vector3.one : Vector3.zero;

            while (timer < duration)
            {
                float t = Mathf.Clamp01(timer / duration);
                itemModelHolder.localScale = Vector3.Lerp(initialScale, targetScale, t);

                yield return null;
                timer += Time.deltaTime;
            }
            itemModelHolder.localScale = targetScale;
        }

        #endregion

        //==========================================================
        //   MÉTHODE CENTRALE POUR LA LOGIQUE Backpack/Weapon
        //==========================================================

        private bool CanItemGoInThisInventory(InteractableItemData data)
        {
            if (data == null) return false;

            if (inventoryManager == null)
            {
                Debug.LogWarning("[Inventory] Aucun InventoryManager trouvé dans les parents.");
                return false;
            }

            if (inventoryManager.isBackpackInventory)
            {
                // INVENTAIRE TYPE BACKPACK
                return data.canInventory;
            }
            else
            {
                // INVENTAIRE TYPE WEAPON
                return data.canWeaponInventory;
            }
        }

        //==========================================================
        //   INTERACTION PRINCIPALE
        //==========================================================

        public void InteractWithSlot(XRBaseInteractor controller)
        {
            if (!controller || isBusy)
                return;

            // Vérification item en main
            XRBaseInteractable inHand = GetItemInHand(controller);
            if (inHand)
            {
                var data = inHand.GetComponent<InteractableItemData>();
                if (!CanItemGoInThisInventory(data))
                {
                    Debug.Log($"[Inventory] {inHand.name} refusé : ne peut PAS aller dans cet inventaire.");
                    return;
                }
            }

            // Vérification item dans le slot
            if (CurrentSlotItem)
            {
                var data = CurrentSlotItem.GetComponent<InteractableItemData>();
                if (!CanItemGoInThisInventory(data))
                {
                    Debug.Log($"[Inventory] {CurrentSlotItem.name} refusé : ne peut PAS être retiré ou manipulé.");
                    return;
                }
            }

            isBusy = true;

            if (animateItemToSlotCoroutine != null)
                StopCoroutine(animateItemToSlotCoroutine);

            var itemInHand = GetItemInHand(controller);

            if (itemInHand)
                AddItemToSlot(controller);
            else if (CurrentSlotItem)
                RetrieveItemFromSlot(controller, destroyItemMesh: true);
            else
                isBusy = false;

            StartCoroutine(AnimateIcon());
        }

        //==========================================================
        //    AJOUTER ITEM DANS LE SLOT (AVEC SWAP)
        //==========================================================

        private void AddItemToSlot(XRBaseInteractor controller)
        {
            var itemHandIsHolding = GetItemInHand(controller);
            if (!itemHandIsHolding)
            {
                isBusy = false;
                return;
            }

            // SWAP si un objet est déjà dans le slot
            if (CurrentSlotItem != null)
            {
                if (itemSlotMeshClone)
                    Destroy(itemSlotMeshClone.gameObject);

                CurrentSlotItem.gameObject.SetActive(true);
                CurrentSlotItem.transform.SetParent(null);

                StartCoroutine(GrabNewItem(controller, CurrentSlotItem));

                CurrentSlotItem = null;
            }

            releaseAudio?.Play();
            ReleaseItemFromHand(controller, itemHandIsHolding);

            itemHandIsHolding.transform.SetParent(transform);

            var grabDisable = itemHandIsHolding.GetComponent<OnGrabEnableDisable>();
            grabDisable?.EnableAll();

            CurrentSlotItem = itemHandIsHolding;

            SetupNewMeshClone(itemHandIsHolding);
            itemHandIsHolding.gameObject.SetActive(false);
            itemHandIsHolding.transform.localPosition = Vector3.zero;
            itemHandIsHolding.transform.localEulerAngles = Vector3.zero;

            animateItemToSlotCoroutine = StartCoroutine(AnimateItemToSlot());
        }

        //==========================================================
        //       RETIRER ITEM DU SLOT
        //==========================================================

        private void RetrieveItemFromSlot(XRBaseInteractor controller, bool destroyItemMesh)
        {
            if (!CurrentSlotItem) return;

            if (itemSlotMeshClone && destroyItemMesh)
                Destroy(itemSlotMeshClone.gameObject);

            CurrentSlotItem.gameObject.SetActive(true);
            CurrentSlotItem.transform.SetParent(null);

            StartCoroutine(GrabNewItem(controller, CurrentSlotItem));
            grabAudio?.Play();

            CurrentSlotItem = null;
        }

        //==========================================================
        //   UTILITAIRES
        //==========================================================

        private static XRBaseInteractable GetItemInHand(XRBaseInteractor controller)
        {
            if (!controller.hasSelection) return null;
            if (controller.interactablesSelected.Count == 0) return null;
            return controller.interactablesSelected[0] as XRBaseInteractable;
        }

        private void ReleaseItemFromHand(XRBaseInteractor interactor, XRBaseInteractable interactable)
        {
            interactionManager?.SelectExit((IXRSelectInteractor)interactor, interactable);
        }

        private IEnumerator GrabNewItem(XRBaseInteractor interactor, XRBaseInteractable interactable)
        {
            yield return new WaitForFixedUpdate();
            interactionManager?.SelectEnter((IXRSelectInteractor)interactor, interactable);
        }

        //==========================================================
        //       ANIMATIONS & CLONE
        //==========================================================

        private IEnumerator AnimateItemToSlot()
        {
            float timer = 0f;

            while (timer < AnimationLengthItemToSlot + Time.deltaTime)
            {
                float t = timer / AnimationLengthItemToSlot;
                boundCenterTransform.localPosition =
                    Vector3.Lerp(itemStartingTransform.position, Vector3.zero, t);
                boundCenterTransform.localRotation =
                    Quaternion.Lerp(itemStartingTransform.rotation, Quaternion.Euler(0, 90, 0), t);
                boundCenterTransform.localScale =
                    Vector3.Lerp(itemStartingTransform.scale, goalSizeToFitInSlot, t);

                yield return null;
                timer += Time.deltaTime;
            }
            isBusy = false;
        }

        private void SnapItemToSlot()
        {
            boundCenterTransform.localPosition = Vector3.zero;
            boundCenterTransform.localScale = goalSizeToFitInSlot;
            boundCenterTransform.localRotation = Quaternion.Euler(0, 90, 0);
        }

        private void SetupNewMeshClone(XRBaseInteractable newItem)
        {
            if (itemSlotMeshClone)
                Destroy(itemSlotMeshClone.gameObject);

            CreateBoundsCenter();

            itemSlotMeshClone = GameObjectCloner.DuplicateAndStrip(newItem.gameObject).transform;

            itemSlotMeshClone.SetParent(itemModelHolder);
            itemSlotMeshClone.SetPositionAndRotation(newItem.transform.position, newItem.transform.rotation);

            var bounds = GetBoundsOfAllMeshes(itemSlotMeshClone);

            boundCenterTransform.position = bounds.center;
            boundCenterTransform.rotation = newItem.transform.rotation;

            itemSlotMeshClone.SetParent(boundCenterTransform);

            inventorySize.enabled = true;
            Vector3 parentSize = inventorySize.bounds.size;
            float ratioX = parentSize.x / bounds.size.x;
            float ratioY = parentSize.y / bounds.size.y;
            float ratioZ = parentSize.z / bounds.size.z;
            float scaleRatio = Mathf.Min(ratioX, ratioY, ratioZ);
            scaleRatio = Mathf.Min(scaleRatio, 1f);

            boundCenterTransform.localScale = Vector3.one * scaleRatio;
            inventorySize.enabled = false;

            itemStartingTransform.SetTransformStruct(
                newItem.transform.position,
                newItem.transform.rotation,
                newItem.transform.lossyScale
            );
            goalSizeToFitInSlot = boundCenterTransform.localScale;
        }

        private void CreateBoundsCenter()
        {
            if (boundCenterTransform)
                Destroy(boundCenterTransform.gameObject);

            boundCenterTransform = new GameObject("Bound Center Transform").transform;
            boundCenterTransform.SetParent(itemModelHolder, false);
            boundCenterTransform.localScale = Vector3.one;
        }

        private static Bounds GetBoundsOfAllMeshes(Transform item)
        {
            Bounds bounds = new Bounds();
            var rends = item.GetComponentsInChildren<Renderer>();
            foreach (var rend in rends)
            {
                if (rend.GetComponent<ParticleSystem>()) continue;

                if (bounds.extents == Vector3.zero)
                    bounds = rend.bounds;
                else
                    bounds.Encapsulate(rend.bounds);
            }
            return bounds;
        }
    }
}