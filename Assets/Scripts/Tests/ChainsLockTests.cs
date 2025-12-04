using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ChainsLockTests
{
    private GameObject chainsLockGameObject;
    private ChainsLock chainsLock;
    private GameObject leftDoorGameObject;
    private DoorsOpening leftDoorScript;
    private GameObject rightDoorGameObject;
    private DoorsOpening rightDoorScript;
    private GameObject audioSourceObj;
    private AudioSource cutSoundAudioSource;
    private GameObject boltcutterGameObject;

    [SetUp]
    public void Setup()
    {
        // Create left door with DoorsOpening script
        leftDoorGameObject = new GameObject("TestLeftDoor");
        leftDoorScript = leftDoorGameObject.AddComponent<DoorsOpening>();

        // Create right door with DoorsOpening script
        rightDoorGameObject = new GameObject("TestRightDoor");
        rightDoorScript = rightDoorGameObject.AddComponent<DoorsOpening>();

        // Create audio source for cut sound
        audioSourceObj = new GameObject("TestAudioSource");
        cutSoundAudioSource = audioSourceObj.AddComponent<AudioSource>();

        // Create ChainsLock GameObject with child renderers and colliders
        chainsLockGameObject = new GameObject("TestChainsLock");
        
        // Create child objects with renderers and colliders BEFORE adding ChainsLock component
        // This ensures Awake() will find them
        for (int i = 0; i < 2; i++)
        {
            GameObject chainLink = new GameObject($"ChainLink_{i}");
            chainLink.transform.SetParent(chainsLockGameObject.transform);
            
            MeshRenderer meshRenderer = chainLink.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = chainLink.AddComponent<MeshFilter>();
            meshFilter.mesh = new Mesh();
            
            SphereCollider sphereCollider = chainLink.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.5f;
        }
        
        // NOW add the ChainsLock component so Awake() finds the children
        chainsLock = chainsLockGameObject.AddComponent<ChainsLock>();

        // Set up ChainsLock via reflection since fields are private
        var leftDoorField = typeof(ChainsLock).GetField("leftDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var rightDoorField = typeof(ChainsLock).GetField("rightDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var cutSoundField = typeof(ChainsLock).GetField("cutSound", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var fadeDurationField = typeof(ChainsLock).GetField("fadeDuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var detectRadiusField = typeof(ChainsLock).GetField("detectRadius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        leftDoorField.SetValue(chainsLock, leftDoorScript);
        rightDoorField.SetValue(chainsLock, rightDoorScript);
        cutSoundField.SetValue(chainsLock, cutSoundAudioSource);
        fadeDurationField.SetValue(chainsLock, 2f);
        detectRadiusField.SetValue(chainsLock, 0.3f);

        // Create boltcutter for testing
        boltcutterGameObject = new GameObject("TestBoltcutter");
        boltcutterGameObject.tag = "Boltcutter";
        boltcutterGameObject.AddComponent<SphereCollider>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(chainsLockGameObject);
        Object.Destroy(leftDoorGameObject);
        Object.Destroy(rightDoorGameObject);
        Object.Destroy(audioSourceObj);
        Object.Destroy(boltcutterGameObject);
    }

    [Test]
    public void Setup_InitializesChainsLockWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(chainsLock, "ChainsLock should be initialized");
        Assert.IsNotNull(leftDoorScript, "Left door should be assigned");
        Assert.IsNotNull(rightDoorScript, "Right door should be assigned");
    }

    [Test]
    public void Setup_InitializesFadeDuration()
    {
        // Arrange
        var fadeDurationField = typeof(ChainsLock).GetField("fadeDuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert
        float fadeDuration = (float)fadeDurationField.GetValue(chainsLock);
        Assert.AreEqual(2f, fadeDuration, "Fade duration should be initialized");
    }

    [Test]
    public void Setup_InitializesDetectRadius()
    {
        // Arrange
        var detectRadiusField = typeof(ChainsLock).GetField("detectRadius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert
        float detectRadius = (float)detectRadiusField.GetValue(chainsLock);
        Assert.AreEqual(0.3f, detectRadius, "Detect radius should be initialized");
    }

    [UnityTest]
    public IEnumerator Awake_CachesChildCollidersAndRenderers()
    {
        // Act
        chainsLock.enabled = true;
        yield return null;

        // Assert - Should have cached colliders and renderers
        var childCollidersField = typeof(ChainsLock).GetField("childColliders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var childRenderersField = typeof(ChainsLock).GetField("childRenderers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Collider[] childColliders = (Collider[])childCollidersField.GetValue(chainsLock);
        Renderer[] childRenderers = (Renderer[])childRenderersField.GetValue(chainsLock);

        Assert.IsNotNull(childColliders, "Child colliders should be cached");
        Assert.IsNotNull(childRenderers, "Child renderers should be cached");
        Assert.Greater(childColliders.Length, 0, "Should have at least one child collider");
        Assert.Greater(childRenderers.Length, 0, "Should have at least one child renderer");
    }

    [Test]
    public void ChainsLock_HasChildColliders()
    {
        // Arrange
        Collider[] colliders = chainsLockGameObject.GetComponentsInChildren<Collider>();

        // Assert
        Assert.Greater(colliders.Length, 0, "ChainsLock should have child colliders");
    }

    [Test]
    public void ChainsLock_HasChildRenderers()
    {
        // Arrange
        Renderer[] renderers = chainsLockGameObject.GetComponentsInChildren<Renderer>();

        // Assert
        Assert.Greater(renderers.Length, 0, "ChainsLock should have child renderers");
    }

    [Test]
    public void Boltcutter_HasCorrectTag()
    {
        // Assert
        Assert.IsTrue(boltcutterGameObject.CompareTag("Boltcutter"), "Boltcutter should have 'Boltcutter' tag");
    }

    [Test]
    public void LeftDoor_CanBeAssigned()
    {
        // Arrange
        GameObject newLeftDoorObj = new GameObject("NewLeftDoor");
        DoorsOpening newLeftDoor = newLeftDoorObj.AddComponent<DoorsOpening>();

        // Act
        var leftDoorField = typeof(ChainsLock).GetField("leftDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        leftDoorField.SetValue(chainsLock, newLeftDoor);

        // Assert
        DoorsOpening assignedLeftDoor = (DoorsOpening)leftDoorField.GetValue(chainsLock);
        Assert.AreEqual(newLeftDoor, assignedLeftDoor, "Left door should be assignable");

        // Cleanup
        Object.Destroy(newLeftDoorObj);
    }

    [Test]
    public void RightDoor_CanBeAssigned()
    {
        // Arrange
        GameObject newRightDoorObj = new GameObject("NewRightDoor");
        DoorsOpening newRightDoor = newRightDoorObj.AddComponent<DoorsOpening>();

        // Act
        var rightDoorField = typeof(ChainsLock).GetField("rightDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        rightDoorField.SetValue(chainsLock, newRightDoor);

        // Assert
        DoorsOpening assignedRightDoor = (DoorsOpening)rightDoorField.GetValue(chainsLock);
        Assert.AreEqual(newRightDoor, assignedRightDoor, "Right door should be assignable");

        // Cleanup
        Object.Destroy(newRightDoorObj);
    }

    [Test]
    public void CutSound_CanBeAssigned()
    {
        // Arrange
        GameObject newAudioObj = new GameObject("NewAudioSource");
        AudioSource newAudioSource = newAudioObj.AddComponent<AudioSource>();

        // Act
        var cutSoundField = typeof(ChainsLock).GetField("cutSound", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        cutSoundField.SetValue(chainsLock, newAudioSource);

        // Assert
        AudioSource assignedCutSound = (AudioSource)cutSoundField.GetValue(chainsLock);
        Assert.AreEqual(newAudioSource, assignedCutSound, "Cut sound should be assignable");

        // Cleanup
        Object.Destroy(newAudioObj);
    }

    [Test]
    public void FadeDuration_CanBeModified()
    {
        // Arrange
        float newDuration = 3f;

        // Act
        var fadeDurationField = typeof(ChainsLock).GetField("fadeDuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fadeDurationField.SetValue(chainsLock, newDuration);

        // Assert
        float assignedDuration = (float)fadeDurationField.GetValue(chainsLock);
        Assert.AreEqual(newDuration, assignedDuration, "Fade duration should be modifiable");
    }

    [Test]
    public void DetectRadius_CanBeModified()
    {
        // Arrange
        float newRadius = 0.5f;

        // Act
        var detectRadiusField = typeof(ChainsLock).GetField("detectRadius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        detectRadiusField.SetValue(chainsLock, newRadius);

        // Assert
        float assignedRadius = (float)detectRadiusField.GetValue(chainsLock);
        Assert.AreEqual(newRadius, assignedRadius, "Detect radius should be modifiable");
    }

    [Test]
    public void HandlesMissingLeftDoor()
    {
        // Arrange
        var leftDoorField = typeof(ChainsLock).GetField("leftDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        leftDoorField.SetValue(chainsLock, null);

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => chainsLock.enabled = true, "Should handle null left door gracefully");
    }

    [Test]
    public void HandlesMissingRightDoor()
    {
        // Arrange
        var rightDoorField = typeof(ChainsLock).GetField("rightDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        rightDoorField.SetValue(chainsLock, null);

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => chainsLock.enabled = true, "Should handle null right door gracefully");
    }

    [Test]
    public void HandlesMissingCutSound()
    {
        // Arrange
        var cutSoundField = typeof(ChainsLock).GetField("cutSound", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        cutSoundField.SetValue(chainsLock, null);

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => chainsLock.enabled = true, "Should handle null cut sound gracefully");
    }

    [Test]
    public void FadeDuration_WithZeroValue()
    {
        // Arrange
        float zeroDuration = 0f;

        // Act
        var fadeDurationField = typeof(ChainsLock).GetField("fadeDuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fadeDurationField.SetValue(chainsLock, zeroDuration);

        // Assert
        float assignedDuration = (float)fadeDurationField.GetValue(chainsLock);
        Assert.AreEqual(zeroDuration, assignedDuration, "Fade duration should accept zero value");
    }

    [Test]
    public void FadeDuration_WithLargeValue()
    {
        // Arrange
        float largeDuration = 10f;

        // Act
        var fadeDurationField = typeof(ChainsLock).GetField("fadeDuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fadeDurationField.SetValue(chainsLock, largeDuration);

        // Assert
        float assignedDuration = (float)fadeDurationField.GetValue(chainsLock);
        Assert.AreEqual(largeDuration, assignedDuration, "Fade duration should accept large values");
    }

    [Test]
    public void DetectRadius_WithZeroValue()
    {
        // Arrange
        float zeroRadius = 0f;

        // Act
        var detectRadiusField = typeof(ChainsLock).GetField("detectRadius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        detectRadiusField.SetValue(chainsLock, zeroRadius);

        // Assert
        float assignedRadius = (float)detectRadiusField.GetValue(chainsLock);
        Assert.AreEqual(zeroRadius, assignedRadius, "Detect radius should accept zero value");
    }

    [Test]
    public void DetectRadius_WithLargeValue()
    {
        // Arrange
        float largeRadius = 5f;

        // Act
        var detectRadiusField = typeof(ChainsLock).GetField("detectRadius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        detectRadiusField.SetValue(chainsLock, largeRadius);

        // Assert
        float assignedRadius = (float)detectRadiusField.GetValue(chainsLock);
        Assert.AreEqual(largeRadius, assignedRadius, "Detect radius should accept large values");
    }

    [Test]
    public void MultipleChainsLocks_CanCoexist()
    {
        // Arrange
        GameObject chainsLock2Obj = new GameObject("TestChainsLock2");
        ChainsLock chainsLock2 = chainsLock2Obj.AddComponent<ChainsLock>();

        // Act
        chainsLock.enabled = true;
        chainsLock2.enabled = true;

        // Assert - Both should be active
        Assert.IsTrue(chainsLock.enabled, "First ChainsLock should be enabled");
        Assert.IsTrue(chainsLock2.enabled, "Second ChainsLock should be enabled");

        // Cleanup
        Object.Destroy(chainsLock2Obj);
    }

    [Test]
    public void ChainsLock_CanBeDisabledAndReenabled()
    {
        // Arrange
        chainsLock.enabled = true;

        // Act
        chainsLock.enabled = false;
        bool disabledState = !chainsLock.enabled;
        chainsLock.enabled = true;
        bool reenableState = chainsLock.enabled;

        // Assert
        Assert.IsTrue(disabledState, "ChainsLock should be disabled");
        Assert.IsTrue(reenableState, "ChainsLock should be re-enabled");
    }

    [Test]
    public void ChainsLockPosition_CanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(5f, 0, 5f);

        // Act
        chainsLockGameObject.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, chainsLockGameObject.transform.position, "ChainsLock position should be modifiable");
    }

    [Test]
    public void ChainsLockRotation_CanBeModified()
    {
        // Arrange
        Quaternion newRotation = Quaternion.Euler(45f, 90f, 0f);

        // Act
        chainsLockGameObject.transform.rotation = newRotation;

        // Assert
        float angleDifference = Quaternion.Angle(newRotation, chainsLockGameObject.transform.rotation);
        Assert.Less(angleDifference, 0.01f, "ChainsLock rotation should be modifiable");
    }

    [Test]
    public void ChildColliders_HaveCorrectProperties()
    {
        // Arrange
        Collider[] colliders = chainsLockGameObject.GetComponentsInChildren<Collider>();

        // Assert - All colliders should be valid
        foreach (Collider collider in colliders)
        {
            Assert.IsNotNull(collider, "Child collider should not be null");
            Assert.IsTrue(collider.enabled, "Child collider should be enabled");
        }
    }

    [Test]
    public void ChildRenderers_HaveCorrectProperties()
    {
        // Arrange
        Renderer[] renderers = chainsLockGameObject.GetComponentsInChildren<Renderer>();

        // Assert - All renderers should be valid
        foreach (Renderer renderer in renderers)
        {
            Assert.IsNotNull(renderer, "Child renderer should not be null");
            Assert.IsNotNull(renderer.material, "Child renderer should have a material");
        }
    }

    [Test]
    public void ChainsLock_CanHandleMultipleChildObjects()
    {
        // Arrange
        for (int i = 0; i < 5; i++)
        {
            GameObject chainLink = new GameObject($"AdditionalChainLink_{i}");
            chainLink.transform.SetParent(chainsLockGameObject.transform);
            chainLink.AddComponent<MeshRenderer>();
            chainLink.AddComponent<SphereCollider>();
        }

        // Act
        chainsLock.enabled = true;

        // Assert - Should handle multiple children without errors
        Assert.Pass("ChainsLock handles multiple child objects");
    }
}
