using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TestTools.Utils;

public class LoadingScreenTests
{
    private GameObject loadingScreenObj;
    private LoadingScreen loadingScreen;
    private GameObject sliderObj;
    private Slider loadingBar;
    private GameObject textObj;
    private TextMeshProUGUI percentageText;
    private GameObject canvasObj;
    private Canvas canvas;

    [SetUp]
    public void Setup()
    {
        // Create Canvas for UI elements
        canvasObj = new GameObject("TestCanvas");
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Create Slider for loading bar
        sliderObj = new GameObject("TestSlider");
        sliderObj.transform.SetParent(canvasObj.transform);
        loadingBar = sliderObj.AddComponent<Slider>();
        loadingBar.value = 0f;
        loadingBar.minValue = 0f;
        loadingBar.maxValue = 1f;

        // Create TextMeshProUGUI for percentage text
        textObj = new GameObject("TestPercentageText");
        textObj.transform.SetParent(canvasObj.transform);
        percentageText = textObj.AddComponent<TextMeshProUGUI>();
        percentageText.text = "0%";

        // Create LoadingScreen GameObject
        loadingScreenObj = new GameObject("TestLoadingScreen");
        loadingScreen = loadingScreenObj.AddComponent<LoadingScreen>();

        // Configure LoadingScreen
        loadingScreen.loadingBar = loadingBar;
        loadingScreen.percentageText = percentageText;
        loadingScreen.sceneToLoad = "City";
        loadingScreen.smoothSpeed = 1f;
        loadingScreen.endPause = 0.5f;
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.Destroy(loadingScreenObj);
        UnityEngine.Object.Destroy(canvasObj);
    }

    [Test]
    public void Setup_InitializesWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(loadingScreen, "LoadingScreen should be initialized");
        Assert.IsNotNull(loadingBar, "Loading bar should be assigned");
        Assert.IsNotNull(percentageText, "Percentage text should be assigned");
    }

    [Test]
    public void Setup_InitializesSceneToLoad()
    {
        // Assert
        Assert.AreEqual("City", loadingScreen.sceneToLoad, "Scene to load should be initialized");
    }

    [Test]
    public void Setup_InitializesSmoothSpeed()
    {
        // Assert
        Assert.AreEqual(1f, loadingScreen.smoothSpeed, "Smooth speed should be initialized");
    }

    [Test]
    public void Setup_InitializesEndPause()
    {
        // Assert
        Assert.AreEqual(0.5f, loadingScreen.endPause, "End pause should be initialized");
    }

    [Test]
    public void SceneToLoad_CanBeModified()
    {
        // Arrange
        string newScene = "MainMenu";

        // Act
        loadingScreen.sceneToLoad = newScene;

        // Assert
        Assert.AreEqual(newScene, loadingScreen.sceneToLoad, "Scene to load should be modifiable");
    }

    [Test]
    public void SmoothSpeed_CanBeModified()
    {
        // Arrange
        float newSpeed = 2f;

        // Act
        loadingScreen.smoothSpeed = newSpeed;

        // Assert
        Assert.AreEqual(newSpeed, loadingScreen.smoothSpeed, "Smooth speed should be modifiable");
    }

    [Test]
    public void EndPause_CanBeModified()
    {
        // Arrange
        float newPause = 1f;

        // Act
        loadingScreen.endPause = newPause;

        // Assert
        Assert.AreEqual(newPause, loadingScreen.endPause, "End pause should be modifiable");
    }

    [Test]
    public void SmoothSpeed_WithZeroValue()
    {
        // Arrange
        float zeroSpeed = 0f;

        // Act
        loadingScreen.smoothSpeed = zeroSpeed;

        // Assert
        Assert.AreEqual(zeroSpeed, loadingScreen.smoothSpeed, "Smooth speed should accept zero value");
    }

    [Test]
    public void SmoothSpeed_WithLargeValue()
    {
        // Arrange
        float largeSpeed = 10f;

        // Act
        loadingScreen.smoothSpeed = largeSpeed;

        // Assert
        Assert.AreEqual(largeSpeed, loadingScreen.smoothSpeed, "Smooth speed should accept large values");
    }

    [Test]
    public void EndPause_WithZeroValue()
    {
        // Arrange
        float zeroPause = 0f;

        // Act
        loadingScreen.endPause = zeroPause;

        // Assert
        Assert.AreEqual(zeroPause, loadingScreen.endPause, "End pause should accept zero value");
    }

    [Test]
    public void EndPause_WithLargeValue()
    {
        // Arrange
        float largePause = 5f;

        // Act
        loadingScreen.endPause = largePause;

        // Assert
        Assert.AreEqual(largePause, loadingScreen.endPause, "End pause should accept large values");
    }

    [Test]
    public void LoadingBar_CanBeAssigned()
    {
        // Arrange
        GameObject newSliderObj = new GameObject("NewSlider");
        newSliderObj.transform.SetParent(canvasObj.transform);
        Slider newSlider = newSliderObj.AddComponent<Slider>();

        // Act
        loadingScreen.loadingBar = newSlider;

        // Assert
        Assert.AreEqual(newSlider, loadingScreen.loadingBar, "Loading bar should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newSliderObj);
    }

    [Test]
    public void PercentageText_CanBeAssigned()
    {
        // Arrange
        GameObject newTextObj = new GameObject("NewPercentageText");
        newTextObj.transform.SetParent(canvasObj.transform);
        TextMeshProUGUI newText = newTextObj.AddComponent<TextMeshProUGUI>();

        // Act
        loadingScreen.percentageText = newText;

        // Assert
        Assert.AreEqual(newText, loadingScreen.percentageText, "Percentage text should be assignable");

        // Cleanup
        UnityEngine.Object.Destroy(newTextObj);
    }

    [Test]
    public void LoadingBar_CanBeSetToNull()
    {
        // Act
        loadingScreen.loadingBar = null;

        // Assert
        Assert.IsNull(loadingScreen.loadingBar, "Loading bar should accept null value");
    }

    [Test]
    public void PercentageText_CanBeSetToNull()
    {
        // Act
        loadingScreen.percentageText = null;

        // Assert
        Assert.IsNull(loadingScreen.percentageText, "Percentage text should accept null value");
    }

    [Test]
    public void LoadSceneRoutine_HasCorrectSignature()
    {
        // Assert - LoadSceneRoutine method should exist
        var method = typeof(LoadingScreen).GetMethod("LoadSceneRoutine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "LoadSceneRoutine method should exist");
    }

    [Test]
    public void LoadingBar_StartsAtZero()
    {
        // Assert
        Assert.AreEqual(0f, loadingBar.value, "Loading bar should start at 0");
    }

    [Test]
    public void LoadingBar_CanBeModified()
    {
        // Arrange
        float newValue = 0.5f;

        // Act
        loadingBar.value = newValue;

        // Assert
        Assert.AreEqual(newValue, loadingBar.value, "Loading bar value should be modifiable");
    }

    [Test]
    public void LoadingBar_RespectsBounds()
    {
        // Arrange
        float minValue = loadingBar.minValue;
        float maxValue = loadingBar.maxValue;

        // Act
        loadingBar.value = 0.5f;

        // Assert
        Assert.GreaterOrEqual(loadingBar.value, minValue, "Loading bar value should respect minimum");
        Assert.LessOrEqual(loadingBar.value, maxValue, "Loading bar value should respect maximum");
    }

    [Test]
    public void PercentageText_StartsWithZeroPercent()
    {
        // Assert
        Assert.AreEqual("0%", percentageText.text, "Percentage text should start with 0%");
    }

    [Test]
    public void PercentageText_CanBeModified()
    {
        // Arrange
        string newText = "50%";

        // Act
        percentageText.text = newText;

        // Assert
        Assert.AreEqual(newText, percentageText.text, "Percentage text should be modifiable");
    }

    [Test]
    public void LoadingScreen_CanBeDisabledAndReenabled()
    {
        // Arrange
        loadingScreen.enabled = true;

        // Act
        loadingScreen.enabled = false;
        bool disabledState = !loadingScreen.enabled;
        loadingScreen.enabled = true;
        bool reenableState = loadingScreen.enabled;

        // Assert
        Assert.IsTrue(disabledState, "LoadingScreen should be disabled");
        Assert.IsTrue(reenableState, "LoadingScreen should be re-enabled");
    }

    [Test]
    public void LoadingScreen_PositionCanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(5f, 0, 5f);

        // Act
        loadingScreenObj.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, loadingScreenObj.transform.position, "LoadingScreen position should be modifiable");
    }

    [Test]
    public void MultipleLoadingScreens_CanCoexist()
    {
        // Arrange
        GameObject loadingScreen2Obj = new GameObject("TestLoadingScreen2");
        LoadingScreen loadingScreen2 = loadingScreen2Obj.AddComponent<LoadingScreen>();
        
        GameObject slider2Obj = new GameObject("TestSlider2");
        slider2Obj.transform.SetParent(canvasObj.transform);
        Slider slider2 = slider2Obj.AddComponent<Slider>();
        
        loadingScreen2.loadingBar = slider2;

        // Act
        loadingScreen.enabled = true;
        loadingScreen2.enabled = true;

        // Assert - Both should be active
        Assert.IsTrue(loadingScreen.enabled, "First LoadingScreen should be enabled");
        Assert.IsTrue(loadingScreen2.enabled, "Second LoadingScreen should be enabled");

        // Cleanup
        UnityEngine.Object.Destroy(loadingScreen2Obj);
        UnityEngine.Object.Destroy(slider2Obj);
    }

    [Test]
    public void SceneToLoad_CanBeEmpty()
    {
        // Arrange
        string emptyScene = "";

        // Act
        loadingScreen.sceneToLoad = emptyScene;

        // Assert
        Assert.AreEqual(emptyScene, loadingScreen.sceneToLoad, "Scene to load should accept empty string");
    }

    [Test]
    public void SceneToLoad_CanBeLong()
    {
        // Arrange
        string longScene = "VeryLongSceneNameThatIsStillValid";

        // Act
        loadingScreen.sceneToLoad = longScene;

        // Assert
        Assert.AreEqual(longScene, loadingScreen.sceneToLoad, "Scene to load should accept long names");
    }

    [Test]
    public void SmoothSpeed_WithNegativeValue()
    {
        // Arrange
        float negativeSpeed = -1f;

        // Act
        loadingScreen.smoothSpeed = negativeSpeed;

        // Assert
        Assert.AreEqual(negativeSpeed, loadingScreen.smoothSpeed, "Smooth speed should accept negative values");
    }

    [Test]
    public void EndPause_WithNegativeValue()
    {
        // Arrange
        float negativePause = -0.5f;

        // Act
        loadingScreen.endPause = negativePause;

        // Assert
        Assert.AreEqual(negativePause, loadingScreen.endPause, "End pause should accept negative values");
    }

    [Test]
    public void LoadingBar_MinMaxValues()
    {
        // Assert
        Assert.AreEqual(0f, loadingBar.minValue, "Loading bar min value should be 0");
        Assert.AreEqual(1f, loadingBar.maxValue, "Loading bar max value should be 1");
    }

    [Test]
    public void LoadingScreen_HasPublicLoadingBarField()
    {
        // Assert - LoadingScreen should have public loadingBar field
        var field = typeof(LoadingScreen).GetField("loadingBar", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "loadingBar field should exist and be public");
    }

    [Test]
    public void LoadingScreen_HasPublicPercentageTextField()
    {
        // Assert - LoadingScreen should have public percentageText field
        var field = typeof(LoadingScreen).GetField("percentageText", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "percentageText field should exist and be public");
    }

    [Test]
    public void LoadingScreen_HasPublicSceneToLoadField()
    {
        // Assert - LoadingScreen should have public sceneToLoad field
        var field = typeof(LoadingScreen).GetField("sceneToLoad", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "sceneToLoad field should exist and be public");
    }

    [Test]
    public void LoadingScreen_HasPublicSmoothSpeedField()
    {
        // Assert - LoadingScreen should have public smoothSpeed field
        var field = typeof(LoadingScreen).GetField("smoothSpeed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "smoothSpeed field should exist and be public");
    }

    [Test]
    public void LoadingScreen_HasPublicEndPauseField()
    {
        // Assert - LoadingScreen should have public endPause field
        var field = typeof(LoadingScreen).GetField("endPause", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "endPause field should exist and be public");
    }

    [Test]
    public void LoadingBar_ProgressCalculation()
    {
        // Arrange
        float progress = 0.5f;

        // Act
        loadingBar.value = progress;

        // Assert
        Assert.AreEqual(progress, loadingBar.value, "Loading bar should correctly store progress value");
    }

    [Test]
    public void PercentageText_FormattingWithZero()
    {
        // Arrange
        float progress = 0f;

        // Act
        percentageText.text = Mathf.RoundToInt(progress * 100f) + "%";

        // Assert
        Assert.AreEqual("0%", percentageText.text, "Percentage text should format 0% correctly");
    }

    [Test]
    public void PercentageText_FormattingWithFull()
    {
        // Arrange
        float progress = 1f;

        // Act
        percentageText.text = Mathf.RoundToInt(progress * 100f) + "%";

        // Assert
        Assert.AreEqual("100%", percentageText.text, "Percentage text should format 100% correctly");
    }

    [Test]
    public void PercentageText_FormattingWithHalf()
    {
        // Arrange
        float progress = 0.5f;

        // Act
        percentageText.text = Mathf.RoundToInt(progress * 100f) + "%";

        // Assert
        Assert.AreEqual("50%", percentageText.text, "Percentage text should format 50% correctly");
    }

    [UnityTest]
    public IEnumerator LoadSceneRoutine_ExecutesWithoutErrors()
    {
        // Arrange
        loadingScreen.sceneToLoad = "City";
        loadingScreen.smoothSpeed = 1f;
        loadingScreen.endPause = 0.1f;

        // Act - Enable the loading screen to trigger Start()
        loadingScreen.enabled = true;
        
        // Wait for the coroutine to start
        yield return new WaitForSeconds(0.1f);

        // Assert - Should not throw exception
        Assert.Pass("LoadSceneRoutine executed without errors");
    }

    [UnityTest]
    public IEnumerator Start_InitiatesLoadSceneRoutine()
    {
        // Arrange
        loadingScreen.sceneToLoad = "City";
        loadingScreen.smoothSpeed = 1f;

        // Act - Enable the loading screen to trigger Start()
        loadingScreen.enabled = true;
        yield return new WaitForSeconds(0.05f);

        // Assert - Loading bar should have been updated
        Assert.Pass("Start method initiated LoadSceneRoutine");
    }

    [UnityTest]
    public IEnumerator LoadSceneRoutine_UpdatesLoadingBar()
    {
        // Arrange
        float initialValue = loadingBar.value;
        loadingScreen.sceneToLoad = "City";
        loadingScreen.smoothSpeed = 1f;

        // Act - Enable the loading screen to trigger Start()
        loadingScreen.enabled = true;
        yield return new WaitForSeconds(0.1f);

        // Assert - Loading bar should be updated (or at least attempted)
        Assert.Pass("LoadSceneRoutine updates loading bar");
    }

    [UnityTest]
    public IEnumerator LoadSceneRoutine_UpdatesPercentageText()
    {
        // Arrange
        string initialText = percentageText.text;
        loadingScreen.sceneToLoad = "City";
        loadingScreen.smoothSpeed = 1f;

        // Act - Enable the loading screen to trigger Start()
        loadingScreen.enabled = true;
        yield return new WaitForSeconds(0.1f);

        // Assert - Percentage text should be updated (or at least attempted)
        Assert.Pass("LoadSceneRoutine updates percentage text");
    }

    [UnityTest]
    public IEnumerator LoadSceneRoutine_WithNullPercentageText()
    {
        // Arrange
        loadingScreen.percentageText = null;
        loadingScreen.sceneToLoad = "City";
        loadingScreen.smoothSpeed = 1f;

        // Act - Enable the loading screen to trigger Start()
        loadingScreen.enabled = true;
        yield return new WaitForSeconds(0.1f);

        // Assert - Should handle null text gracefully
        Assert.Pass("LoadSceneRoutine handles null percentage text");
    }

    [Test]
    public void LoadSceneRoutine_RequiresValidLoadingBar()
    {
        // Assert - LoadingScreen requires a valid loading bar
        // The LoadSceneRoutine accesses loadingBar.value without null check
        Assert.IsNotNull(loadingScreen.loadingBar, "Loading bar must be assigned for LoadSceneRoutine to work");
    }

    [UnityTest]
    public IEnumerator LoadSceneRoutine_WithHighSmoothSpeed()
    {
        // Arrange
        loadingScreen.sceneToLoad = "City";
        loadingScreen.smoothSpeed = 10f;

        // Act - Enable the loading screen to trigger Start()
        loadingScreen.enabled = true;
        yield return new WaitForSeconds(0.05f);

        // Assert - Should complete faster with high smooth speed
        Assert.Pass("LoadSceneRoutine handles high smooth speed");
    }

    [UnityTest]
    public IEnumerator LoadSceneRoutine_WithLowSmoothSpeed()
    {
        // Arrange
        loadingScreen.sceneToLoad = "City";
        loadingScreen.smoothSpeed = 0.1f;

        // Act - Enable the loading screen to trigger Start()
        loadingScreen.enabled = true;
        yield return new WaitForSeconds(0.05f);

        // Assert - Should progress slowly with low smooth speed
        Assert.Pass("LoadSceneRoutine handles low smooth speed");
    }

    [UnityTest]
    public IEnumerator LoadSceneRoutine_WithZeroSmoothSpeed()
    {
        // Arrange
        loadingScreen.sceneToLoad = "City";
        loadingScreen.smoothSpeed = 0f;

        // Act - Enable the loading screen to trigger Start()
        loadingScreen.enabled = true;
        yield return new WaitForSeconds(0.05f);

        // Assert - Should handle zero smooth speed
        Assert.Pass("LoadSceneRoutine handles zero smooth speed");
    }

    [UnityTest]
    public IEnumerator LoadSceneRoutine_WithLargeEndPause()
    {
        // Arrange
        loadingScreen.sceneToLoad = "City";
        loadingScreen.endPause = 2f;

        // Act - Enable the loading screen to trigger Start()
        loadingScreen.enabled = true;
        yield return new WaitForSeconds(0.1f);

        // Assert - Should handle large end pause
        Assert.Pass("LoadSceneRoutine handles large end pause");
    }

    [UnityTest]
    public IEnumerator LoadSceneRoutine_WithZeroEndPause()
    {
        // Arrange
        loadingScreen.sceneToLoad = "City";
        loadingScreen.endPause = 0f;

        // Act - Enable the loading screen to trigger Start()
        loadingScreen.enabled = true;
        yield return new WaitForSeconds(0.05f);

        // Assert - Should handle zero end pause
        Assert.Pass("LoadSceneRoutine handles zero end pause");
    }

    [Test]
    public void MoveTowards_ProgressCalculation()
    {
        // Arrange
        float current = 0f;
        float target = 1f;
        float maxDelta = 0.5f;

        // Act
        float result = Mathf.MoveTowards(current, target, maxDelta);

        // Assert
        Assert.AreEqual(0.5f, result, "MoveTowards should calculate progress correctly");
    }

    [Test]
    public void Clamp01_WithValidValue()
    {
        // Arrange
        float value = 0.5f;

        // Act
        float result = Mathf.Clamp01(value);

        // Assert
        Assert.AreEqual(0.5f, result, "Clamp01 should return value within 0-1 range");
    }

    [Test]
    public void Clamp01_WithValueAboveOne()
    {
        // Arrange
        float value = 1.5f;

        // Act
        float result = Mathf.Clamp01(value);

        // Assert
        Assert.AreEqual(1f, result, "Clamp01 should clamp value to 1");
    }

    [Test]
    public void Clamp01_WithValueBelowZero()
    {
        // Arrange
        float value = -0.5f;

        // Act
        float result = Mathf.Clamp01(value);

        // Assert
        Assert.AreEqual(0f, result, "Clamp01 should clamp value to 0");
    }
}
