using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DialogueFollowTests
{
    private GameObject testGameObject;
    private DialogueFollow dialogueFollow;
    private GameObject cameraObject;
    private Transform playerCamera;

    [SetUp]
    public void Setup()
    {
        // Create a test camera
        cameraObject = new GameObject("TestPlayerCamera");
        playerCamera = cameraObject.transform;
        playerCamera.position = Vector3.zero;

        // Create the dialogue follow GameObject
        testGameObject = new GameObject("TestDialogueFollow");
        dialogueFollow = testGameObject.AddComponent<DialogueFollow>();

        // Assign the camera
        dialogueFollow.playerCamera = playerCamera;
        dialogueFollow.offset = new Vector3(0, 0.1f, 1.5f);
        dialogueFollow.followSpeed = 5f;

        // Set initial position
        testGameObject.transform.position = Vector3.zero;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(testGameObject);
        Object.Destroy(cameraObject);
    }

    [Test]
    public void Setup_InitializesWithValidCamera()
    {
        // Assert
        Assert.IsNotNull(dialogueFollow.playerCamera, "Player camera should be assigned");
        Assert.AreEqual(playerCamera, dialogueFollow.playerCamera, "Camera reference should match");
    }

    [Test]
    public void Setup_InitializesOffsetCorrectly()
    {
        // Assert
        Assert.AreEqual(new Vector3(0, 0.1f, 1.5f), dialogueFollow.offset, "Offset should be initialized correctly");
    }

    [Test]
    public void Setup_InitializesFollowSpeedCorrectly()
    {
        // Assert
        Assert.AreEqual(5f, dialogueFollow.followSpeed, "Follow speed should be initialized correctly");
    }

    [UnityTest]
    public IEnumerator FollowsPlayerCameraPosition()
    {
        // Arrange
        Vector3 newCameraPos = new Vector3(5f, 2f, 3f);
        playerCamera.position = newCameraPos;
        testGameObject.transform.position = Vector3.zero;

        // Act - Enable the component and wait for LateUpdate to be called by Unity
        dialogueFollow.enabled = true;
        yield return new WaitForSeconds(0.2f);

        // Assert - Dialogue should have moved towards the target position
        Vector3 expectedTargetPos = newCameraPos + playerCamera.TransformDirection(dialogueFollow.offset);
        float distanceToTarget = Vector3.Distance(testGameObject.transform.position, expectedTargetPos);

        Assert.Less(distanceToTarget, 3f, "Dialogue should move towards camera target position");
    }

    [UnityTest]
    public IEnumerator PositionsDialogueWithOffset()
    {
        // Arrange
        playerCamera.position = Vector3.zero;
        playerCamera.rotation = Quaternion.identity;
        testGameObject.transform.position = Vector3.zero;
        dialogueFollow.enabled = true;

        // Act - Wait for LateUpdate to be called
        yield return new WaitForSeconds(0.2f);

        // Assert - Position should be approximately offset from camera
        Vector3 expectedPos = playerCamera.position + playerCamera.TransformDirection(dialogueFollow.offset);
        float distance = Vector3.Distance(testGameObject.transform.position, expectedPos);

        Assert.Less(distance, 1f, "Dialogue should be positioned at camera + offset");
    }

    [UnityTest]
    public IEnumerator RotatesDialogueTowardCamera()
    {
        // Arrange
        playerCamera.position = Vector3.zero;
        playerCamera.rotation = Quaternion.identity;
        testGameObject.transform.position = new Vector3(0, 0, 1.5f);
        testGameObject.transform.rotation = Quaternion.identity;
        dialogueFollow.enabled = true;

        // Act - Wait for LateUpdate to be called
        yield return new WaitForSeconds(0.2f);

        // Assert - Should be looking at camera (rotation should have changed)
        Vector3 lookDir = testGameObject.transform.position - playerCamera.position;
        lookDir.y = 0;
        Quaternion expectedRotation = Quaternion.LookRotation(lookDir);

        float angleDifference = Quaternion.Angle(testGameObject.transform.rotation, expectedRotation);
        Assert.Less(angleDifference, 15f, "Dialogue should be rotated towards camera");
    }

    [UnityTest]
    public IEnumerator HandlesNullCamera()
    {
        // Arrange
        dialogueFollow.playerCamera = null;
        Vector3 initialPos = testGameObject.transform.position;
        dialogueFollow.enabled = true;

        // Act - Should not throw exception
        yield return new WaitForSeconds(0.1f);

        // Assert - Position should remain unchanged
        Assert.AreEqual(initialPos, testGameObject.transform.position, "Position should not change with null camera");
    }

    [UnityTest]
    public IEnumerator SmoothFollowsWithLerp()
    {
        // Arrange
        playerCamera.position = Vector3.zero;
        testGameObject.transform.position = Vector3.zero;

        Vector3 newCameraPos = new Vector3(10f, 0, 0);
        playerCamera.position = newCameraPos;
        dialogueFollow.enabled = true;

        // Act - Wait a short time
        yield return new WaitForSeconds(0.05f);
        Vector3 posAfterShortWait = testGameObject.transform.position;

        // Assert - Should move but not reach target immediately (due to lerp)
        Vector3 targetPos = newCameraPos + playerCamera.TransformDirection(dialogueFollow.offset);
        float distanceAfterShortWait = Vector3.Distance(posAfterShortWait, targetPos);

        Assert.Greater(distanceAfterShortWait, 0.1f, "Should not reach target immediately due to lerp");
        Assert.Less(Vector3.Distance(posAfterShortWait, Vector3.zero), 10f, "Should move towards target");
    }

    [UnityTest]
    public IEnumerator MaintainsYAxisAlignment()
    {
        // Arrange
        playerCamera.position = new Vector3(5f, 2f, 5f);
        playerCamera.rotation = Quaternion.Euler(45f, 45f, 0f);
        testGameObject.transform.position = Vector3.zero;
        dialogueFollow.enabled = true;

        // Act - Wait for LateUpdate to be called
        yield return new WaitForSeconds(0.2f);

        // Assert - Y rotation should keep dialogue upright (lookDir.y = 0)
        Vector3 forward = testGameObject.transform.forward;
        Assert.That(Mathf.Abs(forward.y), Is.LessThan(0.2f), "Dialogue should remain upright (Y-axis aligned)");
    }

    [Test]
    public void OffsetCanBeModified()
    {
        // Arrange
        Vector3 newOffset = new Vector3(1f, 0.5f, 2f);

        // Act
        dialogueFollow.offset = newOffset;

        // Assert
        Assert.AreEqual(newOffset, dialogueFollow.offset, "Offset should be modifiable");
    }

    [Test]
    public void FollowSpeedCanBeModified()
    {
        // Arrange
        float newSpeed = 10f;

        // Act
        dialogueFollow.followSpeed = newSpeed;

        // Assert
        Assert.AreEqual(newSpeed, dialogueFollow.followSpeed, "Follow speed should be modifiable");
    }

    [Test]
    public void PlayerCameraCanBeAssigned()
    {
        // Arrange
        GameObject newCameraObj = new GameObject("NewCamera");
        Transform newCamera = newCameraObj.transform;

        // Act
        dialogueFollow.playerCamera = newCamera;

        // Assert
        Assert.AreEqual(newCamera, dialogueFollow.playerCamera, "Player camera should be assignable");

        // Cleanup
        Object.Destroy(newCameraObj);
    }

    [UnityTest]
    public IEnumerator FollowsRotatedCamera()
    {
        // Arrange
        playerCamera.position = Vector3.zero;
        playerCamera.rotation = Quaternion.Euler(0, 90f, 0); // Rotated 90 degrees
        testGameObject.transform.position = Vector3.zero;
        dialogueFollow.enabled = true;

        // Act - Wait for LateUpdate to be called
        yield return new WaitForSeconds(0.2f);

        // Assert - Should position relative to rotated camera
        Vector3 expectedPos = playerCamera.position + playerCamera.TransformDirection(dialogueFollow.offset);
        float distance = Vector3.Distance(testGameObject.transform.position, expectedPos);

        Assert.Less(distance, 1.5f, "Should follow rotated camera correctly");
    }

    [UnityTest]
    public IEnumerator ConvergesOnTarget()
    {
        // Arrange
        playerCamera.position = new Vector3(5f, 1f, 5f);
        playerCamera.rotation = Quaternion.identity;
        testGameObject.transform.position = Vector3.zero;
        dialogueFollow.enabled = true;

        // Act - Wait longer to allow convergence
        yield return new WaitForSeconds(0.5f);

        // Assert - Should converge close to target
        Vector3 targetPos = playerCamera.position + playerCamera.TransformDirection(dialogueFollow.offset);
        float distance = Vector3.Distance(testGameObject.transform.position, targetPos);

        Assert.Less(distance, 1f, "Should converge towards target position");
    }
}
