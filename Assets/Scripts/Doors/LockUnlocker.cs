using UnityEngine;

public class LockUnlocker : MonoBehaviour
{
    [SerializeField] private DoorsOpening linkedDoor;
    [SerializeField] private AudioSource unlockSound;
    [SerializeField] private float checkRadius = 0.1f; // distance autour du cadenas

    private bool unlocked = false;

    private void Update()
    {
        if (unlocked) return;

        // Vérifie si la clé est dans le rayon
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

        // Jouer le son
        if (unlockSound != null)
            unlockSound.Play();

        // Déverrouiller la porte
        if (linkedDoor != null)
            linkedDoor.LockDoor(false); // je vais expliquer cette fonction

        // Disparaît en 2 secondes
        StartCoroutine(FadeOutLock());

        // Faire disparaître la clé
        key.SetActive(false);
    }

    private System.Collections.IEnumerator FadeOutLock()
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }

        // Désactiver complètement
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
