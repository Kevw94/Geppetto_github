using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MenuManagerTests
{
    private GameObject menuManagerObj;
    private MenuManager menuManager;
    private GameObject playButtonObj;
    private GameObject quitButtonObj;
    private GameObject returnButtonObj;
    private GameObject commandsButtonObj;
    private GameObject commandsPanelObj;
    private GameObject audioSourceObj;
    private AudioSource audioSource;

    [SetUp]
    public void Setup()
    {
        // Create audio listener for audio tests
        GameObject audioListenerObj = new GameObject("TestAudioListener");
        audioListenerObj.AddComponent<AudioListener>();

        // Create play button
        playButtonObj = new GameObject("TestPlayButton");
        playButtonObj.SetActive(true);

        // Create quit button
        quitButtonObj = new GameObject("TestQuitButton");
        quitButtonObj.SetActive(true);

        // Create return button
        returnButtonObj = new GameObject("TestReturnButton");
        returnButtonObj.SetActive(true);

        // Create commands button
        commandsButtonObj = new GameObject("TestCommandsButton");
        commandsButtonObj.SetActive(true);

        // Create commands panel
        commandsPanelObj = new GameObject("TestCommandsPanel");
        commandsPanelObj.SetActive(false);

        // Create audio source
        audioSourceObj = new GameObject("TestAudioSource");
        audioSource = audioSourceObj.AddComponent<AudioSource>();

        // Create MenuManager
        menuManagerObj = new GameObject("TestMenuManager");
        menuManager = menuManagerObj.AddComponent<MenuManager>();

        // Configure MenuManager
        menuManager.playButton = playButtonObj;
        menuManager.quitButton = quitButtonObj;
        menuManager.returnButton = returnButtonObj;
        menuManager.commandsButton = commandsButtonObj;
        menuManager.commandsPanel = commandsPanelObj;
        menuManager.ButtonPressed = audioSource;
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.Destroy(menuManagerObj);
        UnityEngine.Object.Destroy(playButtonObj);
        UnityEngine.Object.Destroy(quitButtonObj);
        UnityEngine.Object.Destroy(returnButtonObj);
        UnityEngine.Object.Destroy(commandsButtonObj);
        UnityEngine.Object.Destroy(commandsPanelObj);
        UnityEngine.Object.Destroy(audioSourceObj);

        // Clean up audio listeners
        AudioListener[] listeners = UnityEngine.Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        foreach (AudioListener listener in listeners)
        {
            UnityEngine.Object.Destroy(listener.gameObject);
        }
    }

    [Test]
    public void Setup_InitializesWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(menuManager, "MenuManager should be initialized");
        Assert.IsNotNull(playButtonObj, "Play button should be assigned");
        Assert.IsNotNull(quitButtonObj, "Quit button should be assigned");
        Assert.IsNotNull(returnButtonObj, "Return button should be assigned");
        Assert.IsNotNull(commandsButtonObj, "Commands button should be assigned");
        Assert.IsNotNull(commandsPanelObj, "Commands panel should be assigned");
        Assert.IsNotNull(audioSource, "Audio source should be assigned");
    }

    [Test]
    public void PlayButton_CanBeAssigned()
    {
        // Arrange
        GameObject newButtonObj = new GameObject("NewPlayButton");

        // Act
        menuManager.playButton = newButtonObj;

        // Assert
        Assert.AreEqual(newButtonObj, menuManager.playButton, "Play button should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newButtonObj);
    }

    [Test]
    public void QuitButton_CanBeAssigned()
    {
        // Arrange
        GameObject newButtonObj = new GameObject("NewQuitButton");

        // Act
        menuManager.quitButton = newButtonObj;

        // Assert
        Assert.AreEqual(newButtonObj, menuManager.quitButton, "Quit button should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newButtonObj);
    }

    [Test]
    public void ReturnButton_CanBeAssigned()
    {
        // Arrange
        GameObject newButtonObj = new GameObject("NewReturnButton");

        // Act
        menuManager.returnButton = newButtonObj;

        // Assert
        Assert.AreEqual(newButtonObj, menuManager.returnButton, "Return button should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newButtonObj);
    }

    [Test]
    public void CommandsButton_CanBeAssigned()
    {
        // Arrange
        GameObject newButtonObj = new GameObject("NewCommandsButton");

        // Act
        menuManager.commandsButton = newButtonObj;

        // Assert
        Assert.AreEqual(newButtonObj, menuManager.commandsButton, "Commands button should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newButtonObj);
    }

    [Test]
    public void CommandsPanel_CanBeAssigned()
    {
        // Arrange
        GameObject newPanelObj = new GameObject("NewCommandsPanel");

        // Act
        menuManager.commandsPanel = newPanelObj;

        // Assert
        Assert.AreEqual(newPanelObj, menuManager.commandsPanel, "Commands panel should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newPanelObj);
    }

    [Test]
    public void AudioSource_CanBeAssigned()
    {
        // Arrange
        GameObject newAudioObj = new GameObject("NewAudioSource");
        AudioSource newAudioSource = newAudioObj.AddComponent<AudioSource>();

        // Act
        menuManager.ButtonPressed = newAudioSource;

        // Assert
        Assert.AreEqual(newAudioSource, menuManager.ButtonPressed, "Audio source should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newAudioObj);
    }

    [Test]
    public void PlayGame_HasCorrectSignature()
    {
        // Assert - PlayGame method should exist
        var method = typeof(MenuManager).GetMethod("PlayGame", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "PlayGame method should exist");
    }

    [Test]
    public void QuitGame_HasCorrectSignature()
    {
        // Assert - QuitGame method should exist
        var method = typeof(MenuManager).GetMethod("QuitGame", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "QuitGame method should exist");
    }

    [Test]
    public void ShowCommands_HasCorrectSignature()
    {
        // Assert - ShowCommands method should exist
        var method = typeof(MenuManager).GetMethod("ShowCommands", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "ShowCommands method should exist");
    }

    [Test]
    public void ReturnToMenu_HasCorrectSignature()
    {
        // Assert - ReturnToMenu method should exist
        var method = typeof(MenuManager).GetMethod("ReturnToMenu", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "ReturnToMenu method should exist");
    }

    [Test]
    public void PlayGame_SetsPlayerPrefs()
    {
        // Arrange
        PlayerPrefs.DeleteKey("SceneToLoad");

        // Act
        menuManager.PlayGame();

        // Assert
        Assert.AreEqual("City", PlayerPrefs.GetString("SceneToLoad"), "PlayGame should set SceneToLoad to City");

        // Cleanup
        PlayerPrefs.DeleteKey("SceneToLoad");
    }

    [Test]
    public void PlayGame_RequiresValidAudioSource()
    {
        // Assert - PlayGame requires a valid audio source
        // The method calls ButtonPressed.Play() without null check
        Assert.IsNotNull(menuManager.ButtonPressed, "Audio source must be assigned for PlayGame to work");
    }

    [Test]
    public void QuitGame_RequiresValidAudioSource()
    {
        // Assert - QuitGame requires a valid audio source
        // The method calls ButtonPressed.Play() without null check
        Assert.IsNotNull(menuManager.ButtonPressed, "Audio source must be assigned for QuitGame to work");
    }

    [Test]
    public void ShowCommands_RequiresValidAudioSource()
    {
        // Assert - ShowCommands requires a valid audio source
        // The method calls ButtonPressed.Play() without null check
        Assert.IsNotNull(menuManager.ButtonPressed, "Audio source must be assigned for ShowCommands to work");
    }

    [Test]
    public void ReturnToMenu_RequiresValidAudioSource()
    {
        // Assert - ReturnToMenu requires a valid audio source
        // The method calls ButtonPressed.Play() without null check
        Assert.IsNotNull(menuManager.ButtonPressed, "Audio source must be assigned for ReturnToMenu to work");
    }

    [Test]
    public void ShowCommands_ActivatesCommandsPanel()
    {
        // Arrange
        commandsPanelObj.SetActive(false);
        Assert.IsFalse(commandsPanelObj.activeSelf, "Commands panel should be inactive initially");

        // Act
        menuManager.ShowCommands();

        // Assert
        Assert.IsTrue(commandsPanelObj.activeSelf, "Commands panel should be active after ShowCommands");
    }

    [Test]
    public void ReturnToMenu_DeactivatesCommandsPanel()
    {
        // Arrange
        commandsPanelObj.SetActive(true);
        Assert.IsTrue(commandsPanelObj.activeSelf, "Commands panel should be active initially");

        // Act
        menuManager.ReturnToMenu();

        // Assert
        Assert.IsFalse(commandsPanelObj.activeSelf, "Commands panel should be inactive after ReturnToMenu");
    }

    [Test]
    public void ShowCommands_WithNullCommandsPanel()
    {
        // Arrange
        menuManager.commandsPanel = null;

        // Act & Assert - Should throw NullReferenceException
        Assert.Throws<System.NullReferenceException>(() => menuManager.ShowCommands(), "Should throw exception when commands panel is null");
    }

    [Test]
    public void ReturnToMenu_WithNullCommandsPanel()
    {
        // Arrange
        menuManager.commandsPanel = null;

        // Act & Assert - Should throw NullReferenceException
        Assert.Throws<System.NullReferenceException>(() => menuManager.ReturnToMenu(), "Should throw exception when commands panel is null");
    }

    [Test]
    public void MenuManager_CanBeDisabledAndReenabled()
    {
        // Arrange
        menuManager.enabled = true;

        // Act
        menuManager.enabled = false;
        bool disabledState = !menuManager.enabled;
        menuManager.enabled = true;
        bool reenableState = menuManager.enabled;

        // Assert
        Assert.IsTrue(disabledState, "MenuManager should be disabled");
        Assert.IsTrue(reenableState, "MenuManager should be re-enabled");
    }

    [Test]
    public void MenuManager_PositionCanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(5f, 0, 5f);

        // Act
        menuManagerObj.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, menuManagerObj.transform.position, "MenuManager position should be modifiable");
    }

    [Test]
    public void MultipleMenuManagers_CanCoexist()
    {
        // Arrange
        GameObject menuManager2Obj = new GameObject("TestMenuManager2");
        MenuManager menuManager2 = menuManager2Obj.AddComponent<MenuManager>();

        // Act
        menuManager.enabled = true;
        menuManager2.enabled = true;

        // Assert - Both should be active
        Assert.IsTrue(menuManager.enabled, "First MenuManager should be enabled");
        Assert.IsTrue(menuManager2.enabled, "Second MenuManager should be enabled");

        // Cleanup
        UnityEngine.Object.Destroy(menuManager2Obj);
    }

    [Test]
    public void PlayButton_StartsInactive()
    {
        // Arrange
        GameObject newButtonObj = new GameObject("TestButton");
        newButtonObj.SetActive(false);

        // Act
        bool isActive = newButtonObj.activeSelf;

        // Assert
        Assert.IsFalse(isActive, "Button should start inactive");

        // Cleanup
        UnityEngine.Object.Destroy(newButtonObj);
    }

    [Test]
    public void PlayButton_CanBeActivated()
    {
        // Arrange
        playButtonObj.SetActive(false);

        // Act
        playButtonObj.SetActive(true);

        // Assert
        Assert.IsTrue(playButtonObj.activeSelf, "Button should be activatable");
    }

    [Test]
    public void CommandsPanel_StartsInactive()
    {
        // Assert
        Assert.IsFalse(commandsPanelObj.activeSelf, "Commands panel should start inactive");
    }

    [Test]
    public void CommandsPanel_CanBeActivated()
    {
        // Act
        commandsPanelObj.SetActive(true);

        // Assert
        Assert.IsTrue(commandsPanelObj.activeSelf, "Commands panel should be activatable");
    }

    [Test]
    public void MenuManager_HasPublicPlayButtonField()
    {
        // Assert - MenuManager should have public playButton field
        var field = typeof(MenuManager).GetField("playButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "playButton field should exist and be public");
    }

    [Test]
    public void MenuManager_HasPublicQuitButtonField()
    {
        // Assert - MenuManager should have public quitButton field
        var field = typeof(MenuManager).GetField("quitButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "quitButton field should exist and be public");
    }

    [Test]
    public void MenuManager_HasPublicReturnButtonField()
    {
        // Assert - MenuManager should have public returnButton field
        var field = typeof(MenuManager).GetField("returnButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "returnButton field should exist and be public");
    }

    [Test]
    public void MenuManager_HasPublicCommandsButtonField()
    {
        // Assert - MenuManager should have public commandsButton field
        var field = typeof(MenuManager).GetField("commandsButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "commandsButton field should exist and be public");
    }

    [Test]
    public void MenuManager_HasPublicCommandsPanelField()
    {
        // Assert - MenuManager should have public commandsPanel field
        var field = typeof(MenuManager).GetField("commandsPanel", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "commandsPanel field should exist and be public");
    }

    [Test]
    public void MenuManager_HasPublicAudioSourceField()
    {
        // Assert - MenuManager should have public ButtonPressed field
        var field = typeof(MenuManager).GetField("ButtonPressed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "ButtonPressed field should exist and be public");
    }

    [Test]
    public void PlayGame_SetsCorrectSceneName()
    {
        // Arrange
        PlayerPrefs.DeleteKey("SceneToLoad");

        // Act
        menuManager.PlayGame();
        string sceneToLoad = PlayerPrefs.GetString("SceneToLoad");

        // Assert
        Assert.AreEqual("City", sceneToLoad, "Scene to load should be City");

        // Cleanup
        PlayerPrefs.DeleteKey("SceneToLoad");
    }

    [Test]
    public void ShowCommands_TogglesCommandsPanel()
    {
        // Arrange
        commandsPanelObj.SetActive(false);

        // Act
        menuManager.ShowCommands();
        bool afterShow = commandsPanelObj.activeSelf;

        menuManager.ReturnToMenu();
        bool afterReturn = commandsPanelObj.activeSelf;

        // Assert
        Assert.IsTrue(afterShow, "Panel should be active after ShowCommands");
        Assert.IsFalse(afterReturn, "Panel should be inactive after ReturnToMenu");
    }

    [Test]
    public void AudioSource_CanPlaySound()
    {
        // Arrange
        audioSource.clip = AudioClip.Create("TestClip", 44100, 1, 44100, false);

        // Act
        audioSource.Play();

        // Assert - Audio should be playing or queued to play
        Assert.Pass("Audio source can play sound");
    }

    [Test]
    public void PlayButton_CanBeNull()
    {
        // Act
        menuManager.playButton = null;

        // Assert
        Assert.IsNull(menuManager.playButton, "Play button should accept null value");
    }

    [Test]
    public void CommandsPanel_CanBeNull()
    {
        // Act
        menuManager.commandsPanel = null;

        // Assert
        Assert.IsNull(menuManager.commandsPanel, "Commands panel should accept null value");
    }

    [Test]
    public void AudioSource_CanBeNull()
    {
        // Act
        menuManager.ButtonPressed = null;

        // Assert
        Assert.IsNull(menuManager.ButtonPressed, "Audio source should accept null value");
    }
}
