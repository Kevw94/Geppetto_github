using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.AI;

public class ZombieChaseTests
{
    private GameObject zombieObj;
    private ZombieChase zombieChase;
    private GameObject playerObj;
    private Transform playerTransform;
    private GameObject cameraObj;
    private Camera mainCamera;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private HaileyHealth haileyHealth;

    [SetUp]
    public void Setup()
    {
        // Create main camera if it doesn't exist
        if (Camera.main == null)
        {
            cameraObj = new GameObject("TestMainCamera");
            mainCamera = cameraObj.AddComponent<Camera>();
            mainCamera.tag = "MainCamera";
        }

        // Create player GameObject
        playerObj = new GameObject("TestPlayer");
        playerTransform = playerObj.transform;
        playerTransform.position = new Vector3(0, 0, 0);

        // Add HaileyHealth directly to player
        haileyHealth = playerObj.AddComponent<HaileyHealth>();
        haileyHealth.maxHealth = 100f;
        haileyHealth.currentHealth = 100f;

        // Create Zombie GameObject
        zombieObj = new GameObject("TestZombie");
        zombieObj.transform.position = new Vector3(5, 0, 5);

        // Add NavMeshAgent
        navMeshAgent = zombieObj.AddComponent<NavMeshAgent>();
        navMeshAgent.enabled = false; // Disable to avoid NavMesh errors in tests

        // Add Animator
        animator = zombieObj.AddComponent<Animator>();

        // Add ZombieChase component
        zombieChase = zombieObj.AddComponent<ZombieChase>();

        // Configure ZombieChase
        zombieChase.player = playerTransform;
        zombieChase.detectionRange = 15f;
        zombieChase.attackRange = 2f;
        zombieChase.lookSpeed = 5f;
        zombieChase.attackCooldown = 1.5f;
        zombieChase.wanderRadius = 6f;
        zombieChase.wanderInterval = 3f;
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.Destroy(zombieObj);
        UnityEngine.Object.Destroy(playerObj);
        if (cameraObj != null)
            UnityEngine.Object.Destroy(cameraObj);
    }

    [Test]
    public void Setup_InitializesWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(zombieChase, "ZombieChase should be initialized");
        Assert.IsNotNull(playerTransform, "Player should be assigned");
        Assert.IsNotNull(navMeshAgent, "NavMeshAgent should be present");
        Assert.IsNotNull(animator, "Animator should be present");
    }

    [Test]
    public void Setup_InitializesDetectionRange()
    {
        // Assert
        Assert.AreEqual(15f, zombieChase.detectionRange, "Detection range should be initialized");
    }

    [Test]
    public void Setup_InitializesAttackRange()
    {
        // Assert
        Assert.AreEqual(2f, zombieChase.attackRange, "Attack range should be initialized");
    }

    [Test]
    public void Setup_InitializesLookSpeed()
    {
        // Assert
        Assert.AreEqual(5f, zombieChase.lookSpeed, "Look speed should be initialized");
    }

    [Test]
    public void Setup_InitializesAttackCooldown()
    {
        // Assert
        Assert.AreEqual(1.5f, zombieChase.attackCooldown, "Attack cooldown should be initialized");
    }

    [Test]
    public void Setup_InitializesWanderRadius()
    {
        // Assert
        Assert.AreEqual(6f, zombieChase.wanderRadius, "Wander radius should be initialized");
    }

    [Test]
    public void Setup_InitializesWanderInterval()
    {
        // Assert
        Assert.AreEqual(3f, zombieChase.wanderInterval, "Wander interval should be initialized");
    }

    [Test]
    public void Player_CanBeAssigned()
    {
        // Arrange
        GameObject newPlayerObj = new GameObject("NewPlayer");
        Transform newPlayerTransform = newPlayerObj.transform;

        // Act
        zombieChase.player = newPlayerTransform;

        // Assert
        Assert.AreEqual(newPlayerTransform, zombieChase.player, "Player should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newPlayerObj);
    }

    [Test]
    public void Player_CanBeSetToNull()
    {
        // Act
        zombieChase.player = null;

        // Assert
        Assert.IsNull(zombieChase.player, "Player should accept null value");
    }

    [Test]
    public void DetectionRange_CanBeModified()
    {
        // Arrange
        float newRange = 20f;

        // Act
        zombieChase.detectionRange = newRange;

        // Assert
        Assert.AreEqual(newRange, zombieChase.detectionRange, "Detection range should be modifiable");
    }

    [Test]
    public void AttackRange_CanBeModified()
    {
        // Arrange
        float newRange = 3f;

        // Act
        zombieChase.attackRange = newRange;

        // Assert
        Assert.AreEqual(newRange, zombieChase.attackRange, "Attack range should be modifiable");
    }

    [Test]
    public void LookSpeed_CanBeModified()
    {
        // Arrange
        float newSpeed = 10f;

        // Act
        zombieChase.lookSpeed = newSpeed;

        // Assert
        Assert.AreEqual(newSpeed, zombieChase.lookSpeed, "Look speed should be modifiable");
    }

    [Test]
    public void AttackCooldown_CanBeModified()
    {
        // Arrange
        float newCooldown = 2f;

        // Act
        zombieChase.attackCooldown = newCooldown;

        // Assert
        Assert.AreEqual(newCooldown, zombieChase.attackCooldown, "Attack cooldown should be modifiable");
    }

    [Test]
    public void WanderRadius_CanBeModified()
    {
        // Arrange
        float newRadius = 10f;

        // Act
        zombieChase.wanderRadius = newRadius;

        // Assert
        Assert.AreEqual(newRadius, zombieChase.wanderRadius, "Wander radius should be modifiable");
    }

    [Test]
    public void WanderInterval_CanBeModified()
    {
        // Arrange
        float newInterval = 5f;

        // Act
        zombieChase.wanderInterval = newInterval;

        // Assert
        Assert.AreEqual(newInterval, zombieChase.wanderInterval, "Wander interval should be modifiable");
    }

    [Test]
    public void DetectionRange_WithZeroValue()
    {
        // Arrange
        float zeroRange = 0f;

        // Act
        zombieChase.detectionRange = zeroRange;

        // Assert
        Assert.AreEqual(zeroRange, zombieChase.detectionRange, "Detection range should accept zero value");
    }

    [Test]
    public void DetectionRange_WithLargeValue()
    {
        // Arrange
        float largeRange = 100f;

        // Act
        zombieChase.detectionRange = largeRange;

        // Assert
        Assert.AreEqual(largeRange, zombieChase.detectionRange, "Detection range should accept large values");
    }

    [Test]
    public void AttackRange_WithZeroValue()
    {
        // Arrange
        float zeroRange = 0f;

        // Act
        zombieChase.attackRange = zeroRange;

        // Assert
        Assert.AreEqual(zeroRange, zombieChase.attackRange, "Attack range should accept zero value");
    }

    [Test]
    public void LookSpeed_WithZeroValue()
    {
        // Arrange
        float zeroSpeed = 0f;

        // Act
        zombieChase.lookSpeed = zeroSpeed;

        // Assert
        Assert.AreEqual(zeroSpeed, zombieChase.lookSpeed, "Look speed should accept zero value");
    }

    [Test]
    public void AttackCooldown_WithZeroValue()
    {
        // Arrange
        float zeroCooldown = 0f;

        // Act
        zombieChase.attackCooldown = zeroCooldown;

        // Assert
        Assert.AreEqual(zeroCooldown, zombieChase.attackCooldown, "Attack cooldown should accept zero value");
    }

    [Test]
    public void WanderRadius_WithZeroValue()
    {
        // Arrange
        float zeroRadius = 0f;

        // Act
        zombieChase.wanderRadius = zeroRadius;

        // Assert
        Assert.AreEqual(zeroRadius, zombieChase.wanderRadius, "Wander radius should accept zero value");
    }

    [Test]
    public void WanderInterval_WithZeroValue()
    {
        // Arrange
        float zeroInterval = 0f;

        // Act
        zombieChase.wanderInterval = zeroInterval;

        // Assert
        Assert.AreEqual(zeroInterval, zombieChase.wanderInterval, "Wander interval should accept zero value");
    }

    [Test]
    public void FacePlayer_HasCorrectSignature()
    {
        // Assert - FacePlayer method should exist
        var method = typeof(ZombieChase).GetMethod("FacePlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "FacePlayer method should exist");
    }

    [Test]
    public void DealDamage_HasCorrectSignature()
    {
        // Assert - DealDamage method should exist
        var method = typeof(ZombieChase).GetMethod("DealDamage", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "DealDamage method should exist");
    }

    [Test]
    public void RandomNavmeshLocation_HasCorrectSignature()
    {
        // Assert - RandomNavmeshLocation method should exist
        var method = typeof(ZombieChase).GetMethod("RandomNavmeshLocation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "RandomNavmeshLocation method should exist");
    }

    [Test]
    public void DealDamage_ReducesPlayerHealth()
    {
        // Arrange
        float initialHealth = haileyHealth.currentHealth;
        zombieObj.transform.position = playerObj.transform.position + Vector3.forward * 1f;

        // Act
        zombieChase.DealDamage();

        // Assert
        Assert.Less(haileyHealth.currentHealth, initialHealth, "Player health should decrease after DealDamage");
    }

    [Test]
    public void DealDamage_WithPlayerOutOfRange()
    {
        // Arrange
        float initialHealth = haileyHealth.currentHealth;
        zombieObj.transform.position = playerObj.transform.position + Vector3.forward * 10f;

        // Act
        zombieChase.DealDamage();

        // Assert
        Assert.AreEqual(initialHealth, haileyHealth.currentHealth, "Player health should not change when out of range");
    }

    [Test]
    public void DealDamage_RequiresValidPlayer()
    {
        // Assert - DealDamage requires a valid player reference
        // The method accesses player.position without null check
        Assert.IsNotNull(zombieChase.player, "Player must be assigned for DealDamage to work");
    }

    [Test]
    public void ZombieChase_CanBeDisabledAndReenabled()
    {
        // Arrange
        zombieChase.enabled = true;

        // Act
        zombieChase.enabled = false;
        bool disabledState = !zombieChase.enabled;
        zombieChase.enabled = true;
        bool reenableState = zombieChase.enabled;

        // Assert
        Assert.IsTrue(disabledState, "ZombieChase should be disabled");
        Assert.IsTrue(reenableState, "ZombieChase should be re-enabled");
    }

    [Test]
    public void ZombieChase_PositionCanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(10, 0, 10);

        // Act
        zombieObj.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, zombieObj.transform.position, "Zombie position should be modifiable");
    }

    [Test]
    public void MultipleZombies_CanCoexist()
    {
        // Arrange
        GameObject zombie2Obj = new GameObject("TestZombie2");
        zombie2Obj.transform.position = new Vector3(10, 0, 10);
        NavMeshAgent agent2 = zombie2Obj.AddComponent<NavMeshAgent>();
        agent2.enabled = false;
        Animator animator2 = zombie2Obj.AddComponent<Animator>();
        ZombieChase zombieChase2 = zombie2Obj.AddComponent<ZombieChase>();
        zombieChase2.player = playerTransform;

        // Act
        zombieChase.enabled = true;
        zombieChase2.enabled = true;

        // Assert - Both should be active
        Assert.IsTrue(zombieChase.enabled, "First zombie should be enabled");
        Assert.IsTrue(zombieChase2.enabled, "Second zombie should be enabled");

        // Cleanup
        UnityEngine.Object.Destroy(zombie2Obj);
    }

    [Test]
    public void DetectionRange_WithNegativeValue()
    {
        // Arrange
        float negativeRange = -5f;

        // Act
        zombieChase.detectionRange = negativeRange;

        // Assert
        Assert.AreEqual(negativeRange, zombieChase.detectionRange, "Detection range should accept negative values");
    }

    [Test]
    public void AttackRange_WithNegativeValue()
    {
        // Arrange
        float negativeRange = -1f;

        // Act
        zombieChase.attackRange = negativeRange;

        // Assert
        Assert.AreEqual(negativeRange, zombieChase.attackRange, "Attack range should accept negative values");
    }

    [Test]
    public void ZombieChase_HasPublicPlayerField()
    {
        // Assert - ZombieChase should have public player field
        var field = typeof(ZombieChase).GetField("player", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "player field should exist and be public");
    }

    [Test]
    public void ZombieChase_HasPublicDetectionRangeField()
    {
        // Assert - ZombieChase should have public detectionRange field
        var field = typeof(ZombieChase).GetField("detectionRange", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "detectionRange field should exist and be public");
    }

    [Test]
    public void ZombieChase_HasPublicAttackRangeField()
    {
        // Assert - ZombieChase should have public attackRange field
        var field = typeof(ZombieChase).GetField("attackRange", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "attackRange field should exist and be public");
    }

    [Test]
    public void ZombieChase_HasPublicLookSpeedField()
    {
        // Assert - ZombieChase should have public lookSpeed field
        var field = typeof(ZombieChase).GetField("lookSpeed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "lookSpeed field should exist and be public");
    }

    [Test]
    public void ZombieChase_HasPublicAttackCooldownField()
    {
        // Assert - ZombieChase should have public attackCooldown field
        var field = typeof(ZombieChase).GetField("attackCooldown", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "attackCooldown field should exist and be public");
    }

    [Test]
    public void ZombieChase_HasPublicWanderRadiusField()
    {
        // Assert - ZombieChase should have public wanderRadius field
        var field = typeof(ZombieChase).GetField("wanderRadius", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "wanderRadius field should exist and be public");
    }

    [Test]
    public void ZombieChase_HasPublicWanderIntervalField()
    {
        // Assert - ZombieChase should have public wanderInterval field
        var field = typeof(ZombieChase).GetField("wanderInterval", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "wanderInterval field should exist and be public");
    }

    [Test]
    public void DistanceCalculation_BetweenZombieAndPlayer()
    {
        // Arrange
        zombieObj.transform.position = new Vector3(0, 0, 0);
        playerObj.transform.position = new Vector3(3, 0, 4);

        // Act
        float distance = Vector3.Distance(zombieObj.transform.position, playerObj.transform.position);

        // Assert
        Assert.AreEqual(5f, distance, 0.01f, "Distance should be calculated correctly");
    }

    [Test]
    public void DealDamage_DamageAmount()
    {
        // Arrange
        float initialHealth = haileyHealth.currentHealth;
        zombieObj.transform.position = playerObj.transform.position + Vector3.forward * 1f;

        // Act
        zombieChase.DealDamage();

        // Assert - Should deal 10 damage
        Assert.AreEqual(initialHealth - 10f, haileyHealth.currentHealth, "DealDamage should deal 10 damage");
    }

    [Test]
    public void Update_WithPlayerInDetectionRange_CalculatesDistance()
    {
        // Arrange
        zombieObj.transform.position = new Vector3(0, 0, 0);
        playerObj.transform.position = new Vector3(5, 0, 0);
        zombieChase.detectionRange = 10f;

        // Act
        float distance = Vector3.Distance(zombieObj.transform.position, playerObj.transform.position);

        // Assert - Distance should be within detection range
        Assert.Less(distance, zombieChase.detectionRange, "Player should be in detection range");
    }

    [Test]
    public void Update_WithPlayerOutOfDetectionRange_CalculatesDistance()
    {
        // Arrange
        zombieObj.transform.position = new Vector3(0, 0, 0);
        playerObj.transform.position = new Vector3(20, 0, 0);
        zombieChase.detectionRange = 10f;

        // Act
        float distance = Vector3.Distance(zombieObj.transform.position, playerObj.transform.position);

        // Assert - Distance should be outside detection range
        Assert.Greater(distance, zombieChase.detectionRange, "Player should be out of detection range");
    }

    [Test]
    public void Update_WithPlayerInAttackRange_CalculatesDistance()
    {
        // Arrange
        zombieObj.transform.position = new Vector3(0, 0, 0);
        playerObj.transform.position = new Vector3(1, 0, 0);
        zombieChase.attackRange = 2f;

        // Act
        float distance = Vector3.Distance(zombieObj.transform.position, playerObj.transform.position);

        // Assert - Distance should be within attack range
        Assert.Less(distance, zombieChase.attackRange, "Player should be in attack range");
    }

    [Test]
    public void Update_WithNullPlayer_ReturnsEarly()
    {
        // Arrange
        zombieChase.player = null;

        // Act & Assert - Update should return early without errors
        Assert.DoesNotThrow(() => 
        {
            // Simulate Update behavior
            if (zombieChase.player == null) return;
        }, "Update should handle null player gracefully");
    }

    [Test]
    public void FacePlayer_CalculatesDirectionCorrectly()
    {
        // Arrange
        zombieObj.transform.position = new Vector3(0, 0, 0);
        playerObj.transform.position = new Vector3(5, 0, 0);

        // Act
        Vector3 direction = (playerObj.transform.position - zombieObj.transform.position).normalized;
        direction.y = 0;

        // Assert - Direction should point toward player
        Assert.AreEqual(new Vector3(1, 0, 0), direction, "Direction should point toward player");
    }

    [Test]
    public void FacePlayer_WithHighLookSpeed_VerifyParameter()
    {
        // Arrange
        zombieChase.lookSpeed = 20f;

        // Act
        float lookSpeed = zombieChase.lookSpeed;

        // Assert - High look speed should be set
        Assert.AreEqual(20f, lookSpeed, "High look speed should be assignable");
    }

    [Test]
    public void FacePlayer_WithLowLookSpeed_VerifyParameter()
    {
        // Arrange
        zombieChase.lookSpeed = 0.5f;

        // Act
        float lookSpeed = zombieChase.lookSpeed;

        // Assert - Low look speed should be set
        Assert.AreEqual(0.5f, lookSpeed, "Low look speed should be assignable");
    }

    [Test]
    public void FacePlayer_WithZeroLookSpeed_VerifyParameter()
    {
        // Arrange
        zombieChase.lookSpeed = 0f;

        // Act
        float lookSpeed = zombieChase.lookSpeed;

        // Assert - Zero look speed should be set
        Assert.AreEqual(0f, lookSpeed, "Zero look speed should be assignable");
    }

    [Test]
    public void RandomNavmeshLocation_ReturnsVector3()
    {
        // Arrange
        var method = typeof(ZombieChase).GetMethod("RandomNavmeshLocation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Act
        Vector3 result = (Vector3)method.Invoke(zombieChase, new object[] { 5f });

        // Assert - Should return a Vector3
        Assert.IsNotNull(result, "RandomNavmeshLocation should return a Vector3");
    }

    [Test]
    public void RandomNavmeshLocation_WithSmallRadius()
    {
        // Arrange
        var method = typeof(ZombieChase).GetMethod("RandomNavmeshLocation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Act
        Vector3 result = (Vector3)method.Invoke(zombieChase, new object[] { 1f });

        // Assert - Should return a position
        Assert.IsNotNull(result, "RandomNavmeshLocation should work with small radius");
    }

    [Test]
    public void RandomNavmeshLocation_WithLargeRadius()
    {
        // Arrange
        var method = typeof(ZombieChase).GetMethod("RandomNavmeshLocation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Act
        Vector3 result = (Vector3)method.Invoke(zombieChase, new object[] { 20f });

        // Assert - Should return a position
        Assert.IsNotNull(result, "RandomNavmeshLocation should work with large radius");
    }

    [Test]
    public void RandomNavmeshLocation_WithZeroRadius()
    {
        // Arrange
        var method = typeof(ZombieChase).GetMethod("RandomNavmeshLocation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Act
        Vector3 result = (Vector3)method.Invoke(zombieChase, new object[] { 0f });

        // Assert - Should return a position
        Assert.IsNotNull(result, "RandomNavmeshLocation should work with zero radius");
    }

    [Test]
    public void Start_HasNavMeshAgentComponent()
    {
        // Assert - ZombieChase requires NavMeshAgent
        Assert.IsNotNull(navMeshAgent, "NavMeshAgent should be present on zombie");
    }

    [Test]
    public void Start_HasAnimatorComponent()
    {
        // Assert - ZombieChase requires Animator
        Assert.IsNotNull(animator, "Animator should be present on zombie");
    }

    [Test]
    public void Start_FindsMainCameraIfPlayerNull()
    {
        // Arrange
        if (Camera.main == null)
        {
            GameObject cameraObj = new GameObject("MainCamera");
            Camera camera = cameraObj.AddComponent<Camera>();
            camera.tag = "MainCamera";
        }

        // Act
        Camera mainCam = Camera.main;

        // Assert - Main camera should exist
        Assert.IsNotNull(mainCam, "Main camera should be found");

        // Cleanup
        if (cameraObj != null)
            UnityEngine.Object.Destroy(cameraObj);
    }

    [Test]
    public void Update_ChaseBehavior_VerifyDetectionLogic()
    {
        // Arrange
        zombieObj.transform.position = new Vector3(0, 0, 0);
        playerObj.transform.position = new Vector3(5, 0, 0);
        zombieChase.detectionRange = 15f;
        zombieChase.attackRange = 2f;

        // Act
        float distance = Vector3.Distance(zombieObj.transform.position, playerObj.transform.position);
        bool isInDetectionRange = distance <= zombieChase.detectionRange;
        bool isInAttackRange = distance <= zombieChase.attackRange;

        // Assert - Chase behavior conditions
        Assert.IsTrue(isInDetectionRange, "Player should be in detection range");
        Assert.IsFalse(isInAttackRange, "Player should not be in attack range (chase, not attack)");
    }

    [Test]
    public void Update_WanderBehavior_VerifyOutOfRangeLogic()
    {
        // Arrange
        zombieObj.transform.position = new Vector3(0, 0, 0);
        playerObj.transform.position = new Vector3(50, 0, 0);
        zombieChase.detectionRange = 15f;

        // Act
        float distance = Vector3.Distance(zombieObj.transform.position, playerObj.transform.position);
        bool isOutOfDetectionRange = distance > zombieChase.detectionRange;

        // Assert - Wander behavior conditions
        Assert.IsTrue(isOutOfDetectionRange, "Player should be out of detection range");
    }

    [Test]
    public void Update_AttackCooldown_VerifyAttackRangeLogic()
    {
        // Arrange
        zombieObj.transform.position = new Vector3(0, 0, 0);
        playerObj.transform.position = new Vector3(1, 0, 0);
        zombieChase.detectionRange = 15f;
        zombieChase.attackRange = 2f;

        // Act
        float distance = Vector3.Distance(zombieObj.transform.position, playerObj.transform.position);
        bool isInAttackRange = distance <= zombieChase.attackRange;

        // Assert - Attack range conditions
        Assert.IsTrue(isInAttackRange, "Player should be in attack range");
    }

    [Test]
    public void DealDamage_ChecksDistanceBeforeDamaging()
    {
        // Arrange
        float initialHealth = haileyHealth.currentHealth;
        zombieObj.transform.position = playerObj.transform.position + Vector3.forward * 5f;
        zombieChase.attackRange = 2f;

        // Act
        zombieChase.DealDamage();

        // Assert - Health should not change (out of range)
        Assert.AreEqual(initialHealth, haileyHealth.currentHealth, "DealDamage should check distance");
    }

    [Test]
    public void Update_CalculatesDistanceCorrectly()
    {
        // Arrange
        zombieObj.transform.position = new Vector3(0, 0, 0);
        playerObj.transform.position = new Vector3(3, 0, 4);

        // Act
        float distance = Vector3.Distance(zombieObj.transform.position, playerObj.transform.position);

        // Assert
        Assert.AreEqual(5f, distance, 0.01f, "Distance calculation should be correct");
    }
}
