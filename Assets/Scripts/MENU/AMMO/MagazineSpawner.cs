using UnityEngine;
using System.Collections;
using MikeNspired.XRIStarterKit;

public class MagazineSpawner : MonoBehaviour
{
    public GameObject magazinePrefab;
    public float spawnDelay = 0.5f;
    public int magazineSize = 12;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    private bool isSpawning = false;

    private void Start()
    {
        socketInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        TrySpawnMagazine();
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
        TrySpawnMagazine();
        isSpawning = false;
    }

	private void TrySpawnMagazine()
	{
		var ammoManager = AmmoManagerLocator.Instance;
		int currentAmmo = ammoManager?.GetAmmo() ?? -1;
		Debug.Log($"[Spawner] Ammo Available: {currentAmmo}");

		if (ammoManager != null && currentAmmo > 0 && !socketInteractor.hasSelection)
		{
			SpawnMagazine();
		}
		else
		{
			Debug.Log("[Spawner] Not enough ammo to spawn new magazine.");
		}
	}


	private void SpawnMagazine()
    {
        GameObject newMag = Instantiate(magazinePrefab, transform.position, transform.rotation);
    }
}
