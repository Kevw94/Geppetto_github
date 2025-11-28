using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject quitButton;
    public GameObject returnButton;
    public GameObject commandsButton;
    public GameObject commandsPanel;

    public AudioSource ButtonPressed;

    public void PlayGame()
    {
        ButtonPressed.Play();
        PlayerPrefs.SetString("SceneToLoad", "City");
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        ButtonPressed.Play();
        Application.Quit();
    }

    public void ShowCommands()
    {
        ButtonPressed.Play();
        commandsPanel.SetActive(true);
    }

    public void ReturnToMenu()
    {
        ButtonPressed.Play();
        commandsPanel.SetActive(false);
    }
}
