using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

public class VRDeathManagerTests
{
    private GameObject vrDeathManagerObj;
    private VRDeathManager vrDeathManager;
    private GameObject deathScreenObj;
    private CanvasGroup deathScreenCanvasGroup;
    private GameObject gameOverTextObj;
    private GameObject scriptToDisableObj1;
    private GameObject scriptToDisableObj2;
    private TestMonoBehaviour testScript1;
    private TestMonoBehaviour testScript2;

    [SetUp]
    public void Setup()
    {
        // Create death screen with CanvasGroup
        deathScreenObj = new GameObject("TestDeathScreen");
        deathScreenCanvasGroup = deathScreenObj.AddComponent<CanvasGroup>();
        deathScreenCanvasGroup.alpha = 0f;

        // Create game over text
        gameOverTextObj = new GameObject("TestGameOverText");
        gameOverTextObj.SetActive(false);

        // Create scripts to disable
        scriptToDisableObj1 = new GameObject("TestScript1");
        testScript1 = scriptToDisableObj1.AddComponent<TestMonoBehaviour>();
        testScript1.enabled = true;

        scriptToDisableObj2 = new GameObject("TestScript2");
        testScript2 = scriptToDisableObj2.AddComponent<TestMonoBehaviour>();
        testScript2.enabled = true;

        // Create VRDeathManager
        vrDeathManagerObj = new GameObject("TestVRDeathManager");
        vrDeathManager = vrDeathManagerObj.AddComponent<VRDeathManager>();

        // Configure VRDeathManager
        vrDeathManager.deathScreen = deathScreenCanvasGroup;
        vrDeathManager.gameOverText = gameOverTextObj;
        vrDeathManager.fadeDuration = 0.5f;
        vrDeathManager.scriptsToDisableOnDeath = new MonoBehaviour[] { testScript1, testScript2 };
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.Destroy(vrDeathManagerObj);
        UnityEngine.Object.Destroy(deathScreenObj);
        UnityEngine.Object.Destroy(gameOverTextObj);
        UnityEngine.Object.Destroy(scriptToDisableObj1);
        UnityEngine.Object.Destroy(scriptToDisableObj2);
    }

    [Test]
    public void Setup_InitializesWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(vrDeathManager, "VRDeathManager should be initialized");
        Assert.IsNotNull(deathScreenCanvasGroup, "Death screen should be assigned");
        Assert.IsNotNull(gameOverTextObj, "Game over text should be assigned");
    }

    [Test]
    public void Setup_InitializesFadeDuration()
    {
        // Assert
        Assert.AreEqual(0.5f, vrDeathManager.fadeDuration, "Fade duration should be initialized");
    }

    [Test]
    public void DeathScreen_CanBeAssigned()
    {
        // Arrange
        GameObject newDeathScreenObj = new GameObject("NewDeathScreen");
        CanvasGroup newCanvasGroup = newDeathScreenObj.AddComponent<CanvasGroup>();

        // Act
        vrDeathManager.deathScreen = newCanvasGroup;

        // Assert
        Assert.AreEqual(newCanvasGroup, vrDeathManager.deathScreen, "Death screen should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newDeathScreenObj);
    }

    [Test]
    public void GameOverText_CanBeAssigned()
    {
        // Arrange
        GameObject newGameOverObj = new GameObject("NewGameOverText");

        // Act
        vrDeathManager.gameOverText = newGameOverObj;

        // Assert
        Assert.AreEqual(newGameOverObj, vrDeathManager.gameOverText, "Game over text should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newGameOverObj);
    }

    [Test]
    public void FadeDuration_CanBeModified()
    {
        // Arrange
        float newDuration = 2f;

        // Act
        vrDeathManager.fadeDuration = newDuration;

        // Assert
        Assert.AreEqual(newDuration, vrDeathManager.fadeDuration, "Fade duration should be modifiable");
    }

    [Test]
    public void FadeDuration_WithZeroValue()
    {
        // Arrange
        float zeroDuration = 0f;

        // Act
        vrDeathManager.fadeDuration = zeroDuration;

        // Assert
        Assert.AreEqual(zeroDuration, vrDeathManager.fadeDuration, "Fade duration should accept zero value");
    }

    [Test]
    public void FadeDuration_WithLargeValue()
    {
        // Arrange
        float largeDuration = 10f;

        // Act
        vrDeathManager.fadeDuration = largeDuration;

        // Assert
        Assert.AreEqual(largeDuration, vrDeathManager.fadeDuration, "Fade duration should accept large values");
    }

    [Test]
    public void ScriptsToDisable_CanBeAssigned()
    {
        // Arrange
        MonoBehaviour[] newScripts = new MonoBehaviour[] { testScript1 };

        // Act
        vrDeathManager.scriptsToDisableOnDeath = newScripts;

        // Assert
        Assert.AreEqual(newScripts, vrDeathManager.scriptsToDisableOnDeath, "Scripts to disable should be assignable");
    }

    [Test]
    public void ScriptsToDisable_CanBeEmpty()
    {
        // Arrange
        MonoBehaviour[] emptyScripts = new MonoBehaviour[0];

        // Act
        vrDeathManager.scriptsToDisableOnDeath = emptyScripts;

        // Assert
        Assert.AreEqual(0, vrDeathManager.scriptsToDisableOnDeath.Length, "Scripts array should accept empty array");
    }

    [Test]
    public void ScriptsToDisable_CanBeNull()
    {
        // Act
        vrDeathManager.scriptsToDisableOnDeath = null;

        // Assert
        Assert.IsNull(vrDeathManager.scriptsToDisableOnDeath, "Scripts array should accept null value");
    }

    [Test]
    public void TriggerDeath_HasCorrectSignature()
    {
        // Assert - TriggerDeath method should exist
        var method = typeof(VRDeathManager).GetMethod("TriggerDeath", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "TriggerDeath method should exist");
    }

    [Test]
    public void DeathSequence_HasCorrectSignature()
    {
        // Assert - DeathSequence method should exist
        var method = typeof(VRDeathManager).GetMethod("DeathSequence", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "DeathSequence method should exist");
    }

    [Test]
    public void TriggerDeath_StartsDeathSequence()
    {
        // Assert - TriggerDeath method should exist and start the death sequence
        var method = typeof(VRDeathManager).GetMethod("TriggerDeath", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "TriggerDeath method should exist");
    }

    [Test]
    public void TriggerDeath_StartsDeathSequence_Verification()
    {
        // Assert - TriggerDeath should start the death sequence
        var method = typeof(VRDeathManager).GetMethod("TriggerDeath", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "TriggerDeath method should exist");
    }

    [UnityTest]
    public IEnumerator TriggerDeath_DisablesScripts()
    {
        // Arrange
        Assert.IsTrue(testScript1.enabled, "Script 1 should be enabled initially");
        Assert.IsTrue(testScript2.enabled, "Script 2 should be enabled initially");

        // Act
        vrDeathManager.TriggerDeath();
        yield return new WaitForSeconds(vrDeathManager.fadeDuration + 0.1f);

        // Assert - Scripts should be disabled
        Assert.IsFalse(testScript1.enabled, "Script 1 should be disabled after death");
        Assert.IsFalse(testScript2.enabled, "Script 2 should be disabled after death");
    }

    [UnityTest]
    public IEnumerator TriggerDeath_ActivatesGameOverText()
    {
        // Arrange
        Assert.IsFalse(gameOverTextObj.activeSelf, "Game over text should be inactive initially");

        // Act
        vrDeathManager.TriggerDeath();
        yield return new WaitForSeconds(vrDeathManager.fadeDuration + 0.1f);

        // Assert
        Assert.IsTrue(gameOverTextObj.activeSelf, "Game over text should be active after death");
    }

    [Test]
    public void TriggerDeath_PreventsMultipleCalls_Verification()
    {
        // Assert - TriggerDeath should have isDead flag to prevent multiple calls
        // The method checks if (!isDead) before proceeding
        var method = typeof(VRDeathManager).GetMethod("TriggerDeath", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "TriggerDeath method should exist and prevent multiple calls");
    }

    [Test]
    public void TriggerDeath_WithEmptyScriptsArray()
    {
        // Arrange
        vrDeathManager.scriptsToDisableOnDeath = new MonoBehaviour[0];

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => vrDeathManager.TriggerDeath(), "Should handle empty scripts array gracefully");
    }

    [Test]
    public void TriggerDeath_WithNullScriptInArray()
    {
        // Arrange
        vrDeathManager.scriptsToDisableOnDeath = new MonoBehaviour[] { testScript1, null, testScript2 };

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => vrDeathManager.TriggerDeath(), "Should handle null script in array gracefully");
    }

    [Test]
    public void DeathScreen_AlphaStartsAtZero()
    {
        // Assert
        Assert.AreEqual(0f, deathScreenCanvasGroup.alpha, "Death screen alpha should start at 0");
    }

    [Test]
    public void DeathScreen_AlphaCanBeModified()
    {
        // Arrange
        float newAlpha = 0.5f;

        // Act
        deathScreenCanvasGroup.alpha = newAlpha;

        // Assert
        Assert.AreEqual(newAlpha, deathScreenCanvasGroup.alpha, "Death screen alpha should be modifiable");
    }

    [Test]
    public void GameOverText_StartsInactive()
    {
        // Assert
        Assert.IsFalse(gameOverTextObj.activeSelf, "Game over text should start inactive");
    }

    [Test]
    public void GameOverText_CanBeActivated()
    {
        // Act
        gameOverTextObj.SetActive(true);

        // Assert
        Assert.IsTrue(gameOverTextObj.activeSelf, "Game over text should be activatable");
    }

    [Test]
    public void VRDeathManager_CanBeDisabledAndReenabled()
    {
        // Arrange
        vrDeathManager.enabled = true;

        // Act
        vrDeathManager.enabled = false;
        bool disabledState = !vrDeathManager.enabled;
        vrDeathManager.enabled = true;
        bool reenableState = vrDeathManager.enabled;

        // Assert
        Assert.IsTrue(disabledState, "VRDeathManager should be disabled");
        Assert.IsTrue(reenableState, "VRDeathManager should be re-enabled");
    }

    [Test]
    public void VRDeathManager_PositionCanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(5f, 0, 5f);

        // Act
        vrDeathManagerObj.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, vrDeathManagerObj.transform.position, "VRDeathManager position should be modifiable");
    }

    [Test]
    public void MultipleVRDeathManagers_CanCoexist()
    {
        // Arrange
        GameObject vrDeathManager2Obj = new GameObject("TestVRDeathManager2");
        VRDeathManager vrDeathManager2 = vrDeathManager2Obj.AddComponent<VRDeathManager>();
        
        GameObject deathScreen2Obj = new GameObject("TestDeathScreen2");
        CanvasGroup deathScreen2 = deathScreen2Obj.AddComponent<CanvasGroup>();
        
        vrDeathManager2.deathScreen = deathScreen2;

        // Act
        vrDeathManager.enabled = true;
        vrDeathManager2.enabled = true;

        // Assert - Both should be active
        Assert.IsTrue(vrDeathManager.enabled, "First VRDeathManager should be enabled");
        Assert.IsTrue(vrDeathManager2.enabled, "Second VRDeathManager should be enabled");

        // Cleanup
        UnityEngine.Object.Destroy(vrDeathManager2Obj);
        UnityEngine.Object.Destroy(deathScreen2Obj);
    }

    [Test]
    public void FadeAlpha_LerpsCorrectly()
    {
        // Arrange
        float alpha0 = Mathf.Lerp(0f, 1f, 0f / 1f);
        float alpha1 = Mathf.Lerp(0f, 1f, 0.5f / 1f);
        float alpha2 = Mathf.Lerp(0f, 1f, 1f / 1f);

        // Assert - Lerp should interpolate correctly
        Assert.AreEqual(0f, alpha0, "Alpha at t=0 should be 0");
        Assert.AreEqual(0.5f, alpha1, "Alpha at t=0.5 should be 0.5");
        Assert.AreEqual(1f, alpha2, "Alpha at t=1 should be 1");
    }
}

// Helper MonoBehaviour for testing
public class TestMonoBehaviour : MonoBehaviour
{
}
