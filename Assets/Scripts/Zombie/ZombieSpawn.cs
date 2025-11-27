using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Fixed Zombies (spawn only once)")]
    public Transform[] fixedSpawnPoints;
    public GameObject[] fixedZombiesPrefabs;

    [Header("Random Zombies (spawn over time)")]
    public GameObject[] randomZombiePrefabs;
    public int maxRandomZombies = 10;
    public float spawnRadius = 20f;
    public float spawnInterval = 5f;

    [Header("Exclusion Around Fixed Spawn Points")]
    public float exclusionRadius = 6f;
    public int maxSpawnAttempts = 10;

    [Header("Player Reference")]
    public Transform player;

    private List<GameObject> activeRandomZombies = new List<GameObject>();
    private float nextSpawnTime;


    void Start()
    {
        AutoAssignPlayer();
        SpawnFixedZombies();
        nextSpawnTime = Time.time + spawnInterval;
    }


    void Update()
    {
        // Cleanup destroyed random zombies
        activeRandomZombies.RemoveAll(z => z == null);

        // Random spawn with time interval
        if (Time.time >= nextSpawnTime && activeRandomZombies.Count < maxRandomZombies)
        {
            TrySpawnRandomZombie();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnFixedZombies()
    {
        for (int i = 0; i < fixedSpawnPoints.Length; i++)
        {
            Transform spawnPoint = fixedSpawnPoints[i];

            GameObject prefab = fixedZombiesPrefabs.Length > 0
                ? fixedZombiesPrefabs[Mathf.Min(i, fixedZombiesPrefabs.Length - 1)]
                : randomZombiePrefabs[0];

            GameObject zombie = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            SetZombieTarget(zombie);
        }
    }

    void TrySpawnRandomZombie()
    {
        // Check if too close to any fixed spawn point
        if (!FindValidSpawnPosition(out Vector3 chosenPos))
            return;

        GameObject prefab = randomZombiePrefabs[
            Random.Range(0, randomZombiePrefabs.Length)
        ];

        GameObject zombie = Instantiate(prefab, chosenPos, Quaternion.identity);

        SetZombieTarget(zombie);
        activeRandomZombies.Add(zombie);
    }

    bool FindValidSpawnPosition(out Vector3 result)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector2 rnd = Random.insideUnitCircle * spawnRadius;
            Vector3 pos = new Vector3(
                transform.position.x + rnd.x,
                transform.position.y,
                transform.position.z + rnd.y
            );

            // Check exclusion around fixed spawn points
            if (!IsTooCloseToFixedSpawns(pos))
            {
                result = pos;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    bool IsTooCloseToFixedSpawns(Vector3 pos)
    {
        foreach (var p in fixedSpawnPoints)
        {
            if (p == null) continue;

            if ((pos - p.position).sqrMagnitude < exclusionRadius * exclusionRadius)
                return true;
        }

        return false;
    }

    void SetZombieTarget(GameObject zombie)
    {
        var chase = zombie.GetComponent<ZombieChase>();
        if (chase != null)
            chase.player = player;
    }

    void AutoAssignPlayer()
    {
        if (player != null) return;

        Camera cam = Camera.main;
        if (cam != null)
            player = cam.transform;
    }

    // Use only for debugging purposes
    void OnDrawGizmosSelected()
    {
        // Global spawn radius
        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // Prevent zombies from spawning too close to fixed spawn points
        Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
        if (fixedSpawnPoints != null)
        {
            foreach (var p in fixedSpawnPoints)
            {
                if (p != null)
                    Gizmos.DrawWireSphere(p.position, exclusionRadius);
            }
        }
    }
}
