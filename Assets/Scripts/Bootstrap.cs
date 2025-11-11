using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject[] persistentObjects;

    void Awake()
    {
        // Tout ce qui doit survivre (XR Rig, XR Interaction Manager, etc.)
        foreach (var go in persistentObjects)
        {
            if (go != null)
                DontDestroyOnLoad(go);
        }

        // Charge le menu en premier
        SceneManager.LoadScene("MainMenuLobby", LoadSceneMode.Single);
    }
}
