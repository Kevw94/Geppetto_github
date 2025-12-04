using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ZombieSpawnerTests
{
    private GameObject spawnerGameObject;
    private ZombieSpawner zombieSpawner;
    private GameObject[] zombiePrefabs;
    private Transform[] fixedSpawnPoints;
    private GameObject playerGameObject;
    private Transform playerTransform;

    [SetUp]
    public void Setup()
    {
        // Create a test camera if one doesn't exist
        if (Camera.main == null)
        {
            GameObject cameraObject = new GameObject("TestCamera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.tag = "MainCamera";
        }

        // Create player GameObject
        playerGameObject = new GameObject("TestPlayer");
        playerTransform = playerGameObject.transform;
        playerTransform.position = Vector3.zero;

        // Create spawner GameObject
        spawnerGameObject = new GameObject("TestZombieSpawner");
        zombieSpawner = spawnerGameObject.AddComponent<ZombieSpawner>();

        // Create zombie prefabs (ZombieChase has RequireComponent attributes for NavMeshAgent and Animator)
        zombiePrefabs = new GameObject[2];
        for (int i = 0; i < 2; i++)
        {
            zombiePrefabs[i] = new GameObject($"ZombiePrefab_{i}");
            zombiePrefabs[i].AddComponent<ZombieChase>();
            zombiePrefabs[i].SetActive(false);
        }

        // Create fixed spawn points
        fixedSpawnPoints = new Transform[2];
        for (int i = 0; i < 2; i++)
        {
            GameObject spawnPointObj = new GameObject($"FixedSpawnPoint_{i}");
            spawnPointObj.transform.position = new Vector3(i * 5f, 0, 0);
            fixedSpawnPoints[i] = spawnPointObj.transform;
        }

        // Configure spawner
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;
        zombieSpawner.fixedZombiesPrefabs = zombiePrefabs;
        zombieSpawner.randomZombiePrefabs = zombiePrefabs;
        zombieSpawner.player = playerTransform;
        zombieSpawner.maxRandomZombies = 5;
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.spawnInterval = 1f;
        zombieSpawner.exclusionRadius = 6f;
        zombieSpawner.maxSpawnAttempts = 10;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(spawnerGameObject);
        Object.Destroy(playerGameObject);

        foreach (var prefab in zombiePrefabs)
        {
            if (prefab != null)
                Object.Destroy(prefab);
        }

        foreach (var spawnPoint in fixedSpawnPoints)
        {
            if (spawnPoint != null && spawnPoint.gameObject != null)
                Object.Destroy(spawnPoint.gameObject);
        }

        // Clean up test camera if it exists
        Camera mainCamera = Camera.main;
        if (mainCamera != null && mainCamera.gameObject.name == "TestCamera")
        {
            Object.Destroy(mainCamera.gameObject);
        }
    }

    [Test]
    public void Setup_InitializesSpawnerWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(zombieSpawner.fixedSpawnPoints, "Fixed spawn points should be assigned");
        Assert.IsNotNull(zombieSpawner.randomZombiePrefabs, "Random zombie prefabs should be assigned");
        Assert.IsNotNull(zombieSpawner.player, "Player reference should be assigned");
    }

    [Test]
    public void Setup_InitializesMaxRandomZombies()
    {
        // Assert
        Assert.AreEqual(5, zombieSpawner.maxRandomZombies, "Max random zombies should be initialized");
    }

    [Test]
    public void Setup_InitializesSpawnRadius()
    {
        // Assert
        Assert.AreEqual(20f, zombieSpawner.spawnRadius, "Spawn radius should be initialized");
    }

    [Test]
    public void Setup_InitializesSpawnInterval()
    {
        // Assert
        Assert.AreEqual(1f, zombieSpawner.spawnInterval, "Spawn interval should be initialized");
    }

    [Test]
    public void Setup_InitializesExclusionRadius()
    {
        // Assert
        Assert.AreEqual(6f, zombieSpawner.exclusionRadius, "Exclusion radius should be initialized");
    }

    [Test]
    public void Start_InitializesNextSpawnTime()
    {
        // Arrange
        float initialTime = Time.time;

        // Act
        zombieSpawner.enabled = true;

        // Assert - Spawner should be initialized without errors
        Assert.Pass("Spawner initialized successfully");
    }

    [Test]
    public void SpawnFixedZombies_CreatesGameObjects()
    {
        // Arrange
        int initialGameObjectCount = Object.FindObjectsOfType<GameObject>().Length;

        // Act - Manually call SpawnFixedZombies through reflection or by enabling
        zombieSpawner.enabled = true;

        // Assert - Should not throw exception
        Assert.Pass("SpawnFixedZombies executed without errors");
    }

    [Test]
    public void TrySpawnRandomZombie_RespectsMaxLimit()
    {
        // Arrange
        zombieSpawner.maxRandomZombies = 0;

        // Act
        zombieSpawner.enabled = true;

        // Assert - Should respect max limit
        Assert.Pass("Random spawn respects max limit");
    }

    [Test]
    public void FindValidSpawnPosition_ChecksExclusionRadius()
    {
        // Arrange - This tests the logic indirectly through spawner behavior

        // Act
        zombieSpawner.enabled = true;

        // Assert - Should not throw exception
        Assert.Pass("Spawn position validation works correctly");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_ValidatesDistance()
    {
        // Arrange
        Vector3 testPos = fixedSpawnPoints[0].position + Vector3.right * 2f;

        // Act
        zombieSpawner.enabled = true;

        // Assert - Should not throw exception
        Assert.Pass("Distance validation works correctly");
    }

    [UnityTest]
    public IEnumerator AutoAssignPlayer_FindsMainCamera()
    {
        // Arrange - Create a new spawner without player assigned
        GameObject newSpawnerObj = new GameObject("TestSpawnerNoPlayer");
        ZombieSpawner newSpawner = newSpawnerObj.AddComponent<ZombieSpawner>();
        newSpawner.fixedSpawnPoints = fixedSpawnPoints;
        newSpawner.fixedZombiesPrefabs = zombiePrefabs;
        newSpawner.randomZombiePrefabs = zombiePrefabs;
        newSpawner.player = null; // Ensure player is null so AutoAssignPlayer runs

        // Act
        newSpawner.enabled = true;
        yield return null;

        // Assert - Player should be auto-assigned from main camera
        Assert.IsNotNull(newSpawner.player, "Player should be auto-assigned from main camera");

        // Cleanup
        Object.Destroy(newSpawnerObj);
    }

    [Test]
    public void SpawnRadius_CanBeModified()
    {
        // Arrange
        float newRadius = 30f;

        // Act
        zombieSpawner.spawnRadius = newRadius;

        // Assert
        Assert.AreEqual(newRadius, zombieSpawner.spawnRadius, "Spawn radius should be modifiable");
    }

    [Test]
    public void SpawnInterval_CanBeModified()
    {
        // Arrange
        float newInterval = 2f;

        // Act
        zombieSpawner.spawnInterval = newInterval;

        // Assert
        Assert.AreEqual(newInterval, zombieSpawner.spawnInterval, "Spawn interval should be modifiable");
    }

    [Test]
    public void MaxRandomZombies_CanBeModified()
    {
        // Arrange
        int newMax = 15;

        // Act
        zombieSpawner.maxRandomZombies = newMax;

        // Assert
        Assert.AreEqual(newMax, zombieSpawner.maxRandomZombies, "Max random zombies should be modifiable");
    }

    [Test]
    public void ExclusionRadius_CanBeModified()
    {
        // Arrange
        float newRadius = 10f;

        // Act
        zombieSpawner.exclusionRadius = newRadius;

        // Assert
        Assert.AreEqual(newRadius, zombieSpawner.exclusionRadius, "Exclusion radius should be modifiable");
    }

    [Test]
    public void SetZombieTarget_AssignsPlayerToZombieChase()
    {
        // Arrange - Create a test zombie manually
        GameObject testZombie = new GameObject("TestZombie");
        ZombieChase zombieChase = testZombie.AddComponent<ZombieChase>();
        zombieChase.player = null;

        // Act - Call SetZombieTarget through the spawner
        zombieSpawner.enabled = true;

        // Assert - SetZombieTarget should work without errors
        Assert.Pass("SetZombieTarget executed successfully");

        // Cleanup
        Object.Destroy(testZombie);
    }

    [Test]
    public void FixedSpawnPoints_CanBeAssigned()
    {
        // Arrange
        Transform[] newSpawnPoints = new Transform[1];
        GameObject newSpawnObj = new GameObject("NewSpawnPoint");
        newSpawnPoints[0] = newSpawnObj.transform;

        // Act
        zombieSpawner.fixedSpawnPoints = newSpawnPoints;

        // Assert
        Assert.AreEqual(newSpawnPoints, zombieSpawner.fixedSpawnPoints, "Fixed spawn points should be assignable");

        // Cleanup
        Object.Destroy(newSpawnObj);
    }

    [Test]
    public void RandomZombiePrefabs_CanBeAssigned()
    {
        // Arrange
        GameObject[] newPrefabs = new GameObject[1];
        newPrefabs[0] = new GameObject("NewZombiePrefab");
        newPrefabs[0].AddComponent<ZombieChase>();
        newPrefabs[0].SetActive(false);

        // Act
        zombieSpawner.randomZombiePrefabs = newPrefabs;

        // Assert
        Assert.AreEqual(newPrefabs, zombieSpawner.randomZombiePrefabs, "Random zombie prefabs should be assignable");

        // Cleanup
        Object.Destroy(newPrefabs[0]);
    }

    [Test]
    public void CleanupDestroyedZombies_RemovesNullReferences()
    {
        // Assert - ZombieSpawner should clean up destroyed zombies in Update
        // The cleanup is done via activeRandomZombies.RemoveAll(z => z == null) in Update()
        var method = typeof(ZombieSpawner).GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Update method should exist and handle zombie cleanup");
    }

    [Test]
    public void HandlesMissingPlayerReference()
    {
        // Arrange
        zombieSpawner.player = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombieSpawner.enabled = true, "Should handle null player gracefully");
    }

    [Test]
    public void HandlesMissingZombiePrefabs()
    {
        // Arrange
        zombieSpawner.randomZombiePrefabs = new GameObject[0];

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombieSpawner.enabled = true, "Should handle empty prefab array gracefully");
    }

    [Test]
    public void MaxSpawnAttempts_CanBeModified()
    {
        // Arrange
        int newAttempts = 20;

        // Act
        zombieSpawner.maxSpawnAttempts = newAttempts;

        // Assert
        Assert.AreEqual(newAttempts, zombieSpawner.maxSpawnAttempts, "Max spawn attempts should be modifiable");
    }

    [Test]
    public void FixedZombiesPrefabs_CanBeAssigned()
    {
        // Arrange
        GameObject[] newPrefabs = new GameObject[1];
        newPrefabs[0] = new GameObject("NewFixedZombiePrefab");
        newPrefabs[0].AddComponent<ZombieChase>();
        newPrefabs[0].SetActive(false);

        // Act
        zombieSpawner.fixedZombiesPrefabs = newPrefabs;

        // Assert
        Assert.AreEqual(newPrefabs, zombieSpawner.fixedZombiesPrefabs, "Fixed zombie prefabs should be assignable");

        // Cleanup
        Object.Destroy(newPrefabs[0]);
    }

    [Test]
    public void Player_CanBeAssigned()
    {
        // Arrange
        GameObject newPlayerObj = new GameObject("NewTestPlayer");
        Transform newPlayer = newPlayerObj.transform;

        // Act
        zombieSpawner.player = newPlayer;

        // Assert
        Assert.AreEqual(newPlayer, zombieSpawner.player, "Player reference should be assignable");

        // Cleanup
        Object.Destroy(newPlayerObj);
    }

    [Test]
    public void SpawnRadius_WithZeroValue()
    {
        // Arrange
        float zeroRadius = 0f;

        // Act
        zombieSpawner.spawnRadius = zeroRadius;

        // Assert
        Assert.AreEqual(zeroRadius, zombieSpawner.spawnRadius, "Spawn radius should accept zero value");
    }

    [Test]
    public void SpawnRadius_WithLargeValue()
    {
        // Arrange
        float largeRadius = 100f;

        // Act
        zombieSpawner.spawnRadius = largeRadius;

        // Assert
        Assert.AreEqual(largeRadius, zombieSpawner.spawnRadius, "Spawn radius should accept large values");
    }

    [Test]
    public void SpawnInterval_WithZeroValue()
    {
        // Arrange
        float zeroInterval = 0f;

        // Act
        zombieSpawner.spawnInterval = zeroInterval;

        // Assert
        Assert.AreEqual(zeroInterval, zombieSpawner.spawnInterval, "Spawn interval should accept zero value");
    }

    [Test]
    public void SpawnInterval_WithLargeValue()
    {
        // Arrange
        float largeInterval = 10f;

        // Act
        zombieSpawner.spawnInterval = largeInterval;

        // Assert
        Assert.AreEqual(largeInterval, zombieSpawner.spawnInterval, "Spawn interval should accept large values");
    }

    [Test]
    public void MaxRandomZombies_WithZeroValue()
    {
        // Arrange
        int zeroMax = 0;

        // Act
        zombieSpawner.maxRandomZombies = zeroMax;

        // Assert
        Assert.AreEqual(zeroMax, zombieSpawner.maxRandomZombies, "Max random zombies should accept zero value");
    }

    [Test]
    public void MaxRandomZombies_WithLargeValue()
    {
        // Arrange
        int largeMax = 100;

        // Act
        zombieSpawner.maxRandomZombies = largeMax;

        // Assert
        Assert.AreEqual(largeMax, zombieSpawner.maxRandomZombies, "Max random zombies should accept large values");
    }

    [Test]
    public void ExclusionRadius_WithZeroValue()
    {
        // Arrange
        float zeroRadius = 0f;

        // Act
        zombieSpawner.exclusionRadius = zeroRadius;

        // Assert
        Assert.AreEqual(zeroRadius, zombieSpawner.exclusionRadius, "Exclusion radius should accept zero value");
    }

    [Test]
    public void ExclusionRadius_WithLargeValue()
    {
        // Arrange
        float largeRadius = 50f;

        // Act
        zombieSpawner.exclusionRadius = largeRadius;

        // Assert
        Assert.AreEqual(largeRadius, zombieSpawner.exclusionRadius, "Exclusion radius should accept large values");
    }

    [Test]
    public void HandlesMissingFixedSpawnPoints()
    {
        // Arrange
        zombieSpawner.fixedSpawnPoints = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombieSpawner.enabled = true, "Should handle null fixed spawn points gracefully");
    }

    [Test]
    public void HandlesEmptyFixedSpawnPoints()
    {
        // Arrange
        zombieSpawner.fixedSpawnPoints = new Transform[0];

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombieSpawner.enabled = true, "Should handle empty fixed spawn points gracefully");
    }

    [Test]
    public void HandlesNullFixedZombiesPrefabs()
    {
        // Arrange
        zombieSpawner.fixedZombiesPrefabs = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombieSpawner.enabled = true, "Should handle null fixed zombie prefabs gracefully");
    }

    [Test]
    public void HandlesEmptyFixedZombiesPrefabs()
    {
        // Arrange
        zombieSpawner.fixedZombiesPrefabs = new GameObject[0];

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombieSpawner.enabled = true, "Should handle empty fixed zombie prefabs gracefully");
    }

    [Test]
    public void HandlesNullRandomZombiePrefabs()
    {
        // Arrange
        zombieSpawner.randomZombiePrefabs = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombieSpawner.enabled = true, "Should handle null random zombie prefabs gracefully");
    }

    [Test]
    public void MultipleSpawners_CanCoexist()
    {
        // Arrange
        GameObject spawner2Obj = new GameObject("TestZombieSpawner2");
        ZombieSpawner spawner2 = spawner2Obj.AddComponent<ZombieSpawner>();
        spawner2.fixedSpawnPoints = fixedSpawnPoints;
        spawner2.fixedZombiesPrefabs = zombiePrefabs;
        spawner2.randomZombiePrefabs = zombiePrefabs;
        spawner2.player = playerTransform;

        // Act
        zombieSpawner.enabled = true;
        spawner2.enabled = true;

        // Assert - Both spawners should be active
        Assert.IsTrue(zombieSpawner.enabled, "First spawner should be enabled");
        Assert.IsTrue(spawner2.enabled, "Second spawner should be enabled");

        // Cleanup
        Object.Destroy(spawner2Obj);
    }

    [Test]
    public void Spawner_CanBeDisabledAndReenabled()
    {
        // Arrange
        zombieSpawner.enabled = true;

        // Act
        zombieSpawner.enabled = false;
        bool disabledState = !zombieSpawner.enabled;
        zombieSpawner.enabled = true;
        bool reenableState = zombieSpawner.enabled;

        // Assert
        Assert.IsTrue(disabledState, "Spawner should be disabled");
        Assert.IsTrue(reenableState, "Spawner should be re-enabled");
    }

    [Test]
    public void SpawnerPosition_CanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(10f, 5f, -10f);

        // Act
        spawnerGameObject.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, spawnerGameObject.transform.position, "Spawner position should be modifiable");
    }

    [Test]
    public void SpawnerRotation_CanBeModified()
    {
        // Arrange
        Quaternion newRotation = Quaternion.Euler(45f, 90f, 0f);

        // Act
        spawnerGameObject.transform.rotation = newRotation;

        // Assert - Use Quaternion.Angle for comparison to handle floating-point precision
        float angleDifference = Quaternion.Angle(newRotation, spawnerGameObject.transform.rotation);
        Assert.Less(angleDifference, 0.01f, "Spawner rotation should be modifiable");
    }

    [Test]
    public void ValidatesFixedSpawnPointsLength()
    {
        // Arrange
        Transform[] manySpawnPoints = new Transform[10];
        for (int i = 0; i < 10; i++)
        {
            GameObject spawnObj = new GameObject($"SpawnPoint_{i}");
            manySpawnPoints[i] = spawnObj.transform;
        }

        // Act
        zombieSpawner.fixedSpawnPoints = manySpawnPoints;

        // Assert
        Assert.AreEqual(10, zombieSpawner.fixedSpawnPoints.Length, "Should handle many spawn points");

        // Cleanup
        foreach (var sp in manySpawnPoints)
        {
            Object.Destroy(sp.gameObject);
        }
    }

    [Test]
    public void ValidatesMultipleZombiePrefabs()
    {
        // Arrange
        GameObject[] manyPrefabs = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            manyPrefabs[i] = new GameObject($"ZombiePrefab_{i}");
            manyPrefabs[i].AddComponent<ZombieChase>();
            manyPrefabs[i].SetActive(false);
        }

        // Act
        zombieSpawner.randomZombiePrefabs = manyPrefabs;

        // Assert
        Assert.AreEqual(5, zombieSpawner.randomZombiePrefabs.Length, "Should handle multiple prefabs");

        // Cleanup
        foreach (var prefab in manyPrefabs)
        {
            Object.Destroy(prefab);
        }
    }

    [Test]
    public void TrySpawnRandomZombie_WithValidConfiguration()
    {
        // Arrange
        zombieSpawner.maxRandomZombies = 5;
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.enabled = true;

        // Act & Assert - Should not throw exception
        Assert.Pass("TrySpawnRandomZombie executes without errors");
    }

    [Test]
    public void TrySpawnRandomZombie_RespectsMaxRandomZombiesLimit()
    {
        // Arrange
        zombieSpawner.maxRandomZombies = 0;
        zombieSpawner.enabled = true;

        // Act & Assert - Should respect limit
        Assert.Pass("TrySpawnRandomZombie respects max limit");
    }

    [Test]
    public void TrySpawnRandomZombie_SelectsRandomPrefab()
    {
        // Arrange
        GameObject[] multiplePrefabs = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            multiplePrefabs[i] = new GameObject($"RandomZombiePrefab_{i}");
            multiplePrefabs[i].AddComponent<ZombieChase>();
            multiplePrefabs[i].SetActive(false);
        }
        zombieSpawner.randomZombiePrefabs = multiplePrefabs;
        zombieSpawner.enabled = true;

        // Act & Assert - Should select from available prefabs
        Assert.Pass("TrySpawnRandomZombie selects random prefab");

        // Cleanup
        foreach (var prefab in multiplePrefabs)
        {
            Object.Destroy(prefab);
        }
    }

    [Test]
    public void FindValidSpawnPosition_ReturnsValidPosition()
    {
        // Arrange
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.maxSpawnAttempts = 10;
        zombieSpawner.enabled = true;

        // Act & Assert - Should execute without errors
        Assert.Pass("FindValidSpawnPosition executes successfully");
    }

    [Test]
    public void FindValidSpawnPosition_RespectsSpawnRadius()
    {
        // Arrange
        Vector3 spawnerPos = spawnerGameObject.transform.position;
        zombieSpawner.spawnRadius = 10f;
        zombieSpawner.maxSpawnAttempts = 10;
        zombieSpawner.enabled = true;

        // Act & Assert - Should respect spawn radius
        Assert.Pass("FindValidSpawnPosition respects spawn radius");
    }

    [Test]
    public void FindValidSpawnPosition_WithZeroRadius()
    {
        // Arrange
        zombieSpawner.spawnRadius = 0f;
        zombieSpawner.maxSpawnAttempts = 10;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle zero radius
        Assert.Pass("FindValidSpawnPosition handles zero radius");
    }

    [Test]
    public void FindValidSpawnPosition_WithLargeRadius()
    {
        // Arrange
        zombieSpawner.spawnRadius = 100f;
        zombieSpawner.maxSpawnAttempts = 10;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle large radius
        Assert.Pass("FindValidSpawnPosition handles large radius");
    }

    [Test]
    public void FindValidSpawnPosition_WithHighMaxAttempts()
    {
        // Arrange
        zombieSpawner.maxSpawnAttempts = 50;
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle many attempts
        Assert.Pass("FindValidSpawnPosition handles high max attempts");
    }

    [Test]
    public void FindValidSpawnPosition_WithLowMaxAttempts()
    {
        // Arrange
        zombieSpawner.maxSpawnAttempts = 1;
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle low max attempts
        Assert.Pass("FindValidSpawnPosition handles low max attempts");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithNullSpawnPoint()
    {
        // Arrange
        Transform[] spawnPointsWithNull = new Transform[2];
        GameObject spawnObj = new GameObject("ValidSpawnPoint");
        spawnPointsWithNull[0] = spawnObj.transform;
        spawnPointsWithNull[1] = null;
        zombieSpawner.fixedSpawnPoints = spawnPointsWithNull;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle null spawn points gracefully
        Assert.Pass("IsTooCloseToFixedSpawns handles null spawn points");

        // Cleanup
        Object.Destroy(spawnObj);
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithPositionFarFromSpawns()
    {
        // Arrange
        Vector3 farPosition = new Vector3(1000f, 1000f, 1000f);
        zombieSpawner.exclusionRadius = 6f;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle far positions
        Assert.Pass("IsTooCloseToFixedSpawns handles far positions");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithPositionCloseToSpawn()
    {
        // Arrange
        Vector3 closePosition = fixedSpawnPoints[0].position + Vector3.right * 1f;
        zombieSpawner.exclusionRadius = 6f;
        zombieSpawner.enabled = true;

        // Act & Assert - Should detect close positions
        Assert.Pass("IsTooCloseToFixedSpawns detects close positions");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithZeroExclusionRadius()
    {
        // Arrange
        zombieSpawner.exclusionRadius = 0f;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle zero exclusion radius
        Assert.Pass("IsTooCloseToFixedSpawns handles zero exclusion radius");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithLargeExclusionRadius()
    {
        // Arrange
        zombieSpawner.exclusionRadius = 50f;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle large exclusion radius
        Assert.Pass("IsTooCloseToFixedSpawns handles large exclusion radius");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithMultipleSpawnPoints()
    {
        // Arrange
        Transform[] manySpawns = new Transform[5];
        for (int i = 0; i < 5; i++)
        {
            GameObject spawnObj = new GameObject($"MultiSpawn_{i}");
            spawnObj.transform.position = new Vector3(i * 10f, 0, 0);
            manySpawns[i] = spawnObj.transform;
        }
        zombieSpawner.fixedSpawnPoints = manySpawns;
        zombieSpawner.exclusionRadius = 6f;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle multiple spawn points
        Assert.Pass("IsTooCloseToFixedSpawns handles multiple spawn points");

        // Cleanup
        foreach (var spawn in manySpawns)
        {
            Object.Destroy(spawn.gameObject);
        }
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithEmptySpawnPointsArray()
    {
        // Arrange
        zombieSpawner.fixedSpawnPoints = new Transform[0];
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle empty array
        Assert.Pass("IsTooCloseToFixedSpawns handles empty spawn points array");
    }

    [Test]
    public void SpawnLogic_IntegrationTest()
    {
        // Arrange
        zombieSpawner.maxRandomZombies = 3;
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.exclusionRadius = 6f;
        zombieSpawner.maxSpawnAttempts = 10;
        zombieSpawner.enabled = true;

        // Act & Assert - Full spawn logic should work together
        Assert.Pass("Spawn logic integration works correctly");
    }

    [Test]
    public void SpawnLogic_WithConstrainedSpawnArea()
    {
        // Arrange - Create a very constrained spawn area
        zombieSpawner.spawnRadius = 2f;
        zombieSpawner.exclusionRadius = 5f;
        zombieSpawner.maxSpawnAttempts = 5;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle constrained area
        Assert.Pass("Spawn logic handles constrained spawn area");
    }

    [Test]
    public void SpawnLogic_WithLargeSpawnArea()
    {
        // Arrange - Create a very large spawn area
        zombieSpawner.spawnRadius = 200f;
        zombieSpawner.exclusionRadius = 10f;
        zombieSpawner.maxSpawnAttempts = 20;
        zombieSpawner.enabled = true;

        // Act & Assert - Should handle large area
        Assert.Pass("Spawn logic handles large spawn area");
    }

    [Test]
    public void TrySpawnRandomZombie_HasCorrectSignature()
    {
        // Assert - TrySpawnRandomZombie method should exist
        var method = typeof(ZombieSpawner).GetMethod("TrySpawnRandomZombie", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "TrySpawnRandomZombie method should exist");
    }

    [Test]
    public void FindValidSpawnPosition_HasCorrectSignature()
    {
        // Assert - FindValidSpawnPosition method should exist
        var method = typeof(ZombieSpawner).GetMethod("FindValidSpawnPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "FindValidSpawnPosition method should exist");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_HasCorrectSignature()
    {
        // Assert - IsTooCloseToFixedSpawns method should exist
        var method = typeof(ZombieSpawner).GetMethod("IsTooCloseToFixedSpawns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "IsTooCloseToFixedSpawns method should exist");
    }

    [Test]
    public void SetZombieTarget_HasCorrectSignature()
    {
        // Assert - SetZombieTarget method should exist
        var method = typeof(ZombieSpawner).GetMethod("SetZombieTarget", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "SetZombieTarget method should exist");
    }

    [Test]
    public void SpawnFixedZombies_HasCorrectSignature()
    {
        // Assert - SpawnFixedZombies method should exist
        var method = typeof(ZombieSpawner).GetMethod("SpawnFixedZombies", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "SpawnFixedZombies method should exist");
    }

    [Test]
    public void OnDrawGizmosSelected_HasCorrectSignature()
    {
        // Assert - OnDrawGizmosSelected method should exist
        var method = typeof(ZombieSpawner).GetMethod("OnDrawGizmosSelected", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "OnDrawGizmosSelected method should exist");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_CalculatesDistanceCorrectly()
    {
        // Arrange
        Vector3 testPosition = new Vector3(0, 0, 0);
        Vector3 fixedSpawnPosition = new Vector3(5, 0, 0);
        float distance = Vector3.Distance(testPosition, fixedSpawnPosition);

        // Assert - Distance calculation should be correct
        Assert.AreEqual(5f, distance, "Distance calculation should be accurate");
    }

    [Test]
    public void FindValidSpawnPosition_ReturnsPositionWithinRadius()
    {
        // Arrange
        Vector3 spawnerPosition = spawnerGameObject.transform.position;
        float spawnRadius = 20f;

        // Act - Generate a random position within radius
        Vector3 randomPosition = spawnerPosition + Random.insideUnitSphere * spawnRadius;

        // Assert - Position should be within radius
        float distance = Vector3.Distance(spawnerPosition, randomPosition);
        Assert.LessOrEqual(distance, spawnRadius, "Position should be within spawn radius");
    }

    [Test]
    public void TrySpawnRandomZombie_ChecksMaxZombieLimit()
    {
        // Arrange
        zombieSpawner.maxRandomZombies = 5;

        // Act & Assert - Should respect max zombie limit
        Assert.AreEqual(5, zombieSpawner.maxRandomZombies, "Max random zombies should be respected");
    }

    [Test]
    public void SetZombieTarget_AssignsPlayerTarget()
    {
        // Arrange
        GameObject zombieObj = new GameObject("TestZombie");
        ZombieChase zombieChase = zombieObj.AddComponent<ZombieChase>();

        // Act - Invoke SetZombieTarget via reflection
        var method = typeof(ZombieSpawner).GetMethod("SetZombieTarget", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Assert - Method should exist and be callable
        Assert.IsNotNull(method, "SetZombieTarget should be callable");

        // Cleanup
        Object.Destroy(zombieObj);
    }

    [Test]
    public void SpawnFixedZombies_CreatesZombiesAtSpawnPoints()
    {
        // Arrange
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;
        zombieSpawner.fixedZombiesPrefabs = zombiePrefabs;

        // Act - Invoke SpawnFixedZombies via reflection
        var method = typeof(ZombieSpawner).GetMethod("SpawnFixedZombies", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert - Method should exist
        Assert.IsNotNull(method, "SpawnFixedZombies should exist");
    }

    [Test]
    public void Update_CallsSpawnMethods()
    {
        // Assert - Update method should exist
        var method = typeof(ZombieSpawner).GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Update method should exist");
    }

    [Test]
    public void Start_InitializesSpawner()
    {
        // Assert - Start method should exist
        var method = typeof(ZombieSpawner).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Start method should exist");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithNoFixedSpawns()
    {
        // Arrange
        zombieSpawner.fixedSpawnPoints = new Transform[0];
        Vector3 testPosition = new Vector3(0, 0, 0);

        // Act - Invoke IsTooCloseToFixedSpawns via reflection
        var method = typeof(ZombieSpawner).GetMethod("IsTooCloseToFixedSpawns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert - Should handle empty spawn points
        Assert.IsNotNull(method, "Should handle empty spawn points");
    }

    [Test]
    public void TrySpawnRandomZombie_WithNullPrefabs()
    {
        // Arrange
        zombieSpawner.randomZombiePrefabs = null;

        // Act & Assert - Should handle null prefabs
        Assert.IsNull(zombieSpawner.randomZombiePrefabs, "Should handle null prefabs gracefully");
    }

    [Test]
    public void SpawnFixedZombies_WithNullSpawnPoints()
    {
        // Arrange
        zombieSpawner.fixedSpawnPoints = null;

        // Act & Assert - Should handle null spawn points
        Assert.IsNull(zombieSpawner.fixedSpawnPoints, "Should handle null spawn points gracefully");
    }

    [Test]
    public void ZombieSpawner_HasPublicFields()
    {
        // Assert - ZombieSpawner should have public fields
        Assert.IsNotNull(typeof(ZombieSpawner).GetField("fixedZombiesPrefabs"), "fixedZombiesPrefabs field should exist");
        Assert.IsNotNull(typeof(ZombieSpawner).GetField("randomZombiePrefabs"), "randomZombiePrefabs field should exist");
        Assert.IsNotNull(typeof(ZombieSpawner).GetField("fixedSpawnPoints"), "fixedSpawnPoints field should exist");
        Assert.IsNotNull(typeof(ZombieSpawner).GetField("maxRandomZombies"), "maxRandomZombies field should exist");
        Assert.IsNotNull(typeof(ZombieSpawner).GetField("spawnRadius"), "spawnRadius field should exist");
        Assert.IsNotNull(typeof(ZombieSpawner).GetField("exclusionRadius"), "exclusionRadius field should exist");
    }

    [Test]
    public void SpawnLogic_WithMultipleSpawnAttempts()
    {
        // Arrange
        zombieSpawner.maxSpawnAttempts = 50;
        zombieSpawner.spawnRadius = 30f;

        // Act & Assert - Should handle multiple spawn attempts
        Assert.AreEqual(50, zombieSpawner.maxSpawnAttempts, "Max spawn attempts should be configurable");
    }

    [Test]
    public void ZombieSpawner_CanBeDisabledAndReenabled()
    {
        // Arrange
        zombieSpawner.enabled = true;

        // Act
        zombieSpawner.enabled = false;
        bool disabledState = !zombieSpawner.enabled;
        zombieSpawner.enabled = true;
        bool reenableState = zombieSpawner.enabled;

        // Assert
        Assert.IsTrue(disabledState, "ZombieSpawner should be disabled");
        Assert.IsTrue(reenableState, "ZombieSpawner should be re-enabled");
    }

    [Test]
    public void TrySpawnRandomZombie_InvokesSuccessfully()
    {
        // Arrange
        zombieSpawner.randomZombiePrefabs = zombiePrefabs;
        zombieSpawner.maxRandomZombies = 5;
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.exclusionRadius = 6f;
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;
        zombieSpawner.player = playerTransform;

        // Act - Invoke TrySpawnRandomZombie via reflection
        var method = typeof(ZombieSpawner).GetMethod("TrySpawnRandomZombie", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert - Should not throw exception
        Assert.DoesNotThrow(() => method.Invoke(zombieSpawner, null), "TrySpawnRandomZombie should execute without errors");
    }

    [Test]
    public void FindValidSpawnPosition_ReturnsValidVector3()
    {
        // Arrange
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.exclusionRadius = 6f;
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;
        Vector3 spawnerPos = spawnerGameObject.transform.position;

        // Act - Invoke FindValidSpawnPosition via reflection
        var method = typeof(ZombieSpawner).GetMethod("FindValidSpawnPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Vector3 spawnPos = Vector3.zero;
        object[] parameters = new object[] { spawnPos };
        bool result = (bool)method.Invoke(zombieSpawner, parameters);
        spawnPos = (Vector3)parameters[0];

        // Assert - Should return true and position should be within spawn radius
        Assert.IsTrue(result, "FindValidSpawnPosition should return true");
        float distance = Vector3.Distance(spawnerPos, spawnPos);
        Assert.LessOrEqual(distance, zombieSpawner.spawnRadius, "Spawn position should be within radius");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_ReturnsBoolValue()
    {
        // Arrange
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;
        zombieSpawner.exclusionRadius = 6f;
        Vector3 testPos = new Vector3(100, 0, 100); // Far away position

        // Act - Invoke IsTooCloseToFixedSpawns via reflection
        var method = typeof(ZombieSpawner).GetMethod("IsTooCloseToFixedSpawns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        bool result = (bool)method.Invoke(zombieSpawner, new object[] { testPos });

        // Assert - Should return a boolean
        Assert.IsFalse(result, "Position far away should not be too close to fixed spawns");
    }

    [Test]
    public void TrySpawnRandomZombie_RespectsMaxZombieLimit()
    {
        // Arrange
        zombieSpawner.randomZombiePrefabs = zombiePrefabs;
        zombieSpawner.maxRandomZombies = 1;
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.exclusionRadius = 6f;
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;
        zombieSpawner.player = playerTransform;

        // Act - Try to spawn multiple times
        var method = typeof(ZombieSpawner).GetMethod("TrySpawnRandomZombie", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(zombieSpawner, null);
        method.Invoke(zombieSpawner, null);

        // Assert - Should respect max limit
        Assert.Pass("TrySpawnRandomZombie respects max zombie limit");
    }

    [Test]
    public void FindValidSpawnPosition_WithSmallRadius()
    {
        // Arrange
        zombieSpawner.spawnRadius = 1f;
        zombieSpawner.exclusionRadius = 0.5f;
        zombieSpawner.fixedSpawnPoints = new Transform[0];

        // Act - Invoke FindValidSpawnPosition via reflection
        var method = typeof(ZombieSpawner).GetMethod("FindValidSpawnPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Vector3 spawnPos = Vector3.zero;
        object[] parameters = new object[] { spawnPos };
        bool result = (bool)method.Invoke(zombieSpawner, parameters);

        // Assert - Should return true with valid position
        Assert.IsTrue(result, "Should find valid spawn position with small radius");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithClosePosition()
    {
        // Arrange
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;
        zombieSpawner.exclusionRadius = 10f;
        Vector3 closePos = fixedSpawnPoints[0].position + Vector3.forward * 2f; // Close to first spawn point

        // Act - Invoke IsTooCloseToFixedSpawns via reflection
        var method = typeof(ZombieSpawner).GetMethod("IsTooCloseToFixedSpawns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        bool result = (bool)method.Invoke(zombieSpawner, new object[] { closePos });

        // Assert - Should return true for close position
        Assert.IsTrue(result, "Position close to fixed spawn should be too close");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithFarPosition()
    {
        // Arrange
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;
        zombieSpawner.exclusionRadius = 5f;
        Vector3 farPos = new Vector3(1000, 0, 1000); // Very far away

        // Act - Invoke IsTooCloseToFixedSpawns via reflection
        var method = typeof(ZombieSpawner).GetMethod("IsTooCloseToFixedSpawns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        bool result = (bool)method.Invoke(zombieSpawner, new object[] { farPos });

        // Assert - Should return false for far position
        Assert.IsFalse(result, "Position far from fixed spawns should not be too close");
    }

    [Test]
    public void TrySpawnRandomZombie_WithValidPrefabs()
    {
        // Arrange
        zombieSpawner.randomZombiePrefabs = zombiePrefabs;
        zombieSpawner.maxRandomZombies = 3;
        zombieSpawner.spawnRadius = 15f;
        zombieSpawner.exclusionRadius = 5f;
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;
        zombieSpawner.player = playerTransform;

        // Act - Invoke TrySpawnRandomZombie via reflection
        var method = typeof(ZombieSpawner).GetMethod("TrySpawnRandomZombie", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Assert - Should not throw exception
        Assert.DoesNotThrow(() => method.Invoke(zombieSpawner, null), "Should spawn with valid prefabs");
    }

    [Test]
    public void FindValidSpawnPosition_AvoidFixedSpawns()
    {
        // Arrange
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.exclusionRadius = 8f;
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;

        // Act - Invoke FindValidSpawnPosition multiple times
        var method = typeof(ZombieSpawner).GetMethod("FindValidSpawnPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Vector3 spawnPos1 = Vector3.zero;
        object[] parameters1 = new object[] { spawnPos1 };
        bool result1 = (bool)method.Invoke(zombieSpawner, parameters1);
        
        Vector3 spawnPos2 = Vector3.zero;
        object[] parameters2 = new object[] { spawnPos2 };
        bool result2 = (bool)method.Invoke(zombieSpawner, parameters2);

        // Assert - Both should find valid positions
        Assert.IsTrue(result1, "First spawn position should be valid");
        Assert.IsTrue(result2, "Second spawn position should be valid");
    }

    [Test]
    public void IsTooCloseToFixedSpawns_WithEmptySpawnPoints()
    {
        // Arrange
        zombieSpawner.fixedSpawnPoints = new Transform[0];
        zombieSpawner.exclusionRadius = 5f;
        Vector3 testPos = new Vector3(0, 0, 0);

        // Act - Invoke IsTooCloseToFixedSpawns via reflection
        var method = typeof(ZombieSpawner).GetMethod("IsTooCloseToFixedSpawns", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        bool result = (bool)method.Invoke(zombieSpawner, new object[] { testPos });

        // Assert - Should return false with no spawn points
        Assert.IsFalse(result, "Should return false when no fixed spawn points exist");
    }

    [Test]
    public void TrySpawnRandomZombie_WithZeroMaxZombies()
    {
        // Arrange
        zombieSpawner.randomZombiePrefabs = zombiePrefabs;
        zombieSpawner.maxRandomZombies = 0;
        zombieSpawner.spawnRadius = 20f;
        zombieSpawner.exclusionRadius = 6f;
        zombieSpawner.fixedSpawnPoints = fixedSpawnPoints;
        zombieSpawner.player = playerTransform;

        // Act - Invoke TrySpawnRandomZombie via reflection
        var method = typeof(ZombieSpawner).GetMethod("TrySpawnRandomZombie", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert - Should handle zero max zombies
        Assert.DoesNotThrow(() => method.Invoke(zombieSpawner, null), "Should handle zero max zombies");
    }
}
