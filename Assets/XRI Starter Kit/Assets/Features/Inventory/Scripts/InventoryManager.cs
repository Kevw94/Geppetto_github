using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;

namespace MikeNspired.XRIStarterKit
{
    public class InventoryManager : MonoBehaviour
    {
        public static event Action<InventorySlot> OnLeftSlotHoverBegan, OnLeftSlotHoverEnded, OnRightSlotHoverBegan, OnRightSlotHoverEnded;

        [Header("Inventory Mode")]
        [Tooltip("If TRUE → This is a backpack (no controller input). If FALSE → Behaves like hands inventory.")]
        public bool isBackpackInventory = false;

        [Header("Input Actions (ignored in backpack mode)")]
        [SerializeField] private InputActionReference openMenuInputLeftHand;
        [SerializeField] private InputActionReference openMenuInputRightHand;

        [Header("Interaction Caster / Hands")]
        public SphereInteractionCaster leftController;
        public SphereInteractionCaster rightController;

        [Header("Audio")]
        [SerializeField] private AudioSource enableAudio;
        [SerializeField] private AudioSource disableAudio;

        [Header("Behavior Settings")]
        [SerializeField] private bool lookAtController;
        [SerializeField] private float queryInterval = 0.1f;
        [SerializeField] private float interactionRadius = 0.5f;
        [SerializeField] private InventorySlot[] inventorySlots;

        private bool isActive;
        private float nextQueryTime;

        private InventorySlot activeLeftSlot;
        private InventorySlot activeRightSlot;

        public InventorySlot ActiveLeftSlot => activeLeftSlot;
        public InventorySlot ActiveRightSlot => activeRightSlot;

        private void Awake()
        {
            OnValidate();

            // Only bind input if NOT backpack
            if (!isBackpackInventory)
            {
                openMenuInputLeftHand.GetInputAction().performed += _ => ToggleInventoryAtController(false);
                openMenuInputRightHand.GetInputAction().performed += _ => ToggleInventoryAtController(true);
            }

            // Disable slots at startup
            foreach (var slot in inventorySlots)
                slot.gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            if (inventorySlots == null || inventorySlots.Length == 0)
                inventorySlots = GetComponentsInChildren<InventorySlot>();
        }

        private void OnEnable()
        {
            if (!isBackpackInventory)
            {
                openMenuInputLeftHand.EnableAction();
                openMenuInputRightHand.EnableAction();
            }
        }

        private void OnDisable()
        {
            if (!isBackpackInventory)
            {
                openMenuInputLeftHand.DisableAction();
                openMenuInputRightHand.DisableAction();
            }
        }

        private void Update()
        {
            if (!isActive || Time.time < nextQueryTime)
                return;

            CheckHandProximity(leftController, ref activeLeftSlot, true);
            CheckHandProximity(rightController, ref activeRightSlot, false);

            nextQueryTime = Time.time + queryInterval;
        }

        // Called by input bindings (hand-based toggle). Preserves original behavior:
        // toggles inventory and positions it at the corresponding hand.
        private void ToggleInventoryAtController(bool isRightHand)
        {
            if (isBackpackInventory) return;

            if (isRightHand)
            {
                if (rightController != null)
                    TurnOnInventory(rightController.gameObject);
                else
                    TurnOnInventory();
            }
            else
            {
                if (leftController != null)
                    TurnOnInventory(leftController.gameObject);
                else
                    TurnOnInventory();
            }
        }

        // Public toggle used by backpack mode or external calls (no hand position)
        public void TurnOnInventory()
        {
            TurnOnInventory(null);
        }

        // Core toggle — if hand != null, position the inventory relative to the hand (old weapon behavior)
        private void TurnOnInventory(GameObject hand)
        {
            isActive = !isActive;
            ToggleInventoryItems(isActive, hand);
            PlayAudio(isActive);

            // Clear the active slots if turning off
            if (!isActive)
            {
                if (activeLeftSlot)
                {
                    activeLeftSlot.EndControllerHover();
                    OnLeftSlotHoverEnded?.Invoke(activeLeftSlot);
                    activeLeftSlot = null;
                }

                if (activeRightSlot)
                {
                    activeRightSlot.EndControllerHover();
                    OnRightSlotHoverEnded?.Invoke(activeRightSlot);
                    activeRightSlot = null;
                }
            }
            else
            {
                // Force immediate re-check
                nextQueryTime = Time.time;
            }
        }

        private void PlayAudio(bool state)
        {
            if (state) enableAudio?.Play();
            else disableAudio?.Play();
        }

        // When enabling, if hand is provided we set position/rotation for the inventory (weapon mode).
        private void ToggleInventoryItems(bool state, GameObject hand)
        {
            foreach (var slot in inventorySlots)
            {
                if (!state)
                    slot.DisableSlot();
                else
                {
                    slot.gameObject.SetActive(true);
                    slot.EnableSlot();
                    if (hand != null)
                        SetPositionAndRotation(hand);
                }
            }
        }

        private void SetPositionAndRotation(GameObject hand)
        {
            if (hand == null) return;

            transform.position = hand.transform.position;
            transform.localEulerAngles = Vector3.zero;

            if (lookAtController)
                SetPosition(hand.transform);
            else if (Camera.main)
                transform.LookAt(Camera.main.transform);
        }

        private void SetPosition(Transform hand)
        {
            var handDirection = hand.forward;
            transform.forward = Vector3.ProjectOnPlane(-handDirection, transform.up);
        }

        private void CheckHandProximity(SphereInteractionCaster caster, ref InventorySlot activeSlot, bool isLeft)
        {
            if (caster == null) return;

            Vector3 handPos = caster.transform.position;
            float closestDist = float.MaxValue;
            InventorySlot closestSlot = null;

            foreach (var slot in inventorySlots)
            {
                if (!slot.gameObject.activeInHierarchy) continue;

                float dist = Vector3.Distance(handPos, slot.transform.position);
                if (dist < interactionRadius && dist < closestDist)
                {
                    closestDist = dist;
                    closestSlot = slot;
                }
            }

            if (closestSlot != activeSlot)
            {
                if (activeSlot != null)
                {
                    activeSlot.EndControllerHover();
                    if (isLeft) OnLeftSlotHoverEnded?.Invoke(activeSlot);
                    else OnRightSlotHoverEnded?.Invoke(activeSlot);
                }

                activeSlot = closestSlot;
                if (activeSlot != null)
                {
                    activeSlot.BeginControllerHover();
                    if (isLeft) OnLeftSlotHoverBegan?.Invoke(activeSlot);
                    else OnRightSlotHoverBegan?.Invoke(activeSlot);
                }
            }
            else
            {
                if (activeSlot != null && !activeSlot.gameObject.activeInHierarchy)
                {
                    activeSlot.EndControllerHover();
                    if (isLeft) OnLeftSlotHoverEnded?.Invoke(activeSlot);
                    else OnRightSlotHoverEnded?.Invoke(activeSlot);

                    activeSlot = null;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var slot in inventorySlots)
            {
                if (slot == null) continue;
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(slot.transform.position, interactionRadius);
            }
        }
    }
}
