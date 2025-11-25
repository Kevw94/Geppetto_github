using UnityEngine;

public class KeyLock : MonoBehaviour
{
    [Header("Clé requise")]
    [SerializeField] private GameObject requiredKey;

    [Header("Porte à déverrouiller")]
    [SerializeField] private DoorsOpening door;

    [Header("Sons")]
    [SerializeField] private AudioSource unlockSound;
    [SerializeField] private AudioSource wrongKeySound;

    private bool unlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (unlocked) return;

        if (other.gameObject == requiredKey)
        {
            UnlockDoor();
        }
        else
        {
            WrongKey();
        }
    }

    private void UnlockDoor()
    {
        unlocked = true;

        if (door != null)
            door.UnlockDoor();

        if (unlockSound != null)
            unlockSound.Play();

        Debug.Log("🔓 Clé correcte → porte déverrouillée !");
    }

    private void WrongKey()
    {
        if (wrongKeySound != null)
            wrongKeySound.Play();

        Debug.Log("❌ Mauvaise clé !");
    }
}
