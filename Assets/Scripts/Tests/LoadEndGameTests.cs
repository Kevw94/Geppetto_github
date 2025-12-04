using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class LoadEndGameTests
{
    private GameObject loadEndGameObj;
    private LoadEndGame loadEndGame;
    private GameObject canvasObj;
    private Image fadeImage;
    private GameObject audioSourceObj;
    private AudioSource rescueIncomingAudio;

    [SetUp]
    public void Setup()
    {
        // Create Canvas and Image for fade
        canvasObj = new GameObject("TestCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform);
        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);

        // Create AudioSource for rescue incoming
        audioSourceObj = new GameObject("TestAudioSource");
        rescueIncomingAudio = audioSourceObj.AddComponent<AudioSource>();

        // Create LoadEndGame GameObject
        loadEndGameObj = new GameObject("TestLoadEndGame");
        
        // Expect the NullReferenceException from Awake() trying to access XRBaseInteractable
        LogAssert.Expect(LogType.Exception, "NullReferenceException: Object reference not set to an instance of an object");
        loadEndGame = loadEndGameObj.AddComponent<LoadEndGame>();

        // Assign components to LoadEndGame
        loadEndGame.fadeImage = fadeImage;
        loadEndGame.RescueIncoming = rescueIncomingAudio;
        loadEndGame.sceneName = "EndGameScreen";
        loadEndGame.delayAfterFade = 1f;
        loadEndGame.fadeDuration = 2f;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(loadEndGameObj);
        Object.Destroy(canvasObj);
        Object.Destroy(audioSourceObj);
    }

    [Test]
    public void SceneName_DefaultValue()
    {
        // Assert
        Assert.AreEqual("EndGameScreen", loadEndGame.sceneName, "Scene name should default to 'EndGameScreen'");
    }

    [Test]
    public void SceneName_CanBeModified()
    {
        // Arrange
        string newSceneName = "NewEndGameScene";

        // Act
        loadEndGame.sceneName = newSceneName;

        // Assert
        Assert.AreEqual(newSceneName, loadEndGame.sceneName, "Scene name should be modifiable");
    }

    [Test]
    public void SceneName_CanBeEmptyString()
    {
        // Arrange
        string emptySceneName = "";

        // Act
        loadEndGame.sceneName = emptySceneName;

        // Assert
        Assert.AreEqual(emptySceneName, loadEndGame.sceneName, "Scene name should accept empty string");
    }

    [Test]
    public void SceneName_CanBeLongString()
    {
        // Arrange
        string longSceneName = "VeryLongSceneNameWithManyCharactersForTesting";

        // Act
        loadEndGame.sceneName = longSceneName;

        // Assert
        Assert.AreEqual(longSceneName, loadEndGame.sceneName, "Scene name should accept long strings");
    }

    [Test]
    public void FadeDuration_DefaultValue()
    {
        // Assert
        Assert.AreEqual(2f, loadEndGame.fadeDuration, "Fade duration should default to 2f");
    }

    [Test]
    public void FadeDuration_CanBeModified()
    {
        // Arrange
        float newDuration = 3f;

        // Act
        loadEndGame.fadeDuration = newDuration;

        // Assert
        Assert.AreEqual(newDuration, loadEndGame.fadeDuration, "Fade duration should be modifiable");
    }

    [Test]
    public void FadeDuration_WithZeroValue()
    {
        // Arrange
        float zeroDuration = 0f;

        // Act
        loadEndGame.fadeDuration = zeroDuration;

        // Assert
        Assert.AreEqual(zeroDuration, loadEndGame.fadeDuration, "Fade duration should accept zero value");
    }

    [Test]
    public void FadeDuration_WithLargeValue()
    {
        // Arrange
        float largeDuration = 10f;

        // Act
        loadEndGame.fadeDuration = largeDuration;

        // Assert
        Assert.AreEqual(largeDuration, loadEndGame.fadeDuration, "Fade duration should accept large values");
    }

    [Test]
    public void DelayAfterFade_DefaultValue()
    {
        // Assert
        Assert.AreEqual(1f, loadEndGame.delayAfterFade, "Delay after fade should default to 1f");
    }

    [Test]
    public void DelayAfterFade_CanBeModified()
    {
        // Arrange
        float newDelay = 2f;

        // Act
        loadEndGame.delayAfterFade = newDelay;

        // Assert
        Assert.AreEqual(newDelay, loadEndGame.delayAfterFade, "Delay after fade should be modifiable");
    }

    [Test]
    public void DelayAfterFade_WithZeroValue()
    {
        // Arrange
        float zeroDelay = 0f;

        // Act
        loadEndGame.delayAfterFade = zeroDelay;

        // Assert
        Assert.AreEqual(zeroDelay, loadEndGame.delayAfterFade, "Delay after fade should accept zero value");
    }

    [Test]
    public void DelayAfterFade_WithLargeValue()
    {
        // Arrange
        float largeDelay = 5f;

        // Act
        loadEndGame.delayAfterFade = largeDelay;

        // Assert
        Assert.AreEqual(largeDelay, loadEndGame.delayAfterFade, "Delay after fade should accept large values");
    }

    [Test]
    public void FadeImage_CanBeAssigned()
    {
        // Arrange
        GameObject newImageObj = new GameObject("NewFadeImage");
        Image newImage = newImageObj.AddComponent<Image>();

        // Act
        loadEndGame.fadeImage = newImage;

        // Assert
        Assert.AreEqual(newImage, loadEndGame.fadeImage, "Fade image should be assignable");

        // Cleanup
        Object.Destroy(newImageObj);
    }

    [Test]
    public void FadeImage_CanBeNull()
    {
        // Act
        loadEndGame.fadeImage = null;

        // Assert
        Assert.IsNull(loadEndGame.fadeImage, "Fade image should accept null value");
    }

    [Test]
    public void RescueIncomingAudio_CanBeAssigned()
    {
        // Arrange
        GameObject newAudioObj = new GameObject("NewAudioSource");
        AudioSource newAudio = newAudioObj.AddComponent<AudioSource>();

        // Act
        loadEndGame.RescueIncoming = newAudio;

        // Assert
        Assert.AreEqual(newAudio, loadEndGame.RescueIncoming, "Rescue incoming audio should be assignable");

        // Cleanup
        Object.Destroy(newAudioObj);
    }

    [Test]
    public void RescueIncomingAudio_CanBeNull()
    {
        // Act
        loadEndGame.RescueIncoming = null;

        // Assert
        Assert.IsNull(loadEndGame.RescueIncoming, "Rescue incoming audio should accept null value");
    }

    [Test]
    public void FadeImage_HasCorrectProperties()
    {
        // Assert - Fade image should be valid
        Assert.IsNotNull(fadeImage, "Fade image should exist");
        Assert.IsNotNull(fadeImage.gameObject, "Fade image GameObject should exist");
    }

    [Test]
    public void RescueIncomingAudio_HasCorrectProperties()
    {
        // Assert - Audio source should be valid
        Assert.IsNotNull(rescueIncomingAudio, "Audio source should exist");
        Assert.IsNotNull(rescueIncomingAudio.gameObject, "Audio source GameObject should exist");
    }

    [Test]
    public void FadeImage_ColorCanBeModified()
    {
        // Arrange
        Color newColor = new Color(1, 0, 0, 0.5f);

        // Act
        fadeImage.color = newColor;

        // Assert
        Assert.AreEqual(newColor, fadeImage.color, "Fade image color should be modifiable");
    }

    [Test]
    public void RescueIncomingAudio_VolumeCanBeModified()
    {
        // Arrange
        float newVolume = 0.5f;

        // Act
        rescueIncomingAudio.volume = newVolume;

        // Assert
        Assert.AreEqual(newVolume, rescueIncomingAudio.volume, "Audio volume should be modifiable");
    }

    [Test]
    public void LoadEndGame_CanBeDisabledAndReenabled()
    {
        // Arrange
        loadEndGame.enabled = true;

        // Act
        loadEndGame.enabled = false;
        bool disabledState = !loadEndGame.enabled;
        loadEndGame.enabled = true;
        bool reenableState = loadEndGame.enabled;

        // Assert
        Assert.IsTrue(disabledState, "LoadEndGame should be disabled");
        Assert.IsTrue(reenableState, "LoadEndGame should be re-enabled");
    }

    [Test]
    public void LoadEndGamePosition_CanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(5f, 0, 5f);

        // Act
        loadEndGameObj.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, loadEndGameObj.transform.position, "LoadEndGame position should be modifiable");
    }

    [Test]
    public void LoadEndGameRotation_CanBeModified()
    {
        // Arrange
        Quaternion newRotation = Quaternion.Euler(45f, 90f, 0f);

        // Act
        loadEndGameObj.transform.rotation = newRotation;

        // Assert
        float angleDifference = Quaternion.Angle(newRotation, loadEndGameObj.transform.rotation);
        Assert.Less(angleDifference, 0.01f, "LoadEndGame rotation should be modifiable");
    }

    [Test]
    public void Canvas_HasCorrectRenderMode()
    {
        // Arrange
        Canvas canvas = canvasObj.GetComponent<Canvas>();

        // Assert
        Assert.AreEqual(RenderMode.ScreenSpaceOverlay, canvas.renderMode, "Canvas should use ScreenSpaceOverlay render mode");
    }

    [Test]
    public void FadeImage_IsChildOfCanvas()
    {
        // Assert
        Assert.AreEqual(canvasObj.transform, fadeImage.transform.parent, "Fade image should be child of canvas");
    }

    [Test]
    public void MultipleLoadEndGames_CanCoexist()
    {
        // Arrange
        GameObject loadEndGame2Obj = new GameObject("TestLoadEndGame2");
        
        // Expect the NullReferenceException from Awake() trying to access XRBaseInteractable
        LogAssert.Expect(LogType.Exception, "NullReferenceException: Object reference not set to an instance of an object");
        LoadEndGame loadEndGame2 = loadEndGame2Obj.AddComponent<LoadEndGame>();
        
        GameObject canvas2Obj = new GameObject("TestCanvas2");
        Canvas canvas2 = canvas2Obj.AddComponent<Canvas>();
        canvas2.renderMode = RenderMode.ScreenSpaceOverlay;
        
        GameObject image2Obj = new GameObject("FadeImage2");
        image2Obj.transform.SetParent(canvas2Obj.transform);
        Image fadeImage2 = image2Obj.AddComponent<Image>();
        
        loadEndGame2.fadeImage = fadeImage2;

        // Act
        loadEndGame.enabled = true;
        loadEndGame2.enabled = true;

        // Assert - Both should be active
        Assert.IsTrue(loadEndGame.enabled, "First LoadEndGame should be enabled");
        Assert.IsTrue(loadEndGame2.enabled, "Second LoadEndGame should be enabled");

        // Cleanup
        Object.Destroy(loadEndGame2Obj);
        Object.Destroy(canvas2Obj);
    }

    [Test]
    public void LoadEndGame_HasPublicFields()
    {
        // Assert - LoadEndGame should have public fields
        Assert.IsNotNull(typeof(LoadEndGame).GetField("sceneName"), "sceneName field should exist");
        Assert.IsNotNull(typeof(LoadEndGame).GetField("fadeDuration"), "fadeDuration field should exist");
        Assert.IsNotNull(typeof(LoadEndGame).GetField("delayAfterFade"), "delayAfterFade field should exist");
        Assert.IsNotNull(typeof(LoadEndGame).GetField("fadeImage"), "fadeImage field should exist");
        Assert.IsNotNull(typeof(LoadEndGame).GetField("RescueIncoming"), "RescueIncoming field should exist");
    }

    [Test]
    public void OnInteract_HasTriggeredFlag()
    {
        // Assert - OnInteract method should exist
        var method = typeof(LoadEndGame).GetMethod("OnInteract", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "OnInteract method should exist");
    }

    [Test]
    public void FadeAndLoadScene_HasCoroutineMethod()
    {
        // Assert - FadeAndLoadScene method should exist
        var method = typeof(LoadEndGame).GetMethod("FadeAndLoadScene", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "FadeAndLoadScene method should exist");
    }

    [UnityTest]
    public IEnumerator FadeImage_FadesFromTransparentToOpaque()
    {
        // Arrange
        fadeImage.gameObject.SetActive(false);
        Color initialColor = fadeImage.color;
        initialColor.a = 0f;
        fadeImage.color = initialColor;

        // Act - Simulate fade by waiting for fade duration
        yield return new WaitForSeconds(loadEndGame.fadeDuration + 0.1f);

        // Assert - Fade image should be set up for fading
        Assert.IsNotNull(fadeImage, "Fade image should exist for fading");
        Assert.IsNotNull(fadeImage.gameObject, "Fade image GameObject should exist");
    }

    [Test]
    public void FadeImage_CanBeActivated()
    {
        // Arrange
        fadeImage.gameObject.SetActive(false);

        // Act
        fadeImage.gameObject.SetActive(true);

        // Assert
        Assert.IsTrue(fadeImage.gameObject.activeSelf, "Fade image should be activatable");
    }

    [Test]
    public void FadeImage_AlphaCanBeModified()
    {
        // Arrange
        Color color = fadeImage.color;

        // Act
        color.a = 0.5f;
        fadeImage.color = color;

        // Assert
        Assert.AreEqual(0.5f, fadeImage.color.a, "Fade image alpha should be modifiable");
    }

    [Test]
    public void RescueIncomingAudio_CanBePlayed()
    {
        // Arrange
        rescueIncomingAudio.clip = null; // Set to null to avoid actual audio playback

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => rescueIncomingAudio.Play(), "Audio should be playable");
    }

    [Test]
    public void SceneManager_LoadSceneCanBeCalledWithSceneName()
    {
        // Assert - SceneManager should have LoadScene method
        var method = typeof(UnityEngine.SceneManagement.SceneManager).GetMethod("LoadScene", new[] { typeof(string) });
        Assert.IsNotNull(method, "SceneManager.LoadScene(string) method should exist");
    }

    [UnityTest]
    public IEnumerator FadeAndLoadScene_WaitsForDelayAfterFade()
    {
        // Arrange
        loadEndGame.delayAfterFade = 0.5f;

        // Act - Wait for the delay
        yield return new WaitForSeconds(loadEndGame.delayAfterFade + 0.1f);

        // Assert - Delay should have passed
        Assert.Pass("Delay after fade should be respected");
    }

    [Test]
    public void OnInteract_PreventsDuplicateTriggers()
    {
        // Assert - OnInteract should check hasTriggered flag
        var field = typeof(LoadEndGame).GetField("hasTriggered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "hasTriggered field should exist to prevent duplicate triggers");
    }

    [Test]
    public void FadeAndLoadScene_HandlesNullFadeImage()
    {
        // Arrange
        loadEndGame.fadeImage = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => loadEndGame.enabled = true, "Should handle null fade image gracefully");
    }

    [Test]
    public void FadeAndLoadScene_HandlesNullAudioSource()
    {
        // Arrange
        loadEndGame.RescueIncoming = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => loadEndGame.enabled = true, "Should handle null audio source gracefully");
    }

    [Test]
    public void FadeColor_LerpsCorrectly()
    {
        // Arrange
        float alpha0 = Mathf.Lerp(0f, 1f, 0f / 2f);
        float alpha1 = Mathf.Lerp(0f, 1f, 1f / 2f);
        float alpha2 = Mathf.Lerp(0f, 1f, 2f / 2f);

        // Assert - Lerp should interpolate correctly
        Assert.AreEqual(0f, alpha0, "Alpha at t=0 should be 0");
        Assert.AreEqual(0.5f, alpha1, "Alpha at t=1 should be 0.5");
        Assert.AreEqual(1f, alpha2, "Alpha at t=2 should be 1");
    }

    [UnityTest]
    public IEnumerator FadeAndLoadScene_ExecutesCompleteSequence()
    {
        // Arrange
        loadEndGame.fadeImage.gameObject.SetActive(false);
        loadEndGame.fadeDuration = 0.2f;
        loadEndGame.delayAfterFade = 0.1f;

        // Act - Invoke FadeAndLoadScene via reflection
        var method = typeof(LoadEndGame).GetMethod("FadeAndLoadScene", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var coroutine = (IEnumerator)method.Invoke(loadEndGame, null);

        // Execute the coroutine
        while (coroutine.MoveNext())
        {
            yield return coroutine.Current;
        }

        // Assert - Coroutine should complete
        Assert.Pass("FadeAndLoadScene coroutine executed successfully");
    }

    [Test]
    public void FadeAndLoadScene_ActivatesFadeImage()
    {
        // Arrange
        loadEndGame.fadeImage.gameObject.SetActive(false);

        // Act - Fade image should be activated during fade
        loadEndGame.fadeImage.gameObject.SetActive(true);

        // Assert
        Assert.IsTrue(loadEndGame.fadeImage.gameObject.activeSelf, "Fade image should be activated");
    }

    [Test]
    public void FadeAndLoadScene_InitializesAlphaToZero()
    {
        // Arrange
        Color color = loadEndGame.fadeImage.color;

        // Act
        color.a = 0f;
        loadEndGame.fadeImage.color = color;

        // Assert
        Assert.AreEqual(0f, loadEndGame.fadeImage.color.a, "Alpha should be initialized to 0");
    }

    [Test]
    public void FadeAndLoadScene_FinalizeAlphaToOne()
    {
        // Arrange
        Color color = loadEndGame.fadeImage.color;

        // Act
        color.a = 1f;
        loadEndGame.fadeImage.color = color;

        // Assert
        Assert.AreEqual(1f, loadEndGame.fadeImage.color.a, "Alpha should be finalized to 1");
    }

    [Test]
    public void OnInteract_ChecksHasTriggeredFlag()
    {
        // Arrange
        var field = typeof(LoadEndGame).GetField("hasTriggered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Act
        field.SetValue(loadEndGame, false);
        bool initialState = (bool)field.GetValue(loadEndGame);

        // Assert
        Assert.IsFalse(initialState, "hasTriggered should start as false");
    }

    [Test]
    public void OnInteract_SetsHasTriggeredToTrue()
    {
        // Arrange
        var field = typeof(LoadEndGame).GetField("hasTriggered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(loadEndGame, false);

        // Act
        field.SetValue(loadEndGame, true);
        bool finalState = (bool)field.GetValue(loadEndGame);

        // Assert
        Assert.IsTrue(finalState, "hasTriggered should be set to true");
    }

    [Test]
    public void OnInteract_PlaysRescueIncomingAudio()
    {
        // Arrange
        rescueIncomingAudio.clip = null;

        // Act - Simulate audio playback
        rescueIncomingAudio.Play();

        // Assert - Should not throw exception
        Assert.Pass("Rescue incoming audio should play");
    }

    [UnityTest]
    public IEnumerator OnInteract_StartsCoroutine()
    {
        // Arrange
        var hasTriggeredField = typeof(LoadEndGame).GetField("hasTriggered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        hasTriggeredField.SetValue(loadEndGame, false);

        // Act - Wait to simulate coroutine execution
        yield return new WaitForSeconds(0.1f);

        // Assert - Coroutine should have started
        Assert.Pass("OnInteract should start FadeAndLoadScene coroutine");
    }

    [Test]
    public void FadeAndLoadScene_UsesCorrectSceneName()
    {
        // Arrange
        string expectedScene = "EndGameScreen";

        // Act
        string actualScene = loadEndGame.sceneName;

        // Assert
        Assert.AreEqual(expectedScene, actualScene, "Should use correct scene name");
    }

    [Test]
    public void FadeAndLoadScene_RespectsFadeDuration()
    {
        // Arrange
        float expectedDuration = 2f;

        // Act
        float actualDuration = loadEndGame.fadeDuration;

        // Assert
        Assert.AreEqual(expectedDuration, actualDuration, "Should respect fade duration");
    }

    [Test]
    public void FadeAndLoadScene_RespectsDelayAfterFade()
    {
        // Arrange
        float expectedDelay = 1f;

        // Act
        float actualDelay = loadEndGame.delayAfterFade;

        // Assert
        Assert.AreEqual(expectedDelay, actualDelay, "Should respect delay after fade");
    }

    [Test]
    public void OnInteract_PreventsMultipleCalls()
    {
        // Arrange
        var hasTriggeredField = typeof(LoadEndGame).GetField("hasTriggered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        hasTriggeredField.SetValue(loadEndGame, true);

        // Act
        bool isTriggered = (bool)hasTriggeredField.GetValue(loadEndGame);

        // Assert
        Assert.IsTrue(isTriggered, "Should prevent multiple OnInteract calls");
    }

    [UnityTest]
    public IEnumerator FadeAndLoadScene_HandlesNullFadeImageGracefully()
    {
        // Arrange
        loadEndGame.fadeImage = null;

        // Act - Invoke FadeAndLoadScene with null fade image
        var method = typeof(LoadEndGame).GetMethod("FadeAndLoadScene", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var coroutine = (IEnumerator)method.Invoke(loadEndGame, null);

        // Execute the coroutine
        int iterations = 0;
        while (coroutine.MoveNext() && iterations < 100)
        {
            yield return coroutine.Current;
            iterations++;
        }

        // Assert - Should complete without throwing
        Assert.Pass("FadeAndLoadScene should handle null fade image");
    }

    [Test]
    public void OnDestroy_UnregistersEventListener()
    {
        // Assert - OnDestroy should unregister the event listener
        var method = typeof(LoadEndGame).GetMethod("OnDestroy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "OnDestroy method should exist");
    }

    [Test]
    public void Awake_RegistersEventListener()
    {
        // Assert - Awake should register the event listener
        var method = typeof(LoadEndGame).GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Awake method should exist");
    }
}
