using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;



public class VRLoadingManager : MonoBehaviour
{
	public Slider loadingBar;
	private string sceneToLoad;
	private float displayedProgress = 0f;

	void Start()
	{
		sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
		StartCoroutine(LoadSceneAsync());
	}

	IEnumerator LoadSceneAsync()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
		asyncLoad.allowSceneActivation = false;

		while (asyncLoad.progress < 0.9f)
		{
			float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

			displayedProgress = Mathf.Lerp(displayedProgress, targetProgress, Time.deltaTime * 3f);

			loadingBar.value = displayedProgress;

			yield return null;
		}

		while (displayedProgress < 1f)
		{
			displayedProgress = Mathf.Lerp(displayedProgress, 1f, Time.deltaTime * 2f);
			loadingBar.value = displayedProgress;
			yield return null;
		}

		yield return new WaitForSeconds(0.5f);
		asyncLoad.allowSceneActivation = true;
	}

}
