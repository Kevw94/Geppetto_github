using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider loadingBar;
    public TextMeshProUGUI percentageText;

    [Header("Settings")]
    public string sceneToLoad = "City";
    public float smoothSpeed = 1f;
    public float endPause = 0.5f;

    private float displayedProgress = 0f;

    void Start()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    IEnumerator LoadSceneRoutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            displayedProgress = Mathf.MoveTowards(displayedProgress, targetProgress, Time.deltaTime * smoothSpeed);
            loadingBar.value = displayedProgress;

            if (percentageText != null)
                percentageText.text = Mathf.RoundToInt(displayedProgress * 100f) + "%";

            yield return null;
        }


        while (displayedProgress < 1f)
        {
            displayedProgress = Mathf.MoveTowards(displayedProgress, 1f, Time.deltaTime * smoothSpeed);
            loadingBar.value = displayedProgress;

            if (percentageText != null)
                percentageText.text = Mathf.RoundToInt(displayedProgress * 99f) + "%";

            yield return null;
        }

        yield return new WaitForSeconds(endPause);
        asyncLoad.allowSceneActivation = true;
    }
}
