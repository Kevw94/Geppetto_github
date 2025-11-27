using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class DoorLogicTests
{
    private GameObject testDoor;

    [SetUp]
    public void Setup()
    {
        testDoor = new GameObject("TestDoor");
    }

    [TearDown]
    public void Teardown()
    {
        if (testDoor != null)
            Object.Destroy(testDoor);
    }

    [UnityTest]
    public IEnumerator Awake_LockedDoor_MakesRigidBodyKinematic()
    {
        // Arrange
        var rb = testDoor.AddComponent<Rigidbody>();
        var door = testDoor.AddComponent<DoorsOpening>();

        // Wait for Awake to execute
        yield return null;

        // Assert
        Assert.IsTrue(rb.isKinematic, "Locked door should start with kinematic rigidbody.");
    }

    [UnityTest]
    public IEnumerator UnlockDoor_MakesRigidBodyNonKinematic()
    {
        // Arrange
        var rb = testDoor.AddComponent<Rigidbody>();
        var door = testDoor.AddComponent<DoorsOpening>();
        yield return null;

        // Act
        door.UnlockDoor();

        // Assert
        Assert.IsFalse(rb.isKinematic, "UnlockDoor should disable kinematic on rigidbody.");
    }

    [UnityTest]
    public IEnumerator LockDoor_True_MakesRigidBodyKinematic()
    {
        // Arrange
        var rb = testDoor.AddComponent<Rigidbody>();
        var door = testDoor.AddComponent<DoorsOpening>();
        yield return null;

        // Act
        door.UnlockDoor();
        Assert.IsFalse(rb.isKinematic, "Door should be non-kinematic after unlock.");

        door.LockDoor(true);

        // Assert
        Assert.IsTrue(rb.isKinematic, "LockDoor(true) should set rigidbody back to kinematic.");
    }

    [UnityTest]
    public IEnumerator LockDoor_False_MakesRigidBodyNonKinematic()
    {
        // Arrange
        var rb = testDoor.AddComponent<Rigidbody>();
        var door = testDoor.AddComponent<DoorsOpening>();
        yield return null;

        // Act
        door.LockDoor(false);

        // Assert
        Assert.IsFalse(rb.isKinematic, "LockDoor(false) should ensure rigidbody is not kinematic.");
    }

    [UnityTest]
    public IEnumerator DoorsOpening_WithoutRigidbody_LogsWarning()
    {
        // Arrange & Act
        var door = testDoor.AddComponent<DoorsOpening>();
        
        // Wait for Awake
        yield return null;

        // Assert - Vérifie que le warning est bien logué
        LogAssert.Expect(LogType.Warning, "⚠ Aucun Rigidbody trouvé sur la porte !");
    }

    [UnityTest]
    public IEnumerator DoorsOpening_WithoutHingeJoint_LogsWarning()
    {
        // Arrange
        testDoor.AddComponent<Rigidbody>();
        var door = testDoor.AddComponent<DoorsOpening>();
        
        // Act
        yield return null;

        // Assert
        LogAssert.Expect(LogType.Warning, "⚠ Aucun HingeJoint trouvé sur la porte !");
    }
}