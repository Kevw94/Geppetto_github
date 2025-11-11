using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        // Charger la scène du joueur XR (si pas déjà chargée)
        if (!SceneManager.GetSceneByName("PlayerXRScene").isLoaded)
        {
            SceneManager.LoadScene("PlayerXRScene", LoadSceneMode.Additive);
        }
    }

    public void PlayGame()
    {
        // Charger la scène du jeu (City)
        SceneManager.LoadScene("City", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
