using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuManager : MonoBehaviour
{
	private void Start()
	{
	}
	private void Update()
	{
	}

	public void PlayGame()
	{
		SceneManager.LoadScene("City", LoadSceneMode.Single);
	}

	public void QuitGame()
	{
		Debug.Log("Quit game");
		Application.Quit();
	}
}
