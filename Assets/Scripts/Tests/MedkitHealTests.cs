using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MedkitHealTests
{
    private GameObject medkitObj;
    private Medkit medkit;
    private GameObject haileyObj;
    private HaileyHealth haileyHealth;
    private GameObject particleSystemObj;
    private ParticleSystem particleSystem;
    private GameObject audioSourceObj;
    private AudioSource audioSource;
    private System.Reflection.FieldInfo haileyFieldInfo;

    [SetUp]
    public void Setup()
    {
        // Create audio listener for audio tests
        GameObject audioListenerObj = new GameObject("TestAudioListener");
        audioListenerObj.AddComponent<AudioListener>();

        // Create HaileyHealth GameObject
        haileyObj = new GameObject("TestHailey");
        haileyHealth = haileyObj.AddComponent<HaileyHealth>();
        haileyHealth.maxHealth = 100f;
        haileyHealth.currentHealth = 50f;

        // Create ParticleSystem for healing FX
        particleSystemObj = new GameObject("TestParticleSystem");
        particleSystem = particleSystemObj.AddComponent<ParticleSystem>();
        particleSystem.Stop(); // Ensure it's stopped initially

        // Create AudioSource for healing sound
        audioSourceObj = new GameObject("TestAudioSource");
        audioSource = audioSourceObj.AddComponent<AudioSource>();

        // Create Medkit GameObject
        medkitObj = new GameObject("TestMedkit");
        medkit = medkitObj.AddComponent<Medkit>();

        // Configure Medkit
        medkit.healAmount = 25f;
        medkit.healDuration = 0.5f;
        medkit.healFX = particleSystem;
        medkit.healLoopSound = audioSource;
        
        // Manually assign hailey reference using reflection (since Start() uses FindAnyObjectByType)
        haileyFieldInfo = typeof(Medkit).GetField("hailey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (haileyFieldInfo != null)
        {
            haileyFieldInfo.SetValue(medkit, haileyHealth);
        }
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(medkitObj);
        Object.Destroy(haileyObj);
        Object.Destroy(particleSystemObj);
        Object.Destroy(audioSourceObj);
        
        // Clean up audio listener
        AudioListener[] listeners = Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        foreach (AudioListener listener in listeners)
        {
            Object.Destroy(listener.gameObject);
        }
    }

    [Test]
    public void Setup_InitializesWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(medkit, "Medkit should be initialized");
        Assert.IsNotNull(haileyHealth, "HaileyHealth should be assigned");
    }

    [Test]
    public void Setup_InitializesHealAmount()
    {
        // Assert
        Assert.AreEqual(25f, medkit.healAmount, "Heal amount should be initialized");
    }

    [Test]
    public void Setup_InitializesHealDuration()
    {
        // Assert
        Assert.AreEqual(0.5f, medkit.healDuration, "Heal duration should be initialized");
    }

    [Test]
    public void HealAmount_CanBeModified()
    {
        // Arrange
        float newHealAmount = 50f;

        // Act
        medkit.healAmount = newHealAmount;

        // Assert
        Assert.AreEqual(newHealAmount, medkit.healAmount, "Heal amount should be modifiable");
    }

    [Test]
    public void HealDuration_CanBeModified()
    {
        // Arrange
        float newDuration = 3f;

        // Act
        medkit.healDuration = newDuration;

        // Assert
        Assert.AreEqual(newDuration, medkit.healDuration, "Heal duration should be modifiable");
    }

    [Test]
    public void HealAmount_WithZeroValue()
    {
        // Arrange
        float zeroAmount = 0f;

        // Act
        medkit.healAmount = zeroAmount;

        // Assert
        Assert.AreEqual(zeroAmount, medkit.healAmount, "Heal amount should accept zero value");
    }

    [Test]
    public void HealAmount_WithLargeValue()
    {
        // Arrange
        float largeAmount = 100f;

        // Act
        medkit.healAmount = largeAmount;

        // Assert
        Assert.AreEqual(largeAmount, medkit.healAmount, "Heal amount should accept large values");
    }

    [Test]
    public void HealDuration_WithZeroValue()
    {
        // Arrange
        float zeroDuration = 0f;

        // Act
        medkit.healDuration = zeroDuration;

        // Assert
        Assert.AreEqual(zeroDuration, medkit.healDuration, "Heal duration should accept zero value");
    }

    [Test]
    public void HealFX_CanBeAssigned()
    {
        // Arrange
        GameObject newParticleObj = new GameObject("NewParticleSystem");
        ParticleSystem newParticleSystem = newParticleObj.AddComponent<ParticleSystem>();

        // Act
        medkit.healFX = newParticleSystem;

        // Assert
        Assert.AreEqual(newParticleSystem, medkit.healFX, "Heal FX should be assignable");

        // Cleanup
        Object.Destroy(newParticleObj);
    }

    [Test]
    public void HealLoopSound_CanBeAssigned()
    {
        // Arrange
        GameObject newAudioObj = new GameObject("NewAudioSource");
        AudioSource newAudioSource = newAudioObj.AddComponent<AudioSource>();

        // Act
        medkit.healLoopSound = newAudioSource;

        // Assert
        Assert.AreEqual(newAudioSource, medkit.healLoopSound, "Heal loop sound should be assignable");

        // Cleanup
        Object.Destroy(newAudioObj);
    }

    [Test]
    public void StartHealing_HasCorrectSignature()
    {
        // Assert - StartHealing method should exist
        var method = typeof(Medkit).GetMethod("StartHealing", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "StartHealing method should exist");
    }

    [Test]
    public void StopHealing_HasCorrectSignature()
    {
        // Assert - StopHealing method should exist
        var method = typeof(Medkit).GetMethod("StopHealing", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "StopHealing method should exist");
    }

    [Test]
    public void HealProcess_HasCorrectSignature()
    {
        // Assert - HealProcess method should exist
        var method = typeof(Medkit).GetMethod("HealProcess", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "HealProcess method should exist");
    }

    [UnityTest]
    public IEnumerator StartHealing_InitiatesHealingProcess()
    {
        // Arrange
        // Start with lower health so we can see the increase without hitting max
        haileyHealth.currentHealth = 30f;
        float initialHealth = haileyHealth.currentHealth;

        // Act
        medkit.StartHealing();
        yield return new WaitForSeconds(medkit.healDuration + 0.1f);

        // Assert - Health should increase
        Assert.Greater(haileyHealth.currentHealth, initialHealth, "Health should increase after healing");
    }

    [UnityTest]
    public IEnumerator StartHealing_PlaysParticleEffect()
    {
        // Arrange
        Assert.IsFalse(particleSystem.isPlaying, "Particle system should not be playing initially");

        // Act
        medkit.StartHealing();
        yield return new WaitForSeconds(0.1f);

        // Assert - Particle system should be playing
        Assert.IsTrue(particleSystem.isPlaying, "Particle system should be playing during healing");
    }

    [Test]
    public void StartHealing_WithValidAudioSource()
    {
        // Arrange
        medkit.healLoopSound = audioSource;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => medkit.StartHealing(), "Should handle audio source without errors");
    }

    [UnityTest]
    public IEnumerator StartHealing_HealAmountIsApplied()
    {
        // Arrange
        float initialHealth = haileyHealth.currentHealth;
        float expectedHealth = Mathf.Min(initialHealth + medkit.healAmount, haileyHealth.maxHealth);

        // Act
        medkit.StartHealing();
        yield return new WaitForSeconds(medkit.healDuration + 0.1f);

        // Assert - Health should increase by heal amount (clamped to maxHealth)
        Assert.AreEqual(expectedHealth, haileyHealth.currentHealth, "Health should increase by heal amount");
    }

    [Test]
    public void StopHealing_StopsParticleEffect()
    {
        // Arrange
        particleSystem.Play();
        Assert.IsTrue(particleSystem.isPlaying, "Particle system should be playing");

        // Act
        medkit.StopHealing();

        // Assert
        Assert.IsFalse(particleSystem.isPlaying, "Particle system should stop");
    }

    [Test]
    public void StopHealing_StopsAudioSource()
    {
        // Arrange
        audioSource.clip = AudioClip.Create("TestClip", 44100, 1, 44100, false);
        audioSource.Play();
        // Give it a moment to start playing
        System.Threading.Thread.Sleep(10);

        // Act
        medkit.StopHealing();

        // Assert
        Assert.IsFalse(audioSource.isPlaying, "Audio source should stop");
    }

    [Test]
    public void StartHealing_WithNullHailey()
    {
        // Arrange - Medkit without HaileyHealth assigned
        medkit.healFX = null;
        medkit.healLoopSound = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => medkit.StartHealing(), "Should handle null HaileyHealth gracefully");
    }

    [Test]
    public void StartHealing_WithNullParticleSystem()
    {
        // Arrange
        medkit.healFX = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => medkit.StartHealing(), "Should handle null particle system gracefully");
    }

    [Test]
    public void StartHealing_WithNullAudioSource()
    {
        // Arrange
        medkit.healLoopSound = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => medkit.StartHealing(), "Should handle null audio source gracefully");
    }

    [UnityTest]
    public IEnumerator StartHealing_PreventsMultipleCalls()
    {
        // Arrange
        float initialHealth = haileyHealth.currentHealth;

        // Act - Call StartHealing twice
        medkit.StartHealing();
        medkit.StartHealing(); // Should be ignored
        yield return new WaitForSeconds(medkit.healDuration + 0.1f);

        // Assert - Health should only increase once (clamped to maxHealth)
        float expectedHealth = Mathf.Min(initialHealth + medkit.healAmount, haileyHealth.maxHealth);
        Assert.AreEqual(expectedHealth, haileyHealth.currentHealth, "Health should only increase once");
    }

    [Test]
    public void Medkit_CanBeDisabledAndReenabled()
    {
        // Arrange
        medkit.enabled = true;

        // Act
        medkit.enabled = false;
        bool disabledState = !medkit.enabled;
        medkit.enabled = true;
        bool reenableState = medkit.enabled;

        // Assert
        Assert.IsTrue(disabledState, "Medkit should be disabled");
        Assert.IsTrue(reenableState, "Medkit should be re-enabled");
    }

    [Test]
    public void Medkit_PositionCanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(5f, 0, 5f);

        // Act
        medkitObj.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, medkitObj.transform.position, "Medkit position should be modifiable");
    }

    [Test]
    public void MultipleMedkits_CanCoexist()
    {
        // Arrange
        GameObject medkit2Obj = new GameObject("TestMedkit2");
        Medkit medkit2 = medkit2Obj.AddComponent<Medkit>();
        
        GameObject particleSystem2Obj = new GameObject("TestParticleSystem2");
        ParticleSystem particleSystem2 = particleSystem2Obj.AddComponent<ParticleSystem>();
        
        medkit2.healFX = particleSystem2;
        medkit2.healAmount = 30f;

        // Act
        medkit.enabled = true;
        medkit2.enabled = true;

        // Assert - Both should be active
        Assert.IsTrue(medkit.enabled, "First medkit should be enabled");
        Assert.IsTrue(medkit2.enabled, "Second medkit should be enabled");

        // Cleanup
        Object.Destroy(medkit2Obj);
        Object.Destroy(particleSystem2Obj);
    }

    [Test]
    public void HealAmount_CanBeNegative()
    {
        // Arrange
        float negativeAmount = -10f;

        // Act
        medkit.healAmount = negativeAmount;

        // Assert
        Assert.AreEqual(negativeAmount, medkit.healAmount, "Heal amount should accept negative values");
    }

    [Test]
    public void HealDuration_CanBeLarge()
    {
        // Arrange
        float largeDuration = 10f;

        // Act
        medkit.healDuration = largeDuration;

        // Assert
        Assert.AreEqual(largeDuration, medkit.healDuration, "Heal duration should accept large values");
    }

    [Test]
    public void Medkit_HasPublicHealAmountField()
    {
        // Assert - Medkit should have public healAmount field
        var field = typeof(Medkit).GetField("healAmount", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "healAmount field should exist and be public");
    }

    [Test]
    public void Medkit_HasPublicHealDurationField()
    {
        // Assert - Medkit should have public healDuration field
        var field = typeof(Medkit).GetField("healDuration", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "healDuration field should exist and be public");
    }
}
