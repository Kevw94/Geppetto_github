using UnityEngine;

using System.Collections;

public class MagazineSpawner : MonoBehaviour
{
    public GameObject magazinePrefab;
    public float spawnDelay = 0.5f;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    private bool isSpawning = false;

    private void Start()
    {
        socketInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        SpawnMagazine();
    }

    private void Update()
    {
        if (!socketInteractor.hasSelection && !isSpawning)
        {
            StartCoroutine(SpawnAfterDelay());
        }
    }

    private IEnumerator SpawnAfterDelay()
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnDelay);
        SpawnMagazine();
        isSpawning = false;
    }

    private void SpawnMagazine()
    {
        GameObject newMag = Instantiate(magazinePrefab, transform.position, transform.rotation);
    }
}
