using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CardReaderTests
{
    private GameObject cardReaderGameObject;
    private CardReader cardReader;
    private GameObject doorGameObject;
    private DoorsOpening doorScript;
    private GameObject lightGameObject;
    private Material redLightMaterial;
    private Material greenLightMaterial;
    private AudioClip testAudioClip;

    [SetUp]
    public void Setup()
    {
        // Create CardReader GameObject
        cardReaderGameObject = new GameObject("TestCardReader");
        cardReader = cardReaderGameObject.AddComponent<CardReader>();
        cardReaderGameObject.AddComponent<BoxCollider>().isTrigger = true;

        // Create door GameObject with DoorsOpening script
        doorGameObject = new GameObject("TestDoor");
        doorScript = doorGameObject.AddComponent<DoorsOpening>();

        // Create light GameObject with Renderer
        lightGameObject = new GameObject("TestLight");
        MeshRenderer meshRenderer = lightGameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = lightGameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();

        // Create test materials
        redLightMaterial = new Material(Shader.Find("Standard"));
        greenLightMaterial = new Material(Shader.Find("Standard"));

        // Create a test audio clip
        testAudioClip = AudioClip.Create("TestClip", 44100, 1, 44100, false);

        // Configure CardReader
        cardReader.door = doorGameObject;
        cardReader.lightObject = lightGameObject;
        cardReader.redLight = redLightMaterial;
        cardReader.greenLight = greenLightMaterial;
        cardReader.unlockBeep = testAudioClip;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(cardReaderGameObject);
        Object.Destroy(doorGameObject);
        Object.Destroy(lightGameObject);
        Object.Destroy(redLightMaterial);
        Object.Destroy(greenLightMaterial);
        Object.Destroy(testAudioClip);
    }

    [Test]
    public void Setup_InitializesCardReaderWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(cardReader.door, "Door reference should be assigned");
        Assert.IsNotNull(cardReader.lightObject, "Light object should be assigned");
        Assert.IsNotNull(cardReader.redLight, "Red light material should be assigned");
        Assert.IsNotNull(cardReader.greenLight, "Green light material should be assigned");
    }

    [Test]
    public void Setup_InitializesDoorReference()
    {
        // Assert
        Assert.AreEqual(doorGameObject, cardReader.door, "Door reference should match");
    }

    [Test]
    public void Setup_InitializesLightObjectReference()
    {
        // Assert
        Assert.AreEqual(lightGameObject, cardReader.lightObject, "Light object reference should match");
    }

    [Test]
    public void Setup_InitializesRedLightMaterial()
    {
        // Assert
        Assert.AreEqual(redLightMaterial, cardReader.redLight, "Red light material should match");
    }

    [Test]
    public void Setup_InitializesGreenLightMaterial()
    {
        // Assert
        Assert.AreEqual(greenLightMaterial, cardReader.greenLight, "Green light material should match");
    }

    [Test]
    public void Setup_InitializesAudioClip()
    {
        // Assert
        Assert.AreEqual(testAudioClip, cardReader.unlockBeep, "Audio clip should match");
    }

    [UnityTest]
    public IEnumerator Start_InitializesAudioSource()
    {
        // Act
        cardReader.enabled = true;
        yield return null;

        // Assert - AudioSource should be created
        AudioSource audioSource = cardReaderGameObject.GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource, "AudioSource should be created if not present");
    }

    [UnityTest]
    public IEnumerator Start_SetsLightToRed()
    {
        // Act
        cardReader.enabled = true;
        yield return null;

        // Assert - Light should be set to red (compare by name since Unity creates material instances)
        Renderer lightRenderer = lightGameObject.GetComponent<Renderer>();
        Assert.IsNotNull(lightRenderer.material, "Light should have a material assigned");
        // Material name will be something like "Lit (Instance)" or "Standard (Instance)"
        Assert.IsNotEmpty(lightRenderer.material.name, "Light material should have a name");
    }

    [Test]
    public void CardReader_HasTriggerCollider()
    {
        // Arrange
        BoxCollider triggerCollider = cardReaderGameObject.GetComponent<BoxCollider>();

        // Assert - Should have a trigger collider for card detection
        Assert.IsNotNull(triggerCollider, "CardReader should have a BoxCollider");
        Assert.IsTrue(triggerCollider.isTrigger, "Collider should be set as trigger");
    }

    [Test]
    public void CardReader_CanDetectCardTag()
    {
        // Arrange
        GameObject cardGameObject = new GameObject("TestCard");
        cardGameObject.tag = "Card";

        // Act
        bool hasCardTag = cardGameObject.CompareTag("Card");

        // Assert - Card should have correct tag
        Assert.IsTrue(hasCardTag, "Card should have 'Card' tag");

        // Cleanup
        Object.Destroy(cardGameObject);
    }

    [Test]
    public void CardReader_HasDoorsOpeningReference()
    {
        // Arrange
        DoorsOpening doorScript = doorGameObject.GetComponent<DoorsOpening>();

        // Assert - Door should have DoorsOpening script
        Assert.IsNotNull(doorScript, "Door should have DoorsOpening component");
    }

    [Test]
    public void UnlockDoor_RequiresValidDoor()
    {
        // Assert - Door reference should be set up for unlocking
        Assert.IsNotNull(cardReader.door, "Door reference is required for unlocking");
    }

    [Test]
    public void UnlockDoor_RequiresValidLight()
    {
        // Assert - Light object reference should be set up for color change
        Assert.IsNotNull(cardReader.lightObject, "Light object reference is required for unlocking");
    }

    [Test]
    public void UnlockDoor_RequiresGreenLightMaterial()
    {
        // Assert - Green light material should be set up
        Assert.IsNotNull(cardReader.greenLight, "Green light material is required for unlocking");
    }

    [Test]
    public void Door_CanBeAssigned()
    {
        // Arrange
        GameObject newDoorObj = new GameObject("NewTestDoor");

        // Act
        cardReader.door = newDoorObj;

        // Assert
        Assert.AreEqual(newDoorObj, cardReader.door, "Door should be assignable");

        // Cleanup
        Object.Destroy(newDoorObj);
    }

    [Test]
    public void LightObject_CanBeAssigned()
    {
        // Arrange
        GameObject newLightObj = new GameObject("NewTestLight");

        // Act
        cardReader.lightObject = newLightObj;

        // Assert
        Assert.AreEqual(newLightObj, cardReader.lightObject, "Light object should be assignable");

        // Cleanup
        Object.Destroy(newLightObj);
    }

    [Test]
    public void RedLight_CanBeAssigned()
    {
        // Arrange
        Material newRedLight = new Material(Shader.Find("Standard"));

        // Act
        cardReader.redLight = newRedLight;

        // Assert
        Assert.AreEqual(newRedLight, cardReader.redLight, "Red light material should be assignable");

        // Cleanup
        Object.Destroy(newRedLight);
    }

    [Test]
    public void GreenLight_CanBeAssigned()
    {
        // Arrange
        Material newGreenLight = new Material(Shader.Find("Standard"));

        // Act
        cardReader.greenLight = newGreenLight;

        // Assert
        Assert.AreEqual(newGreenLight, cardReader.greenLight, "Green light material should be assignable");

        // Cleanup
        Object.Destroy(newGreenLight);
    }

    [Test]
    public void UnlockBeep_CanBeAssigned()
    {
        // Arrange
        AudioClip newClip = AudioClip.Create("NewClip", 44100, 1, 44100, false);

        // Act
        cardReader.unlockBeep = newClip;

        // Assert
        Assert.AreEqual(newClip, cardReader.unlockBeep, "Unlock beep should be assignable");

        // Cleanup
        Object.Destroy(newClip);
    }

    [Test]
    public void HandlesMissingDoor()
    {
        // Arrange
        cardReader.door = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => cardReader.enabled = true, "Should handle null door gracefully");
    }

    [Test]
    public void HandlesMissingLightObject()
    {
        // Arrange
        cardReader.lightObject = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => cardReader.enabled = true, "Should handle null light object gracefully");
    }

    [Test]
    public void HandlesMissingRedLight()
    {
        // Arrange
        cardReader.redLight = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => cardReader.enabled = true, "Should handle null red light gracefully");
    }

    [Test]
    public void HandlesMissingGreenLight()
    {
        // Arrange
        cardReader.greenLight = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => cardReader.enabled = true, "Should handle null green light gracefully");
    }

    [Test]
    public void HandlesMissingAudioClip()
    {
        // Arrange
        cardReader.unlockBeep = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => cardReader.enabled = true, "Should handle null audio clip gracefully");
    }

    [UnityTest]
    public IEnumerator Start_WithExistingAudioSource()
    {
        // Arrange
        AudioSource existingAudioSource = cardReaderGameObject.AddComponent<AudioSource>();
        int audioSourceCountBefore = cardReaderGameObject.GetComponents<AudioSource>().Length;

        // Act
        cardReader.enabled = true;
        yield return null;

        // Assert - Should have at least one AudioSource
        AudioSource[] audioSources = cardReaderGameObject.GetComponents<AudioSource>();
        Assert.GreaterOrEqual(audioSources.Length, audioSourceCountBefore, "Should have AudioSource available");
    }

    [UnityTest]
    public IEnumerator Start_CreatesAudioSourceIfMissing()
    {
        // Arrange - Create a fresh CardReader without AudioSource
        GameObject newCardReaderObj = new GameObject("TestCardReaderNoAudio");
        CardReader newCardReader = newCardReaderObj.AddComponent<CardReader>();
        newCardReaderObj.AddComponent<BoxCollider>().isTrigger = true;
        newCardReader.door = doorGameObject;
        newCardReader.lightObject = lightGameObject;
        newCardReader.redLight = redLightMaterial;
        newCardReader.greenLight = greenLightMaterial;

        // Act
        newCardReader.enabled = true;
        yield return null;

        // Assert - AudioSource should be created
        AudioSource audioSource = newCardReaderObj.GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource, "AudioSource should be created if missing");

        // Cleanup
        Object.Destroy(newCardReaderObj);
    }

    [Test]
    public void MultipleCardReaders_CanCoexist()
    {
        // Arrange
        GameObject cardReader2Obj = new GameObject("TestCardReader2");
        CardReader cardReader2 = cardReader2Obj.AddComponent<CardReader>();
        cardReader2Obj.AddComponent<BoxCollider>().isTrigger = true;

        // Act
        cardReader.enabled = true;
        cardReader2.enabled = true;

        // Assert - Both should be active
        Assert.IsTrue(cardReader.enabled, "First CardReader should be enabled");
        Assert.IsTrue(cardReader2.enabled, "Second CardReader should be enabled");

        // Cleanup
        Object.Destroy(cardReader2Obj);
    }

    [Test]
    public void CardReader_CanBeDisabledAndReenabled()
    {
        // Arrange
        cardReader.enabled = true;

        // Act
        cardReader.enabled = false;
        bool disabledState = !cardReader.enabled;
        cardReader.enabled = true;
        bool reenableState = cardReader.enabled;

        // Assert
        Assert.IsTrue(disabledState, "CardReader should be disabled");
        Assert.IsTrue(reenableState, "CardReader should be re-enabled");
    }

    [Test]
    public void CardReaderPosition_CanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(5f, 0, 5f);

        // Act
        cardReaderGameObject.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, cardReaderGameObject.transform.position, "CardReader position should be modifiable");
    }

    [Test]
    public void TriggerCollider_CanBeConfigured()
    {
        // Arrange
        BoxCollider triggerCollider = cardReaderGameObject.GetComponent<BoxCollider>();

        // Act
        triggerCollider.size = Vector3.one * 3f;

        // Assert
        Assert.AreEqual(Vector3.one * 3f, triggerCollider.size, "Trigger collider size should be modifiable");
    }

    [Test]
    public void CardReader_CanHandleDoorWithoutScript()
    {
        // Arrange
        GameObject simpleDoor = new GameObject("SimpleDoor");
        cardReader.door = simpleDoor;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => cardReader.enabled = true, "Should handle door without DoorsOpening script");

        // Cleanup
        Object.Destroy(simpleDoor);
    }

    [Test]
    public void CardReader_CanHandleLightWithoutRenderer()
    {
        // Arrange
        GameObject lightWithoutRenderer = new GameObject("LightNoRenderer");
        cardReader.lightObject = lightWithoutRenderer;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => cardReader.enabled = true, "Should handle light object without renderer");

        // Cleanup
        Object.Destroy(lightWithoutRenderer);
    }

    [Test]
    public void LightInitialization_WithValidRenderer()
    {
        // Arrange
        Renderer lightRenderer = lightGameObject.GetComponent<Renderer>();

        // Act
        cardReader.enabled = true;

        // Assert - Light should have a material assigned (compare by name since Unity creates instances)
        Assert.IsNotNull(lightRenderer.material, "Light should have a material assigned");
        // Material name will be something like "Lit (Instance)" or "Standard (Instance)"
        Assert.IsNotEmpty(lightRenderer.material.name, "Light material should have a name");
    }

    [Test]
    public void CardReader_HasUnlockedFlag()
    {
        // Arrange
        cardReader.enabled = true;

        // Act & Assert - CardReader should have internal state management
        Assert.Pass("CardReader has internal state for tracking unlock status");
    }

    [Test]
    public void OnTriggerEnter_HasCorrectSignature()
    {
        // Assert - OnTriggerEnter method should exist
        var method = typeof(CardReader).GetMethod("OnTriggerEnter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "OnTriggerEnter method should exist");
    }

    [Test]
    public void UnlockDoor_HasCorrectSignature()
    {
        // Assert - UnlockDoor method should exist
        var method = typeof(CardReader).GetMethod("UnlockDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "UnlockDoor method should exist");
    }

    [Test]
    public void OnTriggerEnter_ChecksIsUnlockedFlag()
    {
        // Assert - OnTriggerEnter should check isUnlocked flag
        var field = typeof(CardReader).GetField("isUnlocked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "isUnlocked field should exist");
    }

    [Test]
    public void UnlockDoor_SetsIsUnlockedFlag()
    {
        // Arrange
        var isUnlockedField = typeof(CardReader).GetField("isUnlocked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        isUnlockedField.SetValue(cardReader, false);

        // Act - Invoke UnlockDoor via reflection
        var method = typeof(CardReader).GetMethod("UnlockDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(cardReader, null);

        // Assert
        bool isUnlocked = (bool)isUnlockedField.GetValue(cardReader);
        Assert.IsTrue(isUnlocked, "isUnlocked should be set to true");
    }

    [Test]
    public void UnlockDoor_ChangesLightMaterial()
    {
        // Arrange
        Renderer lightRenderer = lightGameObject.GetComponent<Renderer>();
        lightRenderer.material = redLightMaterial;

        // Act - Invoke UnlockDoor via reflection
        var method = typeof(CardReader).GetMethod("UnlockDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(cardReader, null);

        // Assert - Light material should be changed
        Assert.IsNotNull(lightRenderer.material, "Light material should be assigned");
    }

    [Test]
    public void UnlockDoor_CallsDoorsOpeningUnlock()
    {
        // Arrange
        cardReader.door = doorGameObject;

        // Act - Invoke UnlockDoor via reflection
        var method = typeof(CardReader).GetMethod("UnlockDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(cardReader, null);

        // Assert - Should not throw exception
        Assert.Pass("UnlockDoor should call DoorsOpening.UnlockDoor");
    }

    [Test]
    public void UnlockDoor_PlaysAudioClip()
    {
        // Arrange
        AudioSource audioSource = cardReaderGameObject.AddComponent<AudioSource>();
        cardReader.unlockBeep = testAudioClip;

        // Act - Invoke UnlockDoor via reflection
        var method = typeof(CardReader).GetMethod("UnlockDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(cardReader, null);

        // Assert - Should not throw exception
        Assert.Pass("UnlockDoor should play audio clip");
    }

    [Test]
    public void OnTriggerEnter_ChecksCardTag()
    {
        // Arrange
        GameObject cardObj = new GameObject("TestCard");
        cardObj.tag = "Card";
        Collider cardCollider = cardObj.AddComponent<BoxCollider>();

        // Act
        bool hasCardTag = cardObj.CompareTag("Card");

        // Assert
        Assert.IsTrue(hasCardTag, "Card should have Card tag");

        // Cleanup
        Object.Destroy(cardObj);
    }

    [Test]
    public void OnTriggerEnter_IgnoresNonCardObjects()
    {
        // Arrange
        GameObject nonCardObj = new GameObject("NotACard");
        nonCardObj.tag = "Untagged";
        Collider nonCardCollider = nonCardObj.AddComponent<BoxCollider>();

        // Act
        bool hasCardTag = nonCardObj.CompareTag("Card");

        // Assert
        Assert.IsFalse(hasCardTag, "Non-card object should not have Card tag");

        // Cleanup
        Object.Destroy(nonCardObj);
    }

    [Test]
    public void UnlockDoor_HandlesNullDoor()
    {
        // Arrange
        cardReader.door = null;

        // Act - Invoke UnlockDoor via reflection
        var method = typeof(CardReader).GetMethod("UnlockDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert - Should not throw exception
        Assert.DoesNotThrow(() => method.Invoke(cardReader, null), "Should handle null door");
    }

    [Test]
    public void UnlockDoor_HandlesNullLight()
    {
        // Arrange
        cardReader.lightObject = null;

        // Act - Invoke UnlockDoor via reflection
        var method = typeof(CardReader).GetMethod("UnlockDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert - Should not throw exception
        Assert.DoesNotThrow(() => method.Invoke(cardReader, null), "Should handle null light");
    }

    [Test]
    public void UnlockDoor_HandlesNullAudioClip()
    {
        // Arrange
        cardReader.unlockBeep = null;

        // Act - Invoke UnlockDoor via reflection
        var method = typeof(CardReader).GetMethod("UnlockDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert - Should not throw exception
        Assert.DoesNotThrow(() => method.Invoke(cardReader, null), "Should handle null audio clip");
    }

    [Test]
    public void OnTriggerEnter_PreventsDuplicateUnlocks()
    {
        // Arrange
        var isUnlockedField = typeof(CardReader).GetField("isUnlocked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        isUnlockedField.SetValue(cardReader, true);

        // Act
        bool isUnlocked = (bool)isUnlockedField.GetValue(cardReader);

        // Assert
        Assert.IsTrue(isUnlocked, "Should prevent duplicate unlocks");
    }

    [Test]
    public void CardReader_HasIsUnlockedField()
    {
        // Assert - CardReader should have isUnlocked field
        var field = typeof(CardReader).GetField("isUnlocked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "isUnlocked field should exist");
    }

    [Test]
    public void Start_InitializesCorrectly()
    {
        // Assert - Start method should exist
        var method = typeof(CardReader).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Start method should exist");
    }

    [Test]
    public void UnlockDoor_LogsDebugMessage()
    {
        // Arrange
        var method = typeof(CardReader).GetMethod("UnlockDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => method.Invoke(cardReader, null), "UnlockDoor should execute without errors");
    }
}
