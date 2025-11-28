using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class VRLoadingManager : MonoBehaviour
{
    public Slider loadingBar;
    private string sceneToLoad;
    private float displayedProgress = 0f;

    void Start()
    {
        sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        Debug.Log("[Loader] Scene to load = " + sceneToLoad);

        StartCoroutine(LoadSceneRoutine());
    }

    IEnumerator LoadSceneRoutine()
    {
        // Lancement du chargement async
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        Debug.Log("[Loader] Async load started");

        // Tant que la scène charge (va jusqu'à 0.9)
        while (asyncLoad.progress < 0.9f)
        {
            float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // MoveTowards → atteint la valeur réellement (plus d’asymptote)
            displayedProgress = Mathf.MoveTowards(displayedProgress, targetProgress, Time.deltaTime * 1f);
            loadingBar.value = displayedProgress;

            Debug.Log("[Loader] Loading progress: " + asyncLoad.progress + " (display: " + displayedProgress + ")");

            yield return null;
        }

        // La scène est prête à être activée → on smooth jusqu’à 100%
        while (displayedProgress < 1f)
        {
            displayedProgress = Mathf.MoveTowards(displayedProgress, 1f, Time.deltaTime * 1.2f);
            loadingBar.value = displayedProgress;

            yield return null;
        }

        Debug.Log("[Loader] Scene fully loaded → activating");

        // Pause optionnelle (esthétique)
        yield return new WaitForSeconds(10f);

        // Activation de la scène
        asyncLoad.allowSceneActivation = true;
    }
}
