using NUnit.Framework;
using UnityEngine;

public class DoorLogicTests
{
    [Test]
    public void Awake_LockedDoor_MakesRigidBodyKinematic()
    {
        var go = new GameObject("Door");
        var rb = go.AddComponent<Rigidbody>();
        var door = go.AddComponent<DoorsOpening>();

        Assert.IsTrue(rb.isKinematic, "Locked door should start with kinematic rigidbody.");
    }

    [Test]
    public void UnlockDoor_MakesRigidBodyNonKinematic()
    {
        var go = new GameObject("Door");
        var rb = go.AddComponent<Rigidbody>();
        var door = go.AddComponent<DoorsOpening>();

        door.UnlockDoor();

        Assert.IsFalse(rb.isKinematic, "UnlockDoor should disable kinematic on rigidbody.");
    }

    [Test]
    public void LockDoor_True_MakesRigidBodyKinematic()
    {
        var go = new GameObject("Door");
        var rb = go.AddComponent<Rigidbody>();
        var door = go.AddComponent<DoorsOpening>();

        door.UnlockDoor();
        Assert.IsFalse(rb.isKinematic, "Door should be non-kinematic after unlock.");

        door.LockDoor(true);

        Assert.IsTrue(rb.isKinematic, "LockDoor(true) should set rigidbody back to kinematic.");
    }

    [Test]
    public void LockDoor_False_MakesRigidBodyNonKinematic()
    {
        var go = new GameObject("Door");
        var rb = go.AddComponent<Rigidbody>();
        var door = go.AddComponent<DoorsOpening>();

        door.LockDoor(false);

        Assert.IsFalse(rb.isKinematic, "LockDoor(false) should ensure rigidbody is not kinematic.");
    }
}
