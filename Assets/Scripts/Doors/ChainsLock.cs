using System.Collections;
using UnityEngine;

public class ChainsLock : MonoBehaviour
{
    [Header("Portes à déverrouiller")]
    [SerializeField] private DoorsOpening leftDoor;
    [SerializeField] private DoorsOpening rightDoor;

    [Header("Son")]
    [SerializeField] private AudioSource cutSound;

    [Header("Disparition des chaînes")]
    [SerializeField] private float fadeDuration = 2f;

    [Header("Distance de détection")]
    [SerializeField] private float detectRadius = 0.3f;

    private bool cut = false;

    private Collider[] childColliders;
    private Renderer[] childRenderers;

    private void Awake()
    {
        // Récupère tous les colliders et renderers des enfants
        childColliders = GetComponentsInChildren<Collider>();
        childRenderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        if (cut) return;

        foreach (Collider col in childColliders)
        {
            Collider[] hits = Physics.OverlapSphere(col.bounds.center, detectRadius);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Boltcutter"))
                {
                    CutChains(hit.gameObject);
                    return;
                }
            }
        }
    }

    private void CutChains(GameObject cutter)
    {
        cut = true;

        if (cutSound != null)
            cutSound.Play();

        if (leftDoor != null) leftDoor.UnlockDoor();
        if (rightDoor != null) rightDoor.UnlockDoor();

        cutter.SetActive(false);

        StartCoroutine(FadeOutChains());
    }

    private IEnumerator FadeOutChains()
    {
        float elapsed = 0f;

        // Stocke les matériaux pour modifier leur alpha
        Material[] materials = new Material[childRenderers.Length];
        for (int i = 0; i < childRenderers.Length; i++)
        {
            materials[i] = childRenderers[i].material;
            Color c = materials[i].color;
            c.a = 1f;
            materials[i].color = c;
        }

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            foreach (Material mat in materials)
            {
                Color c = mat.color;
                c.a = alpha;
                mat.color = c;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
