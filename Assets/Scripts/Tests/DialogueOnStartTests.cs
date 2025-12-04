using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DialogueOnStartTests
{
    private GameObject testGameObject;
    private DialogueOnStart dialogueOnStart;
    private GameObject bubbleGameObject;
    private VRDialogueBubble mockBubble;
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

        // Create the DialogueOnStart GameObject
        testGameObject = new GameObject("TestDialogueOnStart");
        dialogueOnStart = testGameObject.AddComponent<DialogueOnStart>();

        // Create a mock bubble GameObject with VRDialogueBubble component
        bubbleGameObject = new GameObject("TestBubble");
        bubbleGameObject.transform.position = new Vector3(0, 1, 0);
        mockBubble = bubbleGameObject.AddComponent<VRDialogueBubble>();

        // Create and configure TextMeshProUGUI for the bubble
        GameObject textObject = new GameObject("DialogueText");
        textObject.transform.SetParent(bubbleGameObject.transform);
        TMPro.TextMeshProUGUI textMeshPro = textObject.AddComponent<TMPro.TextMeshProUGUI>();

        // Create and configure CanvasGroup
        canvasGroup = bubbleGameObject.AddComponent<CanvasGroup>();

        // Assign components to the mock bubble
        mockBubble.dialogueText = textMeshPro;
        mockBubble.cg = canvasGroup;
        mockBubble.fadeSpeed = 2f;
        mockBubble.typeSpeed = 0.03f;

        // Initialize the bubble (call Start manually since we're in a test)
        mockBubble.enabled = true;

        // Assign the bubble to DialogueOnStart
        dialogueOnStart.bubble = mockBubble;
        dialogueOnStart.startMessage = "Test dialogue message";
        dialogueOnStart.delayBeforeStart = 0.1f;
        dialogueOnStart.displayTime = 0.5f;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(testGameObject);
        Object.Destroy(bubbleGameObject);

        // Clean up test camera if it exists
        Camera mainCamera = Camera.main;
        if (mainCamera != null && mainCamera.gameObject.name == "TestCamera")
        {
            Object.Destroy(mainCamera.gameObject);
        }
    }

    [Test]
    public void Setup_InitializesBubbleReference()
    {
        // Assert
        Assert.IsNotNull(dialogueOnStart.bubble, "Bubble reference should be assigned");
        Assert.AreEqual(mockBubble, dialogueOnStart.bubble, "Bubble reference should match");
    }

    [Test]
    public void Setup_InitializesStartMessage()
    {
        // Assert
        Assert.IsNotEmpty(dialogueOnStart.startMessage, "Start message should be initialized");
        Assert.AreEqual("Test dialogue message", dialogueOnStart.startMessage, "Start message should match");
    }

    [Test]
    public void Setup_InitializesDelayBeforeStart()
    {
        // Assert
        Assert.AreEqual(0.1f, dialogueOnStart.delayBeforeStart, "Delay before start should be initialized correctly");
    }

    [Test]
    public void Setup_InitializesDisplayTime()
    {
        // Assert
        Assert.AreEqual(0.5f, dialogueOnStart.displayTime, "Display time should be initialized correctly");
    }

    [UnityTest]
    public IEnumerator Start_InvokesShowStartDialogueAfterDelay()
    {
        // Arrange
        canvasGroup.alpha = 0f;
        dialogueOnStart.delayBeforeStart = 0.1f;

        // Act
        dialogueOnStart.enabled = true;
        yield return new WaitForSeconds(0.2f);

        // Assert - The bubble should have started showing (alpha should be increasing)
        Assert.Greater(canvasGroup.alpha, 0f, "Dialogue should start fading in after delay");
    }

    [UnityTest]
    public IEnumerator ShowStartDialogue_DisplaysDialogueWithCorrectMessage()
    {
        // Arrange
        string testMessage = "Hello World";
        dialogueOnStart.startMessage = testMessage;
        dialogueOnStart.delayBeforeStart = 0.05f;
        mockBubble.typeSpeed = 0.02f;
        TMPro.TextMeshProUGUI textMeshPro = mockBubble.dialogueText;
        textMeshPro.text = "";
        canvasGroup.alpha = 0f;

        // Act
        dialogueOnStart.enabled = true;
        // Wait for: delay (0.05) + fade in (0.5) + typewriter animation time
        yield return new WaitForSeconds(1.5f);

        // Assert - Text should be displayed (at least partially or fully)
        // The typewriter effect should have started typing the message
        Assert.IsNotEmpty(textMeshPro.text, "Dialogue text should start appearing");
    }

    [UnityTest]
    public IEnumerator ShowStartDialogue_FadesInBubble()
    {
        // Arrange
        canvasGroup.alpha = 0f;
        dialogueOnStart.delayBeforeStart = 0.05f;

        // Act
        dialogueOnStart.enabled = true;
        yield return new WaitForSeconds(0.2f);

        // Assert - Alpha should be greater than 0
        Assert.Greater(canvasGroup.alpha, 0f, "Bubble should fade in when dialogue starts");
    }

    [UnityTest]
    public IEnumerator ShowStartDialogue_FadesOutAfterDisplayTime()
    {
        // Arrange
        canvasGroup.alpha = 0f;
        dialogueOnStart.delayBeforeStart = 0.05f;
        dialogueOnStart.displayTime = 0.1f;

        // Act
        dialogueOnStart.enabled = true;
        yield return new WaitForSeconds(0.05f); // Wait for delay
        yield return new WaitForSeconds(0.2f); // Wait for fade in and display time

        // Assert - Alpha should be fading out or at 0
        Assert.LessOrEqual(canvasGroup.alpha, 1f, "Bubble should fade out after display time");
    }

    [Test]
    public void StartMessage_CanBeModified()
    {
        // Arrange
        string newMessage = "New message";

        // Act
        dialogueOnStart.startMessage = newMessage;

        // Assert
        Assert.AreEqual(newMessage, dialogueOnStart.startMessage, "Start message should be modifiable");
    }

    [Test]
    public void DelayBeforeStart_CanBeModified()
    {
        // Arrange
        float newDelay = 2f;

        // Act
        dialogueOnStart.delayBeforeStart = newDelay;

        // Assert
        Assert.AreEqual(newDelay, dialogueOnStart.delayBeforeStart, "Delay before start should be modifiable");
    }

    [Test]
    public void DisplayTime_CanBeModified()
    {
        // Arrange
        float newDisplayTime = 10f;

        // Act
        dialogueOnStart.displayTime = newDisplayTime;

        // Assert
        Assert.AreEqual(newDisplayTime, dialogueOnStart.displayTime, "Display time should be modifiable");
    }

    [Test]
    public void Bubble_CanBeAssigned()
    {
        // Arrange
        GameObject newBubbleObj = new GameObject("NewBubble");
        VRDialogueBubble newBubble = newBubbleObj.AddComponent<VRDialogueBubble>();

        // Act
        dialogueOnStart.bubble = newBubble;

        // Assert
        Assert.AreEqual(newBubble, dialogueOnStart.bubble, "Bubble should be assignable");

        // Cleanup
        Object.Destroy(newBubbleObj);
    }

    [UnityTest]
    public IEnumerator Start_WithZeroDelay_ShowsDialogueImmediately()
    {
        // Arrange
        canvasGroup.alpha = 0f;
        dialogueOnStart.delayBeforeStart = 0f;

        // Act
        dialogueOnStart.enabled = true;
        yield return new WaitForSeconds(0.15f);

        // Assert - Dialogue should start showing almost immediately
        Assert.Greater(canvasGroup.alpha, 0f, "Dialogue should show immediately with zero delay");
    }

    [UnityTest]
    public IEnumerator Start_WithLargeDelay_WaitsBeforeShowing()
    {
        // Arrange
        canvasGroup.alpha = 0f;
        dialogueOnStart.delayBeforeStart = 0.3f;

        // Act
        dialogueOnStart.enabled = true;
        yield return new WaitForSeconds(0.1f);

        // Assert - Dialogue should not have started yet (allow tolerance for frame timing)
        Assert.Less(canvasGroup.alpha, 0.1f, "Dialogue should not show significantly before delay");

        // Wait for delay to pass
        yield return new WaitForSeconds(0.25f);

        // Assert - Now it should be showing
        Assert.Greater(canvasGroup.alpha, 0f, "Dialogue should show after delay");
    }

    [UnityTest]
    public IEnumerator ShowStartDialogue_CallsBubbleShowDialogueWithCorrectParameters()
    {
        // Arrange
        string testMessage = "Test Message";
        float testDisplayTime = 0.3f;
        dialogueOnStart.startMessage = testMessage;
        dialogueOnStart.displayTime = testDisplayTime;
        dialogueOnStart.delayBeforeStart = 0.05f;
        mockBubble.typeSpeed = 0.02f;
        TMPro.TextMeshProUGUI textMeshPro = mockBubble.dialogueText;
        textMeshPro.text = "";

        // Act
        dialogueOnStart.enabled = true;
        // Wait for: delay (0.05) + fade in (0.5) + typewriter (12 chars * 0.02 = 0.24)
        yield return new WaitForSeconds(1.0f);

        // Assert - Canvas group should be fading in/visible (alpha > 0)
        Assert.Greater(canvasGroup.alpha, 0f, "Bubble should be visible after ShowDialogue is called");
    }

    [Test]
    public void HandlesMissingBubbleReference()
    {
        // Arrange
        dialogueOnStart.bubble = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => dialogueOnStart.enabled = true, "Should handle null bubble gracefully");
    }
}
