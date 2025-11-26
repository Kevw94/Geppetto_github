using UnityEngine;

public class LockUnlocker : MonoBehaviour
{
    [SerializeField] private DoorsOpening linkedDoor;
    [SerializeField] private AudioSource unlockSound;
    [SerializeField] private float checkRadius = 0.1f;

    private bool unlocked = false;

    private void Update()
    {
        if (unlocked) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, checkRadius);
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Key"))
            {
                Unlock(col.gameObject);
                break;
            }
        }
    }
    private void Unlock(GameObject key)
    {
        unlocked = true;

        if (unlockSound != null)
            unlockSound.Play();

        if (linkedDoor != null)
            linkedDoor.LockDoor(false);

        key.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
