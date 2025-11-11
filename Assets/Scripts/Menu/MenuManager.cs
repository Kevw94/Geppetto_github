using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Transform spawnMenu; // ton point de spawn dans la scène MainMenuLobby

    private void Start()
    {
        StartCoroutine(InitializePlayerXR());
    }

    private IEnumerator InitializePlayerXR()
    {
        // 1️⃣ Charger la scène du joueur XR (si pas déjà chargée)
        if (!SceneManager.GetSceneByName("PlayerXRScene").isLoaded)
        {
            SceneManager.LoadScene("PlayerXRScene", LoadSceneMode.Additive);
        }

        // 2️⃣ Attendre que le rig XR soit prêt
        XROrigin xr = null;
        float timeout = 3f;
        float timer = 0f;

        while (xr == null && timer < timeout)
        {
            xr = FindObjectOfType<XROrigin>();
            timer += Time.deltaTime;
            yield return null;
        }

        // 3️⃣ Si trouvé → placer le joueur sur le Spawn_Menu
        if (xr != null && spawnMenu != null)
        {
            yield return new WaitForSeconds(0.1f); // petit délai pour laisser le tracking se stabiliser
            xr.MoveCameraToWorldLocation(spawnMenu.position);
            xr.transform.rotation = spawnMenu.rotation;

            Debug.Log($"✅ XR Player positionné sur {spawnMenu.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ Impossible de trouver le XR Origin ou le spawnMenu.");
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
