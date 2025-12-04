// using NUnit.Framework;
// using UnityEngine;

// public class DoorsOpeningTests
// {
//     private GameObject doorGameObject;
//     private DoorsOpening doorsOpening;
//     private HingeJoint hingeJoint;
//     private Rigidbody rigidbody;

//     [SetUp]
//     public void Setup()
//     {
//         // Create door GameObject
//         doorGameObject = new GameObject("TestDoor");

//         // Add HingeJoint component first
//         hingeJoint = doorGameObject.AddComponent<HingeJoint>();

//         // Add Rigidbody component
//         rigidbody = doorGameObject.AddComponent<Rigidbody>();
//         rigidbody.mass = 1f;
//         rigidbody.useGravity = true;

//         // Add DoorsOpening component last (it has RequireComponent attributes)
//         doorsOpening = doorGameObject.AddComponent<DoorsOpening>();
//     }

//     [TearDown]
//     public void Teardown()
//     {
//         Object.Destroy(doorGameObject);
//     }

//     [Test]
//     public void Setup_InitializesDoorWithValidConfiguration()
//     {
//         // Assert
//         Assert.IsNotNull(doorsOpening, "DoorsOpening should be initialized");
//         Assert.IsNotNull(hingeJoint, "HingeJoint should be assigned");
//         Assert.IsNotNull(rigidbody, "Rigidbody should be assigned");
//     }

//     [Test]
//     public void Setup_InitializesLockedState()
//     {
//         // Arrange
//         var isLockedField = typeof(DoorsOpening).GetField("isLocked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//         // Assert
//         bool isLocked = (bool)isLockedField.GetValue(doorsOpening);
//         Assert.IsTrue(isLocked, "Door should be locked by default");
//     }

//     [Test]
//     public void Awake_CachesHingeJoint()
//     {
//         // Arrange
//         var hingeField = typeof(DoorsOpening).GetField("hinge", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//         // Assert - HingeJoint should be cached (Awake is called when component is added)
//         HingeJoint cachedHinge = (HingeJoint)hingeField.GetValue(doorsOpening);
//         Assert.IsNotNull(cachedHinge, "HingeJoint should be cached");
//         Assert.AreEqual(hingeJoint, cachedHinge, "Cached HingeJoint should match");
//     }

//     [Test]
//     public void Awake_CachesRigidbody()
//     {
//         // Arrange
//         var rbField = typeof(DoorsOpening).GetField("rb", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//         // Assert - Rigidbody should be cached (Awake is called when component is added)
//         Rigidbody cachedRb = (Rigidbody)rbField.GetValue(doorsOpening);
//         Assert.IsNotNull(cachedRb, "Rigidbody should be cached");
//         Assert.AreEqual(rigidbody, cachedRb, "Cached Rigidbody should match");
//     }

//     [Test]
//     public void Awake_SetsRigidbodyKinematicWhenLocked()
//     {
//         // Assert - Rigidbody should be kinematic when locked (Awake is called when component is added)
//         Assert.IsTrue(rigidbody.isKinematic, "Rigidbody should be kinematic when door is locked");
//     }

//     [Test]
//     public void UnlockDoor_UnlocksTheDoor()
//     {
//         // Arrange
//         var isLockedField = typeof(DoorsOpening).GetField("isLocked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//         // Act
//         doorsOpening.UnlockDoor();

//         // Assert - Door should be unlocked
//         bool isLocked = (bool)isLockedField.GetValue(doorsOpening);
//         Assert.IsFalse(isLocked, "Door should be unlocked after UnlockDoor()");
//     }

//     [Test]
//     public void UnlockDoor_SetsRigidbodyNonKinematic()
//     {
//         // Act
//         doorsOpening.UnlockDoor();

//         // Assert - Rigidbody should be non-kinematic when unlocked
//         Assert.IsFalse(rigidbody.isKinematic, "Rigidbody should be non-kinematic when door is unlocked");
//     }

//     [Test]
//     public void LockDoor_WithTrueParameter_LocksDoor()
//     {
//         // Arrange
//         var isLockedField = typeof(DoorsOpening).GetField("isLocked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//         // Act
//         doorsOpening.LockDoor(true);

//         // Assert - Door should be locked
//         bool isLocked = (bool)isLockedField.GetValue(doorsOpening);
//         Assert.IsTrue(isLocked, "Door should be locked when LockDoor(true) is called");
//     }

//     [Test]
//     public void LockDoor_WithFalseParameter_UnlocksDoor()
//     {
//         // Arrange
//         var isLockedField = typeof(DoorsOpening).GetField("isLocked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//         // Act
//         doorsOpening.LockDoor(false);

//         // Assert - Door should be unlocked
//         bool isLocked = (bool)isLockedField.GetValue(doorsOpening);
//         Assert.IsFalse(isLocked, "Door should be unlocked when LockDoor(false) is called");
//     }

//     [Test]
//     public void LockDoor_WithTrue_SetsRigidbodyKinematic()
//     {
//         // Act
//         doorsOpening.LockDoor(true);

//         // Assert - Rigidbody should be kinematic
//         Assert.IsTrue(rigidbody.isKinematic, "Rigidbody should be kinematic when door is locked");
//     }

//     [Test]
//     public void LockDoor_WithFalse_SetsRigidbodyNonKinematic()
//     {
//         // Act
//         doorsOpening.LockDoor(false);

//         // Assert - Rigidbody should be non-kinematic
//         Assert.IsFalse(rigidbody.isKinematic, "Rigidbody should be non-kinematic when door is unlocked");
//     }

//     [Test]
//     public void HandlesMissingHingeJoint()
//     {
//         // Arrange
//         GameObject newDoorObj = new GameObject("DoorWithoutHinge");
//         Rigidbody newRb = newDoorObj.AddComponent<Rigidbody>();
//         DoorsOpening newDoorsOpening = newDoorObj.AddComponent<DoorsOpening>();

//         // Act & Assert - Should not throw exception
//         Assert.DoesNotThrow(() => newDoorsOpening.UnlockDoor(), "Should handle missing HingeJoint gracefully");

//         // Cleanup
//         Object.Destroy(newDoorObj);
//     }

//     [Test]
//     public void HandlesMissingRigidbody()
//     {
//         // Arrange
//         GameObject newDoorObj = new GameObject("DoorWithoutRigidbody");
//         HingeJoint newHinge = newDoorObj.AddComponent<HingeJoint>();
        
//         // Note: DoorsOpening has [RequireComponent(typeof(Rigidbody))], so it will auto-add one
//         DoorsOpening newDoorsOpening = newDoorObj.AddComponent<DoorsOpening>();

//         // Act & Assert - Should not throw exception
//         Assert.DoesNotThrow(() => newDoorsOpening.UnlockDoor(), "Should handle Rigidbody being added automatically");

//         // Cleanup
//         Object.Destroy(newDoorObj);
//     }

//     [Test]
//     public void UnlockDoor_WorksWithAllComponents()
//     {
//         // Arrange
//         GameObject newDoorObj = new GameObject("DoorWithAllComponents");
//         HingeJoint newHinge = newDoorObj.AddComponent<HingeJoint>();
//         Rigidbody newRb = newDoorObj.AddComponent<Rigidbody>();
//         DoorsOpening newDoorsOpening = newDoorObj.AddComponent<DoorsOpening>();

//         // Act & Assert - Should not throw exception
//         Assert.DoesNotThrow(() => newDoorsOpening.UnlockDoor(), "Should handle UnlockDoor with all components");

//         // Cleanup
//         Object.Destroy(newDoorObj);
//     }

//     [Test]
//     public void LockDoor_WorksWithAllComponents()
//     {
//         // Arrange
//         GameObject newDoorObj = new GameObject("DoorWithAllComponents");
//         HingeJoint newHinge = newDoorObj.AddComponent<HingeJoint>();
//         Rigidbody newRb = newDoorObj.AddComponent<Rigidbody>();
//         DoorsOpening newDoorsOpening = newDoorObj.AddComponent<DoorsOpening>();

//         // Act & Assert - Should not throw exception
//         Assert.DoesNotThrow(() => newDoorsOpening.LockDoor(true), "Should handle LockDoor with all components");

//         // Cleanup
//         Object.Destroy(newDoorObj);
//     }

//     [Test]
//     public void MultipleDoors_CanCoexist()
//     {
//         // Arrange
//         GameObject door2Obj = new GameObject("TestDoor2");
//         DoorsOpening door2 = door2Obj.AddComponent<DoorsOpening>();
//         door2Obj.AddComponent<HingeJoint>();
//         door2Obj.AddComponent<Rigidbody>();

//         // Act
//         doorsOpening.enabled = true;
//         door2.enabled = true;

//         // Assert - Both doors should be active
//         Assert.IsTrue(doorsOpening.enabled, "First door should be enabled");
//         Assert.IsTrue(door2.enabled, "Second door should be enabled");

//         // Cleanup
//         Object.Destroy(door2Obj);
//     }

//     [Test]
//     public void Door_CanBeDisabledAndReenabled()
//     {
//         // Arrange
//         doorsOpening.enabled = true;

//         // Act
//         doorsOpening.enabled = false;
//         bool disabledState = !doorsOpening.enabled;
//         doorsOpening.enabled = true;
//         bool reenableState = doorsOpening.enabled;

//         // Assert
//         Assert.IsTrue(disabledState, "Door should be disabled");
//         Assert.IsTrue(reenableState, "Door should be re-enabled");
//     }

//     [Test]
//     public void DoorPosition_CanBeModified()
//     {
//         // Arrange
//         Vector3 newPosition = new Vector3(5f, 0, 5f);

//         // Act
//         doorGameObject.transform.position = newPosition;

//         // Assert
//         Assert.AreEqual(newPosition, doorGameObject.transform.position, "Door position should be modifiable");
//     }

//     [Test]
//     public void DoorRotation_CanBeModified()
//     {
//         // Arrange
//         Quaternion newRotation = Quaternion.Euler(0, 90f, 0);

//         // Act
//         doorGameObject.transform.rotation = newRotation;

//         // Assert
//         float angleDifference = Quaternion.Angle(newRotation, doorGameObject.transform.rotation);
//         Assert.Less(angleDifference, 0.01f, "Door rotation should be modifiable");
//     }

//     [Test]
//     public void HingeJoint_HasCorrectProperties()
//     {
//         // Assert - HingeJoint should be valid
//         Assert.IsNotNull(hingeJoint, "HingeJoint should exist");
//         Assert.IsTrue(hingeJoint.gameObject.activeSelf, "HingeJoint's GameObject should be active");
//     }

//     [Test]
//     public void Rigidbody_HasCorrectProperties()
//     {
//         // Assert - Rigidbody should be valid
//         Assert.IsNotNull(rigidbody, "Rigidbody should exist");
//         Assert.IsTrue(rigidbody.gameObject.activeSelf, "Rigidbody's GameObject should be active");
//         Assert.Greater(rigidbody.mass, 0, "Rigidbody should have positive mass");
//     }

//     [Test]
//     public void UnlockDoor_CanBeCalledMultipleTimes()
//     {
//         // Act - Call UnlockDoor multiple times
//         doorsOpening.UnlockDoor();
//         doorsOpening.UnlockDoor();
//         doorsOpening.UnlockDoor();

//         // Assert - Should not throw exception
//         Assert.IsFalse(rigidbody.isKinematic, "Door should remain unlocked");
//     }

//     [Test]
//     public void LockDoor_CanBeCalledMultipleTimes()
//     {
//         // Act - Call LockDoor multiple times
//         doorsOpening.LockDoor(true);
//         doorsOpening.LockDoor(true);
//         doorsOpening.LockDoor(true);

//         // Assert - Should not throw exception
//         Assert.IsTrue(rigidbody.isKinematic, "Door should remain locked");
//     }

//     [Test]
//     public void UnlockThenLock_WorksCorrectly()
//     {
//         // Act
//         doorsOpening.UnlockDoor();
//         Assert.IsFalse(rigidbody.isKinematic, "Door should be unlocked");

//         doorsOpening.LockDoor(true);
//         Assert.IsTrue(rigidbody.isKinematic, "Door should be locked again");
//     }

//     [Test]
//     public void LockThenUnlock_WorksCorrectly()
//     {
//         // Arrange - First unlock the door
//         doorsOpening.UnlockDoor();
//         Assert.IsFalse(rigidbody.isKinematic, "Door should start unlocked");

//         // Act - Then lock it
//         doorsOpening.LockDoor(true);
//         Assert.IsTrue(rigidbody.isKinematic, "Door should be locked");

//         // Act - Then unlock it again
//         doorsOpening.UnlockDoor();
//         Assert.IsFalse(rigidbody.isKinematic, "Door should be unlocked again");
//     }

//     [Test]
//     public void Rigidbody_MassCanBeModified()
//     {
//         // Arrange
//         float newMass = 5f;

//         // Act
//         rigidbody.mass = newMass;

//         // Assert
//         Assert.AreEqual(newMass, rigidbody.mass, "Rigidbody mass should be modifiable");
//     }

//     [Test]
//     public void Rigidbody_GravityCanBeToggled()
//     {
//         // Arrange
//         bool initialGravity = rigidbody.useGravity;

//         // Act
//         rigidbody.useGravity = !initialGravity;

//         // Assert
//         Assert.AreNotEqual(initialGravity, rigidbody.useGravity, "Rigidbody gravity should be toggleable");
//     }

//     [Test]
//     public void HingeJoint_CanBeConfigured()
//     {
//         // Arrange
//         float newLimit = 90f;

//         // Act
//         JointLimits limits = hingeJoint.limits;
//         limits.max = newLimit;
//         hingeJoint.limits = limits;

//         // Assert
//         Assert.AreEqual(newLimit, hingeJoint.limits.max, "HingeJoint limits should be configurable");
//     }
// }
