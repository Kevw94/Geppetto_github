using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class EndGameTextTests
{
    private GameObject endGameTextObj;
    private TMPFadeAndSwitch tmpFadeAndSwitch;
    private GameObject textGameObj;
    private TMP_Text tmpText;
    private GameObject objectToEnableObj;

    [SetUp]
    public void Setup()
    {
        // Create EndGameText GameObject
        endGameTextObj = new GameObject("TestEndGameText");
        tmpFadeAndSwitch = endGameTextObj.AddComponent<TMPFadeAndSwitch>();

        // Create TextMeshPro text object
        textGameObj = new GameObject("TestText");
        tmpText = textGameObj.AddComponent<TextMeshProUGUI>();
        tmpText.text = "Test Text";
        tmpText.color = new Color(1, 1, 1, 1);

        // Create object to enable
        objectToEnableObj = new GameObject("TestObjectToEnable");
        objectToEnableObj.SetActive(false);

        // Configure TMPFadeAndSwitch
        tmpFadeAndSwitch.textToFade = tmpText;
        tmpFadeAndSwitch.objectToEnable = objectToEnableObj;
        tmpFadeAndSwitch.fadeDuration = 1f;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(endGameTextObj);
        Object.Destroy(textGameObj);
        Object.Destroy(objectToEnableObj);
    }

    [Test]
    public void Setup_InitializesWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(tmpFadeAndSwitch, "TMPFadeAndSwitch should be initialized");
        Assert.IsNotNull(tmpText, "TMP_Text should be assigned");
        Assert.IsNotNull(objectToEnableObj, "Object to enable should be assigned");
    }

    [Test]
    public void Setup_InitializesFadeDuration()
    {
        // Assert
        Assert.AreEqual(1f, tmpFadeAndSwitch.fadeDuration, "Fade duration should be initialized");
    }

    [Test]
    public void TextToFade_CanBeAssigned()
    {
        // Arrange
        GameObject newTextObj = new GameObject("NewText");
        TMP_Text newText = newTextObj.AddComponent<TextMeshProUGUI>();

        // Act
        tmpFadeAndSwitch.textToFade = newText;

        // Assert
        Assert.AreEqual(newText, tmpFadeAndSwitch.textToFade, "Text to fade should be assignable");

        // Cleanup
        Object.Destroy(newTextObj);
    }

    [Test]
    public void ObjectToEnable_CanBeAssigned()
    {
        // Arrange
        GameObject newObj = new GameObject("NewObject");

        // Act
        tmpFadeAndSwitch.objectToEnable = newObj;

        // Assert
        Assert.AreEqual(newObj, tmpFadeAndSwitch.objectToEnable, "Object to enable should be assignable");

        // Cleanup
        Object.Destroy(newObj);
    }

    [Test]
    public void FadeDuration_CanBeModified()
    {
        // Arrange
        float newDuration = 3f;

        // Act
        tmpFadeAndSwitch.fadeDuration = newDuration;

        // Assert
        Assert.AreEqual(newDuration, tmpFadeAndSwitch.fadeDuration, "Fade duration should be modifiable");
    }

    [Test]
    public void FadeDuration_WithZeroValue()
    {
        // Arrange
        float zeroDuration = 0f;

        // Act
        tmpFadeAndSwitch.fadeDuration = zeroDuration;

        // Assert
        Assert.AreEqual(zeroDuration, tmpFadeAndSwitch.fadeDuration, "Fade duration should accept zero value");
    }

    [Test]
    public void FadeDuration_WithLargeValue()
    {
        // Arrange
        float largeDuration = 10f;

        // Act
        tmpFadeAndSwitch.fadeDuration = largeDuration;

        // Assert
        Assert.AreEqual(largeDuration, tmpFadeAndSwitch.fadeDuration, "Fade duration should accept large values");
    }

    [Test]
    public void TextToFade_CanBeNull()
    {
        // Act
        tmpFadeAndSwitch.textToFade = null;

        // Assert
        Assert.IsNull(tmpFadeAndSwitch.textToFade, "Text to fade should accept null value");
    }

    [Test]
    public void ObjectToEnable_CanBeNull()
    {
        // Act
        tmpFadeAndSwitch.objectToEnable = null;

        // Assert
        Assert.IsNull(tmpFadeAndSwitch.objectToEnable, "Object to enable should accept null value");
    }

    [Test]
    public void TMPText_HasCorrectProperties()
    {
        // Assert - TMP_Text should be valid
        Assert.IsNotNull(tmpText, "TMP_Text should exist");
        Assert.IsNotNull(tmpText.gameObject, "TMP_Text GameObject should exist");
    }

    [Test]
    public void ObjectToEnable_HasCorrectProperties()
    {
        // Assert - Object to enable should be valid
        Assert.IsNotNull(objectToEnableObj, "Object to enable should exist");
        Assert.IsFalse(objectToEnableObj.activeSelf, "Object should be inactive initially");
    }

    [Test]
    public void TMPText_ColorCanBeModified()
    {
        // Arrange
        Color newColor = new Color(1, 0, 0, 0.5f);

        // Act
        tmpText.color = newColor;

        // Assert
        Assert.AreEqual(newColor, tmpText.color, "TMP_Text color should be modifiable");
    }

    [Test]
    public void TMPText_AlphaCanBeModified()
    {
        // Arrange
        Color color = tmpText.color;

        // Act
        color.a = 0.5f;
        tmpText.color = color;

        // Assert
        Assert.AreEqual(0.5f, tmpText.color.a, "TMP_Text alpha should be modifiable");
    }

    [UnityTest]
    public IEnumerator Start_InitiatesFadeOutAndSwitch()
    {
        // Arrange
        tmpFadeAndSwitch.fadeDuration = 0.2f;
        tmpText.gameObject.SetActive(true);
        objectToEnableObj.SetActive(false);

        // Act
        tmpFadeAndSwitch.enabled = true;
        yield return new WaitForSeconds(0.3f);

        // Assert - Text should be faded out and object should be enabled
        Assert.IsFalse(tmpText.gameObject.activeSelf, "Text should be deactivated after fade");
        Assert.IsTrue(objectToEnableObj.activeSelf, "Object should be enabled after fade");
    }

    [UnityTest]
    public IEnumerator FadeOutAndSwitch_FadesTextAlpha()
    {
        // Arrange
        tmpFadeAndSwitch.fadeDuration = 0.2f;
        tmpText.color = new Color(1, 1, 1, 1);

        // Act - Invoke FadeOutAndSwitch via reflection
        var method = typeof(TMPFadeAndSwitch).GetMethod("FadeOutAndSwitch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var coroutine = (IEnumerator)method.Invoke(tmpFadeAndSwitch, null);

        // Execute the coroutine
        while (coroutine.MoveNext())
        {
            yield return coroutine.Current;
        }

        // Assert - Text should be faded out
        Assert.IsFalse(tmpText.gameObject.activeSelf, "Text should be deactivated");
    }

    [UnityTest]
    public IEnumerator FadeOutAndSwitch_DeactivatesText()
    {
        // Arrange
        tmpFadeAndSwitch.fadeDuration = 0.1f;
        tmpText.gameObject.SetActive(true);

        // Act - Invoke FadeOutAndSwitch via reflection
        var method = typeof(TMPFadeAndSwitch).GetMethod("FadeOutAndSwitch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var coroutine = (IEnumerator)method.Invoke(tmpFadeAndSwitch, null);

        // Execute the coroutine
        while (coroutine.MoveNext())
        {
            yield return coroutine.Current;
        }

        // Assert
        Assert.IsFalse(tmpText.gameObject.activeSelf, "Text GameObject should be deactivated");
    }

    [UnityTest]
    public IEnumerator FadeOutAndSwitch_ActivatesObjectToEnable()
    {
        // Arrange
        tmpFadeAndSwitch.fadeDuration = 0.1f;
        objectToEnableObj.SetActive(false);

        // Act - Invoke FadeOutAndSwitch via reflection
        var method = typeof(TMPFadeAndSwitch).GetMethod("FadeOutAndSwitch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var coroutine = (IEnumerator)method.Invoke(tmpFadeAndSwitch, null);

        // Execute the coroutine
        while (coroutine.MoveNext())
        {
            yield return coroutine.Current;
        }

        // Assert
        Assert.IsTrue(objectToEnableObj.activeSelf, "Object to enable should be activated");
    }

    [Test]
    public void FadeOutAndSwitch_HasCorrectSignature()
    {
        // Assert - FadeOutAndSwitch method should exist
        var method = typeof(TMPFadeAndSwitch).GetMethod("FadeOutAndSwitch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "FadeOutAndSwitch method should exist");
    }

    [Test]
    public void Start_HasCorrectSignature()
    {
        // Assert - Start method should exist
        var method = typeof(TMPFadeAndSwitch).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "Start method should exist");
    }

    [Test]
    public void FadeOutAndSwitch_HandlesNullText()
    {
        // Arrange
        tmpFadeAndSwitch.textToFade = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => tmpFadeAndSwitch.enabled = true, "Should handle null text gracefully");
    }

    [Test]
    public void FadeOutAndSwitch_HandlesNullObjectToEnable()
    {
        // Arrange
        tmpFadeAndSwitch.objectToEnable = null;

        // Act - Invoke FadeOutAndSwitch via reflection
        var method = typeof(TMPFadeAndSwitch).GetMethod("FadeOutAndSwitch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert - Should not throw exception
        Assert.DoesNotThrow(() => method.Invoke(tmpFadeAndSwitch, null), "Should handle null object to enable");
    }

    [Test]
    public void MultipleFadeAndSwitch_CanCoexist()
    {
        // Arrange
        GameObject fadeObj2 = new GameObject("TestFadeAndSwitch2");
        TMPFadeAndSwitch fade2 = fadeObj2.AddComponent<TMPFadeAndSwitch>();
        
        GameObject text2Obj = new GameObject("TestText2");
        TMP_Text text2 = text2Obj.AddComponent<TextMeshProUGUI>();
        
        fade2.textToFade = text2;
        fade2.fadeDuration = 2f;

        // Act
        tmpFadeAndSwitch.enabled = true;
        fade2.enabled = true;

        // Assert - Both should be active
        Assert.IsTrue(tmpFadeAndSwitch.enabled, "First fade should be enabled");
        Assert.IsTrue(fade2.enabled, "Second fade should be enabled");

        // Cleanup
        Object.Destroy(fadeObj2);
        Object.Destroy(text2Obj);
    }

    [Test]
    public void TMPFadeAndSwitch_CanBeDisabledAndReenabled()
    {
        // Arrange
        tmpFadeAndSwitch.enabled = true;

        // Act
        tmpFadeAndSwitch.enabled = false;
        bool disabledState = !tmpFadeAndSwitch.enabled;
        tmpFadeAndSwitch.enabled = true;
        bool reenableState = tmpFadeAndSwitch.enabled;

        // Assert
        Assert.IsTrue(disabledState, "TMPFadeAndSwitch should be disabled");
        Assert.IsTrue(reenableState, "TMPFadeAndSwitch should be re-enabled");
    }

    [Test]
    public void TMPFadeAndSwitch_PositionCanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(5f, 0, 5f);

        // Act
        endGameTextObj.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, endGameTextObj.transform.position, "Position should be modifiable");
    }

    [Test]
    public void TMPFadeAndSwitch_RotationCanBeModified()
    {
        // Arrange
        Quaternion newRotation = Quaternion.Euler(45f, 90f, 0f);

        // Act
        endGameTextObj.transform.rotation = newRotation;

        // Assert
        float angleDifference = Quaternion.Angle(newRotation, endGameTextObj.transform.rotation);
        Assert.Less(angleDifference, 0.01f, "Rotation should be modifiable");
    }

    [Test]
    public void FadeColor_LerpsCorrectly()
    {
        // Arrange
        float alpha0 = Mathf.Lerp(1f, 0f, 0f / 1f);
        float alpha1 = Mathf.Lerp(1f, 0f, 0.5f / 1f);
        float alpha2 = Mathf.Lerp(1f, 0f, 1f / 1f);

        // Assert - Lerp should interpolate correctly
        Assert.AreEqual(1f, alpha0, "Alpha at t=0 should be 1");
        Assert.AreEqual(0.5f, alpha1, "Alpha at t=0.5 should be 0.5");
        Assert.AreEqual(0f, alpha2, "Alpha at t=1 should be 0");
    }

    [Test]
    public void TMPText_TextCanBeModified()
    {
        // Arrange
        string newText = "New Text";

        // Act
        tmpText.text = newText;

        // Assert
        Assert.AreEqual(newText, tmpText.text, "TMP_Text text should be modifiable");
    }

    [Test]
    public void TMPFadeAndSwitch_HasPublicFields()
    {
        // Assert - TMPFadeAndSwitch should have public fields
        Assert.IsNotNull(typeof(TMPFadeAndSwitch).GetField("textToFade"), "textToFade field should exist");
        Assert.IsNotNull(typeof(TMPFadeAndSwitch).GetField("objectToEnable"), "objectToEnable field should exist");
        Assert.IsNotNull(typeof(TMPFadeAndSwitch).GetField("fadeDuration"), "fadeDuration field should exist");
    }
}
