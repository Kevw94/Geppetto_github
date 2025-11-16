using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombies réguliers (spawn aléatoire)")]
    public GameObject zombiePrefab;
    public int maxZombies = 10;
    public float spawnRadius = 20f;
    public float spawnInterval = 5f;

    [Header("Zombies fixes (une seule fois)")]
    public GameObject[] fixedZombiesPrefabs;
    public Transform[] fixedSpawnPoints;

    [Header("Exclusion (éviter les spawn aléatoires autour des points fixes)")]
    public float exclusionRadius = 5f;
    public int maxSpawnAttempts = 10;

    [Header("Référence du joueur")]
    public Transform player;

    private List<GameObject> activeZombies = new List<GameObject>();
    private float nextSpawnTime;

    void Start()
    {
        if (player == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
                player = cam.transform;
        }

        SpawnFixedZombies();
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        activeZombies.RemoveAll(z => z == null);

        if (Time.time >= nextSpawnTime && activeZombies.Count < maxZombies)
        {
            SpawnRandomZombie();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnFixedZombies()
    {
        for (int i = 0; i < fixedSpawnPoints.Length; i++)
        {
            GameObject prefab = fixedZombiesPrefabs.Length > 0
                ? fixedZombiesPrefabs[Mathf.Min(i, fixedZombiesPrefabs.Length - 1)]
                : zombiePrefab;

            Transform spawnPoint = fixedSpawnPoints[i];
            GameObject zombie = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);


            // Do not add to activeZombies (fixed spawn, not counted for respawn)
            var chase = zombie.GetComponent<ZombieChase>();
            if (chase != null)
                chase.player = player;
        }
    }

    void SpawnRandomZombie()
    {
        bool found = TryFindValidSpawnPosition(out Vector3 spawnPos);

        if (!found)
        {
            // Debug.LogWarning("Impossible de trouver une position de spawn valide");
            return;
        }

        GameObject newZombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

        var chase = newZombie.GetComponent<ZombieChase>();
        if (chase != null)
            chase.player = player;

        activeZombies.Add(newZombie);
    }

    bool TryFindValidSpawnPosition(out Vector3 validPos)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector2 rnd = Random.insideUnitCircle * spawnRadius;
            Vector3 candidate = new Vector3(transform.position.x + rnd.x, transform.position.y, transform.position.z + rnd.y);

            // Check if too close to any fixed spawn point
            bool tooCloseToFixed = false;
            if (fixedSpawnPoints != null && fixedSpawnPoints.Length > 0)
            {
                for (int i = 0; i < fixedSpawnPoints.Length; i++)
                {
                    if (fixedSpawnPoints[i] == null) continue;
                    float sqrDist = (candidate - fixedSpawnPoints[i].position).sqrMagnitude;
                    if (sqrDist < exclusionRadius * exclusionRadius)
                    {
                        tooCloseToFixed = true;
                        break;
                    }
                }
            }

            if (tooCloseToFixed)
                continue;
            validPos = candidate;
            return true;
        }

        validPos = Vector3.zero;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        // Global spawn radius
        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // Prevent zombies from spawning too close to fixed spawn points
        if (fixedSpawnPoints != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
            foreach (var p in fixedSpawnPoints)
            {
                if (p != null)
                    Gizmos.DrawWireSphere(p.position, exclusionRadius);
            }
        }
    }
}
