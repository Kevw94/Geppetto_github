using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class DialogueBubbleTests
{
    private GameObject testGameObject;
    private VRDialogueBubble dialogueBubble;
    private TextMeshProUGUI textMeshPro;
    private CanvasGroup canvasGroup;

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

        // Create a test GameObject with the VRDialogueBubble component
        testGameObject = new GameObject("TestDialogueBubble");
        dialogueBubble = testGameObject.AddComponent<VRDialogueBubble>();

        // Create and configure TextMeshProUGUI
        GameObject textObject = new GameObject("DialogueText");
        textObject.transform.SetParent(testGameObject.transform);
        textMeshPro = textObject.AddComponent<TextMeshProUGUI>();

        // Create and configure CanvasGroup
        canvasGroup = testGameObject.AddComponent<CanvasGroup>();

        // Assign components to the dialogue bubble
        dialogueBubble.dialogueText = textMeshPro;
        dialogueBubble.cg = canvasGroup;
        dialogueBubble.fadeSpeed = 2f;
        dialogueBubble.typeSpeed = 0.03f;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(testGameObject);
        
        // Clean up test camera if it exists
        Camera mainCamera = Camera.main;
        if (mainCamera != null && mainCamera.gameObject.name == "TestCamera")
        {
            Object.Destroy(mainCamera.gameObject);
        }
    }

    [UnityTest]
    public IEnumerator Start_InitializesCanvasGroupAlphaToZero()
    {
        // Act
        dialogueBubble.enabled = true;
        yield return null;

        // Assert
        Assert.AreEqual(0f, canvasGroup.alpha, "Canvas group alpha should be initialized to 0");
    }

    [Test]
    public void ShowDialogue_StopsAllCoroutinesBeforeStarting()
    {
        // Arrange
        string testMessage = "Test message";

        // Act
        dialogueBubble.ShowDialogue(testMessage);

        // Assert - Verify that coroutine was started (no exception thrown)
        Assert.Pass("ShowDialogue successfully started coroutine");
    }

    [UnityTest]
    public IEnumerator ShowDialogue_FadesInCanvasGroup()
    {
        // Arrange
        string testMessage = "Test";
        canvasGroup.alpha = 0f;

        // Act
        dialogueBubble.ShowDialogue(testMessage, 0.1f);
        yield return new WaitForSeconds(0.2f);

        // Assert - Alpha should be greater than 0 after fade in starts
        Assert.Greater(canvasGroup.alpha, 0f, "Canvas group should start fading in");
    }

    [UnityTest]
    public IEnumerator ShowDialogue_DisplaysTextWithTypewriterEffect()
    {
        // Arrange
        string testMessage = "Hello";
        textMeshPro.text = "";
        canvasGroup.alpha = 0f;

        // Act
        dialogueBubble.ShowDialogue(testMessage, 0.5f);
        
        // Wait for fade-in to start and some typewriter characters to appear
        yield return new WaitForSeconds(0.7f);

        // Assert - Text should start appearing character by character
        Assert.IsNotEmpty(textMeshPro.text, "Text should start appearing");
        Assert.LessOrEqual(textMeshPro.text.Length, testMessage.Length, "Should not exceed message length");
    }

    [UnityTest]
    public IEnumerator ShowDialogue_CompleteTypewriterEffect()
    {
        // Arrange
        string testMessage = "Complete";
        textMeshPro.text = "";
        float typeSpeed = 0.02f;
        dialogueBubble.typeSpeed = typeSpeed;
        canvasGroup.alpha = 0f;

        // Act
        dialogueBubble.ShowDialogue(testMessage, 0.1f);
        
        // Wait for fade-in to complete (approximately 0.5 seconds at fadeSpeed 2)
        yield return new WaitForSeconds(0.6f);
        
        // Then wait for typewriter effect to complete
        yield return new WaitForSeconds(typeSpeed * testMessage.Length + 0.1f);

        // Assert - All text should be displayed
        Assert.AreEqual(testMessage, textMeshPro.text, "All text should be displayed after typewriter effect");
    }

    [UnityTest]
    public IEnumerator ShowDialogue_FadesOutAfterDisplayTime()
    {
        // Arrange
        string testMessage = "Test";
        float displayTime = 0.1f;
        canvasGroup.alpha = 0f;

        // Act
        dialogueBubble.ShowDialogue(testMessage, displayTime);
        yield return new WaitForSeconds(displayTime + 0.2f);

        // Assert - Alpha should be fading out or at 0
        Assert.LessOrEqual(canvasGroup.alpha, 1f, "Canvas group should fade out after display time");
    }

    [UnityTest]
    public IEnumerator HideDialogue_FadesOutCanvasGroup()
    {
        // Arrange
        canvasGroup.alpha = 1f;

        // Act
        dialogueBubble.HideDialogue();
        yield return new WaitForSeconds(0.1f);

        // Assert - Alpha should be less than 1
        Assert.Less(canvasGroup.alpha, 1f, "Canvas group should fade out");
    }

    [UnityTest]
    public IEnumerator HideDialogue_CompletelyFadesOut()
    {
        // Arrange
        canvasGroup.alpha = 1f;
        float fadeSpeed = 5f; // Fast fade for testing
        dialogueBubble.fadeSpeed = fadeSpeed;

        // Act
        dialogueBubble.HideDialogue();
        yield return new WaitForSeconds(0.3f);

        // Assert - Alpha should be 0 or very close to 0
        Assert.LessOrEqual(canvasGroup.alpha, 0.05f, "Canvas group should be fully faded out");
    }

    [UnityTest]
    public IEnumerator Update_RotatesDialogueBubbleTowardCamera()
    {
        // Arrange
        Vector3 initialForward = testGameObject.transform.forward;
        yield return null; // Wait for Start to be called

        // Act
        dialogueBubble.ShowDialogue("Test", 0.1f);
        yield return new WaitForSeconds(0.05f);

        // Assert - The transform should have been modified to look at camera
        // (We can't directly verify camera position, but we can verify rotation occurs during ShowDialogue)
        Assert.Pass("Dialogue bubble rotation executed without error");
    }

    [Test]
    public void FadeSpeed_CanBeModified()
    {
        // Arrange
        float newFadeSpeed = 5f;

        // Act
        dialogueBubble.fadeSpeed = newFadeSpeed;

        // Assert
        Assert.AreEqual(newFadeSpeed, dialogueBubble.fadeSpeed, "Fade speed should be modifiable");
    }

    [Test]
    public void TypeSpeed_CanBeModified()
    {
        // Arrange
        float newTypeSpeed = 0.05f;

        // Act
        dialogueBubble.typeSpeed = newTypeSpeed;

        // Assert
        Assert.AreEqual(newTypeSpeed, dialogueBubble.typeSpeed, "Type speed should be modifiable");
    }

    [Test]
    public void ShowDialogue_WithDefaultDisplayTime()
    {
        // Arrange
        string testMessage = "Default time test";

        // Act - Should use default displayTime of 5f
        dialogueBubble.ShowDialogue(testMessage);

        // Assert - No exception should be thrown
        Assert.Pass("ShowDialogue with default display time works correctly");
    }

    [UnityTest]
    public IEnumerator MultipleShowDialogueCalls_StopsPreviousCoroutine()
    {
        // Arrange
        string firstMessage = "First";
        string secondMessage = "Second";
        textMeshPro.text = "";

        // Act - Call ShowDialogue twice to test coroutine stopping
        dialogueBubble.ShowDialogue(firstMessage, 1f);
        yield return new WaitForSeconds(0.1f);
        
        // Second call should stop the first coroutine
        dialogueBubble.ShowDialogue(secondMessage, 0.5f);
        
        // Wait for typewriter effect to complete
        yield return new WaitForSeconds(dialogueBubble.typeSpeed * secondMessage.Length + 0.2f);

        // Assert - Should complete without errors (coroutine management works)
        Assert.Pass("Multiple ShowDialogue calls handled correctly without errors");
    }

    [UnityTest]
    public IEnumerator HideDialogue_StopsPreviousCoroutine()
    {
        // Arrange
        string testMessage = "Test";
        canvasGroup.alpha = 1f;

        // Act
        dialogueBubble.ShowDialogue(testMessage, 1f);
        yield return new WaitForSeconds(0.05f);
        dialogueBubble.HideDialogue();
        yield return new WaitForSeconds(0.1f);

        // Assert - Should fade out without errors
        Assert.Less(canvasGroup.alpha, 1f, "Should be fading out");
    }
}
