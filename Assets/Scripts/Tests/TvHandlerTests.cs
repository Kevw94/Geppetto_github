using NUnit.Framework;
using UnityEngine;

public class TvHandlerTests
{
    private GameObject tvHandlerObj;
    private TvHandler tvHandler;
    private GameObject tvObj;

    [SetUp]
    public void Setup()
    {
        // Create TV GameObject
        tvObj = new GameObject("TestTV");
        tvObj.SetActive(true);

        // Create TvHandler GameObject
        tvHandlerObj = new GameObject("TestTvHandler");
        tvHandler = tvHandlerObj.AddComponent<TvHandler>();

        // Configure TvHandler
        tvHandler.tv = tvObj;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(tvHandlerObj);
        Object.Destroy(tvObj);
    }

    [Test]
    public void Setup_InitializesWithValidConfiguration()
    {
        // Assert
        Assert.IsNotNull(tvHandler, "TvHandler should be initialized");
        Assert.IsNotNull(tvObj, "TV GameObject should be assigned");
    }

    [Test]
    public void Tv_CanBeAssigned()
    {
        // Arrange
        GameObject newTvObj = new GameObject("NewTV");

        // Act
        tvHandler.tv = newTvObj;

        // Assert
        Assert.AreEqual(newTvObj, tvHandler.tv, "TV should be assignable");

        // Cleanup
        Object.Destroy(newTvObj);
    }

    [Test]
    public void Tv_CanBeSetToNull()
    {
        // Act
        tvHandler.tv = null;

        // Assert
        Assert.IsNull(tvHandler.tv, "TV should accept null value");
    }

    [Test]
    public void ToggleTv_HasCorrectSignature()
    {
        // Assert - ToggleTv method should exist
        var method = typeof(TvHandler).GetMethod("ToggleTv", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(method, "ToggleTv method should exist");
    }

    [Test]
    public void ToggleTv_TogglesActiveTvOff()
    {
        // Arrange
        tvObj.SetActive(true);
        Assert.IsTrue(tvObj.activeSelf, "TV should be active initially");

        // Act
        tvHandler.ToggleTv();

        // Assert
        Assert.IsFalse(tvObj.activeSelf, "TV should be inactive after toggle");
    }

    [Test]
    public void ToggleTv_TogglesInactiveTvOn()
    {
        // Arrange
        tvObj.SetActive(false);
        Assert.IsFalse(tvObj.activeSelf, "TV should be inactive initially");

        // Act
        tvHandler.ToggleTv();

        // Assert
        Assert.IsTrue(tvObj.activeSelf, "TV should be active after toggle");
    }

    [Test]
    public void ToggleTv_TogglesMultipleTimes()
    {
        // Arrange
        tvObj.SetActive(true);

        // Act & Assert - Toggle multiple times
        tvHandler.ToggleTv();
        Assert.IsFalse(tvObj.activeSelf, "TV should be inactive after first toggle");

        tvHandler.ToggleTv();
        Assert.IsTrue(tvObj.activeSelf, "TV should be active after second toggle");

        tvHandler.ToggleTv();
        Assert.IsFalse(tvObj.activeSelf, "TV should be inactive after third toggle");

        tvHandler.ToggleTv();
        Assert.IsTrue(tvObj.activeSelf, "TV should be active after fourth toggle");
    }

    [Test]
    public void ToggleTv_WithNullTv()
    {
        // Arrange
        tvHandler.tv = null;

        // Act & Assert - Should not throw exception
        Assert.DoesNotThrow(() => tvHandler.ToggleTv(), "Should handle null TV gracefully");
    }

    [Test]
    public void ToggleTv_WithActiveTv()
    {
        // Arrange
        tvObj.SetActive(true);

        // Act
        tvHandler.ToggleTv();

        // Assert
        Assert.IsFalse(tvObj.activeSelf, "Active TV should be deactivated");
    }

    [Test]
    public void ToggleTv_WithInactiveTv()
    {
        // Arrange
        tvObj.SetActive(false);

        // Act
        tvHandler.ToggleTv();

        // Assert
        Assert.IsTrue(tvObj.activeSelf, "Inactive TV should be activated");
    }

    [Test]
    public void Tv_ActiveSelfReflectsState()
    {
        // Arrange
        tvObj.SetActive(true);

        // Act
        bool initialState = tvObj.activeSelf;
        tvObj.SetActive(false);
        bool finalState = tvObj.activeSelf;

        // Assert
        Assert.IsTrue(initialState, "activeSelf should reflect true state");
        Assert.IsFalse(finalState, "activeSelf should reflect false state");
    }

    [Test]
    public void MultipleTvHandlers_CanCoexist()
    {
        // Arrange
        GameObject tvHandler2Obj = new GameObject("TestTvHandler2");
        TvHandler tvHandler2 = tvHandler2Obj.AddComponent<TvHandler>();
        
        GameObject tv2Obj = new GameObject("TestTV2");
        tv2Obj.SetActive(true);
        
        tvHandler2.tv = tv2Obj;

        // Act
        tvHandler.ToggleTv();
        tvHandler2.ToggleTv();

        // Assert - Both should toggle independently
        Assert.IsFalse(tvObj.activeSelf, "First TV should be inactive");
        Assert.IsFalse(tv2Obj.activeSelf, "Second TV should be inactive");

        // Cleanup
        Object.Destroy(tvHandler2Obj);
        Object.Destroy(tv2Obj);
    }

    [Test]
    public void TvHandler_CanBeDisabledAndReenabled()
    {
        // Arrange
        tvHandler.enabled = true;

        // Act
        tvHandler.enabled = false;
        bool disabledState = !tvHandler.enabled;
        tvHandler.enabled = true;
        bool reenableState = tvHandler.enabled;

        // Assert
        Assert.IsTrue(disabledState, "TvHandler should be disabled");
        Assert.IsTrue(reenableState, "TvHandler should be re-enabled");
    }

    [Test]
    public void TvHandler_PositionCanBeModified()
    {
        // Arrange
        Vector3 newPosition = new Vector3(5f, 0, 5f);

        // Act
        tvHandlerObj.transform.position = newPosition;

        // Assert
        Assert.AreEqual(newPosition, tvHandlerObj.transform.position, "TvHandler position should be modifiable");
    }

    [Test]
    public void Tv_CanBeReassignedAfterToggle()
    {
        // Arrange
        tvObj.SetActive(true);
        tvHandler.ToggleTv();
        Assert.IsFalse(tvObj.activeSelf, "TV should be inactive");

        // Create new TV
        GameObject newTvObj = new GameObject("NewTV");
        newTvObj.SetActive(true);

        // Act
        tvHandler.tv = newTvObj;
        tvHandler.ToggleTv();

        // Assert
        Assert.IsFalse(newTvObj.activeSelf, "New TV should be toggled off");
        Assert.IsFalse(tvObj.activeSelf, "Old TV should remain inactive");

        // Cleanup
        Object.Destroy(newTvObj);
    }

    [Test]
    public void ToggleTv_PreservesOtherGameObjectStates()
    {
        // Arrange
        GameObject otherObj = new GameObject("OtherObject");
        otherObj.SetActive(true);

        // Act
        tvHandler.ToggleTv();

        // Assert
        Assert.IsTrue(otherObj.activeSelf, "Other objects should not be affected");

        // Cleanup
        Object.Destroy(otherObj);
    }

    [Test]
    public void ToggleTv_WithChildGameObjects()
    {
        // Arrange
        GameObject childObj = new GameObject("ChildTV");
        childObj.transform.SetParent(tvObj.transform);
        childObj.SetActive(true);

        tvObj.SetActive(true);

        // Act
        tvHandler.ToggleTv();

        // Assert - Parent deactivation should deactivate parent
        Assert.IsFalse(tvObj.activeSelf, "Parent TV should be inactive");
        // Note: activeSelf returns false for children of inactive parents, but activeInHierarchy would be false
        Assert.IsFalse(childObj.activeInHierarchy, "Child should be inactive in hierarchy when parent is inactive");

        // Cleanup
        Object.Destroy(childObj);
    }

    [Test]
    public void Tv_StateCanBeCheckedBeforeToggle()
    {
        // Arrange
        tvObj.SetActive(true);

        // Act
        bool stateBeforeToggle = tvObj.activeSelf;
        tvHandler.ToggleTv();
        bool stateAfterToggle = tvObj.activeSelf;

        // Assert
        Assert.IsTrue(stateBeforeToggle, "State before toggle should be true");
        Assert.IsFalse(stateAfterToggle, "State after toggle should be false");
        Assert.AreNotEqual(stateBeforeToggle, stateAfterToggle, "States should be different");
    }

    [Test]
    public void ToggleTv_ConsistentBehavior()
    {
        // Arrange
        tvObj.SetActive(true);

        // Act & Assert - Multiple toggles should be consistent
        for (int i = 0; i < 10; i++)
        {
            bool expectedState = (i % 2 == 0) ? false : true;
            tvHandler.ToggleTv();
            Assert.AreEqual(expectedState, tvObj.activeSelf, $"Toggle {i + 1} should result in state {expectedState}");
        }
    }

    [Test]
    public void TvHandler_HasPublicTvField()
    {
        // Assert - TvHandler should have public tv field
        var field = typeof(TvHandler).GetField("tv", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(field, "tv field should exist and be public");
    }

    [Test]
    public void Tv_ToggleAffectsOnlyAssignedGameObject()
    {
        // Arrange
        GameObject unrelatedObj = new GameObject("UnrelatedTV");
        unrelatedObj.SetActive(true);

        tvObj.SetActive(true);

        // Act
        tvHandler.ToggleTv();

        // Assert
        Assert.IsFalse(tvObj.activeSelf, "Assigned TV should be toggled");
        Assert.IsTrue(unrelatedObj.activeSelf, "Unrelated TV should not be affected");

        // Cleanup
        Object.Destroy(unrelatedObj);
    }
}
