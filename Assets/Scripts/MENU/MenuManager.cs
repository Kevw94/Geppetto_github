using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuManager : MonoBehaviour
{
	public GameObject playButton;
    public GameObject quitButton;
	public GameObject returnButton;
	public GameObject commandsButton;
	public GameObject commandsPanel;
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

	public void ShowCommands()
	{
		Debug.Log("Show Commands");
		if (playButton != null) playButton.SetActive(false);
        if (quitButton != null) quitButton.SetActive(false);
		if (returnButton != null) returnButton.SetActive(true);
		if (commandsPanel != null) commandsPanel.SetActive(true);
		if (commandsButton != null) commandsButton.SetActive(false);

	}

	public void ReturnToMenu()
	{
		playButton.SetActive(true);
		quitButton.SetActive(true);
		returnButton.SetActive(false);
		commandsPanel.SetActive(false);
		commandsButton.SetActive(true);
	}
}
