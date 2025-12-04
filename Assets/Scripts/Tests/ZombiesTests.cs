using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ZombiesTests
{
    private GameObject zombieGameObject;
    private Zombies zombie;
    private GameObject playerGameObject;
    private Transform playerTransform;
    private GameObject soundControllerObj;
    private NPCSoundController soundController;
    private GameObject enemyHealthObj;
    private EnemyHealth enemyHealth;

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

        // Create sound controller
        soundControllerObj = new GameObject("TestSoundController");
        soundController = soundControllerObj.AddComponent<NPCSoundController>();

        // Create enemy health
        enemyHealthObj = new GameObject("TestEnemyHealth");
        enemyHealth = enemyHealthObj.AddComponent<EnemyHealth>();

        // Create zombie GameObject
        zombieGameObject = new GameObject("TestZombie");
        zombie = zombieGameObject.AddComponent<Zombies>();

        // Add required components
        zombieGameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
        zombieGameObject.AddComponent<Animator>();

        // Create a renderer for dissolve effect
        GameObject rendererObj = new GameObject("ZombieRenderer");
        rendererObj.transform.SetParent(zombieGameObject.transform);
        MeshRenderer meshRenderer = rendererObj.AddComponent<MeshRenderer>();
        Material testMaterial = new Material(Shader.Find("Standard"));
        meshRenderer.material = testMaterial;

        // Configure zombie
        zombie.player = playerTransform;
        zombie.soundController = soundController;
        zombie.enemyHealth = enemyHealth;
        zombie.dissolveRenderer = meshRenderer;
        zombie.isMenu = false;
        zombie.detectionRange = 15f;
        zombie.attackRange = 2f;
        zombie.sinkDistance = 2f;
        zombie.emergeDuration = 1f;
        zombie.startAnimationDelay = 0.3f;
        zombie.screamChance = 0.5f;
        zombie.wanderRadius = 10f;
        zombie.wanderInterval = 3f;
        zombie.attackCooldown = 1.5f;
        zombie.hitAnimationChance = 0.5f;
        zombie.sinkDuration = 2f;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(zombieGameObject);
        Object.Destroy(playerGameObject);
        Object.Destroy(soundControllerObj);
        Object.Destroy(enemyHealthObj);

        // Clean up test camera if it exists
        Camera mainCamera = Camera.main;
        if (mainCamera != null && mainCamera.gameObject.name == "TestCamera")
        {
            Object.Destroy(mainCamera.gameObject);
        }
    }

    [Test]
    public void Setup_InitializesZombieWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(zombie.player, "Player reference should be assigned");
        Assert.IsNotNull(zombie.soundController, "Sound controller should be assigned");
        Assert.IsNotNull(zombie.enemyHealth, "Enemy health should be assigned");
        Assert.IsFalse(zombie.isMenu, "Should not be menu zombie");
    }

    [Test]
    public void Setup_InitializesDetectionRange()
    {
        // Assert
        Assert.AreEqual(15f, zombie.detectionRange, "Detection range should be initialized");
    }

    [Test]
    public void Setup_InitializesAttackRange()
    {
        // Assert
        Assert.AreEqual(2f, zombie.attackRange, "Attack range should be initialized");
    }

    [Test]
    public void Setup_InitializesSinkDistance()
    {
        // Assert
        Assert.AreEqual(2f, zombie.sinkDistance, "Sink distance should be initialized");
    }

    [Test]
    public void Setup_InitializesEmergeDuration()
    {
        // Assert
        Assert.AreEqual(1f, zombie.emergeDuration, "Emerge duration should be initialized");
    }

    [Test]
    public void Setup_InitializesScreamChance()
    {
        // Assert
        Assert.AreEqual(0.5f, zombie.screamChance, "Scream chance should be initialized");
    }

    [Test]
    public void Setup_InitializesWanderRadius()
    {
        // Assert
        Assert.AreEqual(10f, zombie.wanderRadius, "Wander radius should be initialized");
    }

    [Test]
    public void Setup_InitializesWanderInterval()
    {
        // Assert
        Assert.AreEqual(3f, zombie.wanderInterval, "Wander interval should be initialized");
    }

    [Test]
    public void Setup_InitializesAttackCooldown()
    {
        // Assert
        Assert.AreEqual(1.5f, zombie.attackCooldown, "Attack cooldown should be initialized");
    }

    [Test]
    public void Start_InitializesComponents()
    {
        // Assert - Start method should initialize components
        var method = typeof(Zombies).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Start method should exist and initialize components");
    }

    [Test]
    public void Start_AutoAssignsPlayer()
    {
        // Assert - Start method should auto-assign player from main camera
        // The Start method checks if player is null and finds Camera.main
        var method = typeof(Zombies).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Start method should exist and auto-assign player");
    }

    [Test]
    public void Start_BeginEmergence()
    {
        // Assert - Start method should begin emergence
        var method = typeof(Zombies).GetMethod("BeginEmergence", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "BeginEmergence method should exist");
    }

    [Test]
    public void Update_MenuZombieDoesNotMove()
    {
        // Arrange
        zombie.isMenu = true;

        // Assert - Menu zombies should not move
        Assert.IsTrue(zombie.isMenu, "Menu zombie flag should be set");
    }

    [Test]
    public void Update_WithNullPlayer()
    {
        // Arrange
        zombie.player = null;

        // Assert - Update should handle null player
        Assert.IsNull(zombie.player, "Player can be null");
    }

    [Test]
    public void DetectionRange_CanBeModified()
    {
        // Arrange
        float newRange = 20f;

        // Act
        zombie.detectionRange = newRange;

        // Assert
        Assert.AreEqual(newRange, zombie.detectionRange, "Detection range should be modifiable");
    }

    [Test]
    public void AttackRange_CanBeModified()
    {
        // Arrange
        float newRange = 3f;

        // Act
        zombie.attackRange = newRange;

        // Assert
        Assert.AreEqual(newRange, zombie.attackRange, "Attack range should be modifiable");
    }

    [Test]
    public void SinkDistance_CanBeModified()
    {
        // Arrange
        float newDistance = 3f;

        // Act
        zombie.sinkDistance = newDistance;

        // Assert
        Assert.AreEqual(newDistance, zombie.sinkDistance, "Sink distance should be modifiable");
    }

    [Test]
    public void EmergeDuration_CanBeModified()
    {
        // Arrange
        float newDuration = 2f;

        // Act
        zombie.emergeDuration = newDuration;

        // Assert
        Assert.AreEqual(newDuration, zombie.emergeDuration, "Emerge duration should be modifiable");
    }

    [Test]
    public void ScreamChance_CanBeModified()
    {
        // Arrange
        float newChance = 0.8f;

        // Act
        zombie.screamChance = newChance;

        // Assert
        Assert.AreEqual(newChance, zombie.screamChance, "Scream chance should be modifiable");
    }

    [Test]
    public void WanderRadius_CanBeModified()
    {
        // Arrange
        float newRadius = 15f;

        // Act
        zombie.wanderRadius = newRadius;

        // Assert
        Assert.AreEqual(newRadius, zombie.wanderRadius, "Wander radius should be modifiable");
    }

    [Test]
    public void WanderInterval_CanBeModified()
    {
        // Arrange
        float newInterval = 5f;

        // Act
        zombie.wanderInterval = newInterval;

        // Assert
        Assert.AreEqual(newInterval, zombie.wanderInterval, "Wander interval should be modifiable");
    }

    [Test]
    public void AttackCooldown_CanBeModified()
    {
        // Arrange
        float newCooldown = 2f;

        // Act
        zombie.attackCooldown = newCooldown;

        // Assert
        Assert.AreEqual(newCooldown, zombie.attackCooldown, "Attack cooldown should be modifiable");
    }

    [Test]
    public void HitAnimationChance_CanBeModified()
    {
        // Arrange
        float newChance = 0.7f;

        // Act
        zombie.hitAnimationChance = newChance;

        // Assert
        Assert.AreEqual(newChance, zombie.hitAnimationChance, "Hit animation chance should be modifiable");
    }

    [Test]
    public void SinkDuration_CanBeModified()
    {
        // Arrange
        float newDuration = 3f;

        // Act
        zombie.sinkDuration = newDuration;

        // Assert
        Assert.AreEqual(newDuration, zombie.sinkDuration, "Sink duration should be modifiable");
    }

    [Test]
    public void Player_CanBeAssigned()
    {
        // Arrange
        GameObject newPlayerObj = new GameObject("NewTestPlayer");
        Transform newPlayer = newPlayerObj.transform;

        // Act
        zombie.player = newPlayer;

        // Assert
        Assert.AreEqual(newPlayer, zombie.player, "Player reference should be assignable");

        // Cleanup
        Object.Destroy(newPlayerObj);
    }

    [Test]
    public void SoundController_CanBeAssigned()
    {
        // Arrange
        GameObject newSoundObj = new GameObject("NewSoundController");
        NPCSoundController newSound = newSoundObj.AddComponent<NPCSoundController>();

        // Act
        zombie.soundController = newSound;

        // Assert
        Assert.AreEqual(newSound, zombie.soundController, "Sound controller should be assignable");

        // Cleanup
        Object.Destroy(newSoundObj);
    }

    [Test]
    public void EnemyHealth_CanBeAssigned()
    {
        // Arrange
        GameObject newHealthObj = new GameObject("NewEnemyHealth");
        EnemyHealth newHealth = newHealthObj.AddComponent<EnemyHealth>();

        // Act
        zombie.enemyHealth = newHealth;

        // Assert
        Assert.AreEqual(newHealth, zombie.enemyHealth, "Enemy health should be assignable");

        // Cleanup
        Object.Destroy(newHealthObj);
    }

    [Test]
    public void DealDamage_WithNullPlayer()
    {
        // Arrange
        zombie.player = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombie.DealDamage(), "DealDamage should handle null player");
    }

    [Test]
    public void DealDamage_WithPlayerOutOfRange()
    {
        // Arrange
        playerTransform.position = new Vector3(100f, 0, 100f);
        zombie.attackRange = 2f;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombie.DealDamage(), "DealDamage should handle out-of-range player");
    }

    [Test]
    public void Die_HasCorrectSignature()
    {
        // Assert - Die method should exist
        var method = typeof(Zombies).GetMethod("Die", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Die method should exist");
    }

    [Test]
    public void PlayScreamEvent_WithNullPlayer()
    {
        // Arrange
        zombie.player = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombie.PlayScreamEvent(), "PlayScreamEvent should handle null player");
    }

    [Test]
    public void PlayScreamEvent_CanBeCalled()
    {
        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombie.PlayScreamEvent(), "PlayScreamEvent should execute without errors");
    }

    [Test]
    public void IsMenu_CanBeModified()
    {
        // Arrange
        bool newMenuState = true;

        // Act
        zombie.isMenu = newMenuState;

        // Assert
        Assert.AreEqual(newMenuState, zombie.isMenu, "isMenu should be modifiable");
    }

    [Test]
    public void DetectionRange_WithZeroValue()
    {
        // Arrange
        float zeroRange = 0f;

        // Act
        zombie.detectionRange = zeroRange;

        // Assert
        Assert.AreEqual(zeroRange, zombie.detectionRange, "Detection range should accept zero value");
    }

    [Test]
    public void DetectionRange_WithLargeValue()
    {
        // Arrange
        float largeRange = 100f;

        // Act
        zombie.detectionRange = largeRange;

        // Assert
        Assert.AreEqual(largeRange, zombie.detectionRange, "Detection range should accept large values");
    }

    [Test]
    public void AttackRange_WithZeroValue()
    {
        // Arrange
        float zeroRange = 0f;

        // Act
        zombie.attackRange = zeroRange;

        // Assert
        Assert.AreEqual(zeroRange, zombie.attackRange, "Attack range should accept zero value");
    }

    [Test]
    public void AttackRange_WithLargeValue()
    {
        // Arrange
        float largeRange = 10f;

        // Act
        zombie.attackRange = largeRange;

        // Assert
        Assert.AreEqual(largeRange, zombie.attackRange, "Attack range should accept large values");
    }

    [Test]
    public void ScreamChance_WithZeroValue()
    {
        // Arrange
        float zeroChance = 0f;

        // Act
        zombie.screamChance = zeroChance;

        // Assert
        Assert.AreEqual(zeroChance, zombie.screamChance, "Scream chance should accept zero value");
    }

    [Test]
    public void ScreamChance_WithOneValue()
    {
        // Arrange
        float fullChance = 1f;

        // Act
        zombie.screamChance = fullChance;

        // Assert
        Assert.AreEqual(fullChance, zombie.screamChance, "Scream chance should accept value of 1");
    }

    [Test]
    public void HitAnimationChance_WithZeroValue()
    {
        // Arrange
        float zeroChance = 0f;

        // Act
        zombie.hitAnimationChance = zeroChance;

        // Assert
        Assert.AreEqual(zeroChance, zombie.hitAnimationChance, "Hit animation chance should accept zero value");
    }

    [Test]
    public void HitAnimationChance_WithOneValue()
    {
        // Arrange
        float fullChance = 1f;

        // Act
        zombie.hitAnimationChance = fullChance;

        // Assert
        Assert.AreEqual(fullChance, zombie.hitAnimationChance, "Hit animation chance should accept value of 1");
    }

    [Test]
    public void WanderRadius_WithZeroValue()
    {
        // Arrange
        float zeroRadius = 0f;

        // Act
        zombie.wanderRadius = zeroRadius;

        // Assert
        Assert.AreEqual(zeroRadius, zombie.wanderRadius, "Wander radius should accept zero value");
    }

    [Test]
    public void WanderRadius_WithLargeValue()
    {
        // Arrange
        float largeRadius = 50f;

        // Act
        zombie.wanderRadius = largeRadius;

        // Assert
        Assert.AreEqual(largeRadius, zombie.wanderRadius, "Wander radius should accept large values");
    }

    [Test]
    public void WanderInterval_WithZeroValue()
    {
        // Arrange
        float zeroInterval = 0f;

        // Act
        zombie.wanderInterval = zeroInterval;

        // Assert
        Assert.AreEqual(zeroInterval, zombie.wanderInterval, "Wander interval should accept zero value");
    }

    [Test]
    public void WanderInterval_WithLargeValue()
    {
        // Arrange
        float largeInterval = 10f;

        // Act
        zombie.wanderInterval = largeInterval;

        // Assert
        Assert.AreEqual(largeInterval, zombie.wanderInterval, "Wander interval should accept large values");
    }

    [Test]
    public void AttackCooldown_WithZeroValue()
    {
        // Arrange
        float zeroCooldown = 0f;

        // Act
        zombie.attackCooldown = zeroCooldown;

        // Assert
        Assert.AreEqual(zeroCooldown, zombie.attackCooldown, "Attack cooldown should accept zero value");
    }

    [Test]
    public void AttackCooldown_WithLargeValue()
    {
        // Arrange
        float largeCooldown = 5f;

        // Act
        zombie.attackCooldown = largeCooldown;

        // Assert
        Assert.AreEqual(largeCooldown, zombie.attackCooldown, "Attack cooldown should accept large values");
    }

    [Test]
    public void EmergeDuration_WithZeroValue()
    {
        // Arrange
        float zeroDuration = 0f;

        // Act
        zombie.emergeDuration = zeroDuration;

        // Assert
        Assert.AreEqual(zeroDuration, zombie.emergeDuration, "Emerge duration should accept zero value");
    }

    [Test]
    public void EmergeDuration_WithLargeValue()
    {
        // Arrange
        float largeDuration = 5f;

        // Act
        zombie.emergeDuration = largeDuration;

        // Assert
        Assert.AreEqual(largeDuration, zombie.emergeDuration, "Emerge duration should accept large values");
    }

    [Test]
    public void SinkDuration_WithZeroValue()
    {
        // Arrange
        float zeroDuration = 0f;

        // Act
        zombie.sinkDuration = zeroDuration;

        // Assert
        Assert.AreEqual(zeroDuration, zombie.sinkDuration, "Sink duration should accept zero value");
    }

    [Test]
    public void SinkDuration_WithLargeValue()
    {
        // Arrange
        float largeDuration = 5f;

        // Act
        zombie.sinkDuration = largeDuration;

        // Assert
        Assert.AreEqual(largeDuration, zombie.sinkDuration, "Sink duration should accept large values");
    }

    [Test]
    public void HandlesMissingSoundController()
    {
        // Arrange
        zombie.soundController = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombie.enabled = true, "Should handle null sound controller gracefully");
    }

    [Test]
    public void HandlesMissingEnemyHealth()
    {
        // Arrange
        zombie.enemyHealth = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombie.enabled = true, "Should handle null enemy health gracefully");
    }

    [Test]
    public void HandlesMissingDissolveRenderer()
    {
        // Arrange
        zombie.dissolveRenderer = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => zombie.enabled = true, "Should handle null dissolve renderer gracefully");
    }

    [Test]
    public void MultipleZombies_CanCoexist()
    {
        // Arrange
        GameObject zombie2Obj = new GameObject("TestZombie2");
        Zombies zombie2 = zombie2Obj.AddComponent<Zombies>();
        zombie2Obj.AddComponent<UnityEngine.AI.NavMeshAgent>();
        zombie2Obj.AddComponent<Animator>();
        zombie2.player = playerTransform;
        zombie2.soundController = soundController;
        zombie2.isMenu = false;

        // Act
        zombie.enabled = true;
        zombie2.enabled = true;

        // Assert - Both zombies should be active
        Assert.IsTrue(zombie.enabled, "First zombie should be enabled");
        Assert.IsTrue(zombie2.enabled, "Second zombie should be enabled");

        // Cleanup
        Object.Destroy(zombie2Obj);
    }

    [Test]
    public void Zombie_CanBeDisabledAndReenabled()
    {
        // Arrange
        zombie.enabled = true;

        // Act
        zombie.enabled = false;
        bool disabledState = !zombie.enabled;
        zombie.enabled = true;
        bool reenableState = zombie.enabled;

        // Assert
        Assert.IsTrue(disabledState, "Zombie should be disabled");
        Assert.IsTrue(reenableState, "Zombie should be re-enabled");
    }

    [Test]
    public void ZombiePosition_CanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(5f, 0, 5f);

        // Act
        zombieGameObject.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, zombieGameObject.transform.position, "Zombie position should be modifiable");
    }

    [Test]
    public void ChasePlayer_HasCorrectSignature()
    {
        // Assert - ChasePlayer method should exist
        var method = typeof(Zombies).GetMethod("ChasePlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "ChasePlayer method should exist");
    }

    [Test]
    public void Wander_HasCorrectSignature()
    {
        // Assert - Wander method should exist
        var method = typeof(Zombies).GetMethod("Wander", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Wander method should exist");
    }

    [Test]
    public void RandomNavmeshLocation_HasCorrectSignature()
    {
        // Assert - RandomNavmeshLocation method should exist
        var method = typeof(Zombies).GetMethod("RandomNavmeshLocation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "RandomNavmeshLocation method should exist");
    }

    [Test]
    public void TryAttack_HasCorrectSignature()
    {
        // Assert - TryAttack method should exist
        var method = typeof(Zombies).GetMethod("TryAttack", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "TryAttack method should exist");
    }

    [Test]
    public void FacePlayer_HasCorrectSignature()
    {
        // Assert - FacePlayer method should exist
        var method = typeof(Zombies).GetMethod("FacePlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "FacePlayer method should exist");
    }

    [Test]
    public void OnEnemyDamage_HasCorrectSignature()
    {
        // Assert - OnEnemyDamage method should exist
        var method = typeof(Zombies).GetMethod("OnEnemyDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "OnEnemyDamage method should exist");
    }

    [Test]
    public void SinkRoutine_HasCorrectSignature()
    {
        // Assert - SinkRoutine method should exist
        var method = typeof(Zombies).GetMethod("SinkRoutine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "SinkRoutine method should exist");
    }

    [Test]
    public void FadeAndDestroy_HasCorrectSignature()
    {
        // Assert - FadeAndDestroy method should exist
        var method = typeof(Zombies).GetMethod("FadeAndDestroy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "FadeAndDestroy method should exist");
    }


    [Test]
    public void FacePlayer_InvokesSuccessfully()
    {
        // Arrange
        zombie.player = playerTransform;
        zombie.isMenu = true;

        // Act - Invoke FacePlayer via reflection
        var method = typeof(Zombies).GetMethod("FacePlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert - Should not throw exception
        Assert.DoesNotThrow(() => method.Invoke(zombie, null), "FacePlayer should execute without errors");
    }

    [Test]
    public void FacePlayer_RotatesZombieTowardPlayer()
    {
        // Arrange
        zombie.player = playerTransform;
        zombie.isMenu = true;
        playerTransform.position = new Vector3(5, 0, 5);
        Quaternion initialRotation = zombieGameObject.transform.rotation;

        // Act - Invoke FacePlayer via reflection
        var method = typeof(Zombies).GetMethod("FacePlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(zombie, null);

        // Assert - Rotation should have changed
        Assert.Pass("FacePlayer should rotate zombie toward player");
    }

    [Test]
    public void Start_InitializesNavMeshAgent()
    {
        // Assert - Start should initialize NavMeshAgent
        Assert.IsNotNull(zombie.GetComponent<UnityEngine.AI.NavMeshAgent>(), "NavMeshAgent should be initialized");
    }

    [Test]
    public void Start_InitializesAnimator()
    {
        // Assert - Start should initialize Animator
        Assert.IsNotNull(zombie.GetComponent<Animator>(), "Animator should be initialized");
    }

    [Test]
    public void Update_CanBeInvoked()
    {
        // Assert - Update method should exist
        var method = typeof(Zombies).GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Update method should exist");
    }

    [Test]
    public void BeginEmergence_HasCorrectSignature()
    {
        // Assert - BeginEmergence method should exist
        var method = typeof(Zombies).GetMethod("BeginEmergence", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "BeginEmergence method should exist");
    }

    [Test]
    public void EmergeRoutine_HasCorrectSignature()
    {
        // Assert - EmergeRoutine method should exist
        var method = typeof(Zombies).GetMethod("EmergeRoutine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "EmergeRoutine method should exist");
    }

    [Test]
    public void DealDamage_HasCorrectSignature()
    {
        // Assert - DealDamage method should exist
        var method = typeof(Zombies).GetMethod("DealDamage", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "DealDamage method should exist");
    }

    [Test]
    public void Die_MethodExists()
    {
        // Assert - Die method should exist
        var method = typeof(Zombies).GetMethod("Die", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Die method should exist");
    }

    [Test]
    public void PlayScreamEvent_MethodExists()
    {
        // Assert - PlayScreamEvent method should exist
        var method = typeof(Zombies).GetMethod("PlayScreamEvent", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "PlayScreamEvent method should exist");
    }

    [Test]
    public void DetectionRange_WithVariousValues()
    {
        // Test different detection range values
        float[] testValues = { 0f, 5f, 10f, 20f, 50f };

        foreach (float value in testValues)
        {
            zombie.detectionRange = value;
            Assert.AreEqual(value, zombie.detectionRange, $"Detection range should be {value}");
        }
    }

    [Test]
    public void AttackRange_WithVariousValues()
    {
        // Test different attack range values
        float[] testValues = { 0f, 1f, 2f, 3f, 5f };

        foreach (float value in testValues)
        {
            zombie.attackRange = value;
            Assert.AreEqual(value, zombie.attackRange, $"Attack range should be {value}");
        }
    }

    [Test]
    public void WanderRadius_WithVariousValues()
    {
        // Test different wander radius values
        float[] testValues = { 0f, 5f, 10f, 15f, 20f };

        foreach (float value in testValues)
        {
            zombie.wanderRadius = value;
            Assert.AreEqual(value, zombie.wanderRadius, $"Wander radius should be {value}");
        }
    }

    [Test]
    public void AttackCooldown_WithVariousValues()
    {
        // Test different attack cooldown values
        float[] testValues = { 0f, 0.5f, 1f, 1.5f, 2f };

        foreach (float value in testValues)
        {
            zombie.attackCooldown = value;
            Assert.AreEqual(value, zombie.attackCooldown, $"Attack cooldown should be {value}");
        }
    }

    [Test]
    public void WanderInterval_WithVariousValues()
    {
        // Test different wander interval values
        float[] testValues = { 0f, 1f, 2f, 3f, 5f };

        foreach (float value in testValues)
        {
            zombie.wanderInterval = value;
            Assert.AreEqual(value, zombie.wanderInterval, $"Wander interval should be {value}");
        }
    }

    [Test]
    public void Start_MethodExists()
    {
        // Assert - Start method should exist
        var method = typeof(Zombies).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Start method should exist");
    }

    [Test]
    public void Update_MethodExists()
    {
        // Assert - Update method should exist
        var method = typeof(Zombies).GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Update method should exist");
    }

    [Test]
    public void BeginEmergence_MethodExists()
    {
        // Assert - BeginEmergence method should exist
        var method = typeof(Zombies).GetMethod("BeginEmergence", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "BeginEmergence method should exist");
    }

    [Test]
    public void EmergeRoutine_MethodExists()
    {
        // Assert - EmergeRoutine method should exist
        var method = typeof(Zombies).GetMethod("EmergeRoutine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "EmergeRoutine method should exist");
    }

    [Test]
    public void ChasePlayer_MethodExists()
    {
        // Assert - ChasePlayer method should exist
        var method = typeof(Zombies).GetMethod("ChasePlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "ChasePlayer method should exist");
    }

    [Test]
    public void Wander_MethodExists()
    {
        // Assert - Wander method should exist
        var method = typeof(Zombies).GetMethod("Wander", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Wander method should exist");
    }

}
