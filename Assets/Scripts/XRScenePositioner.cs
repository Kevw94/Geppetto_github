using UnityEngine;
using UnityEngine.SceneManagement;

public class XRScenePositioner : MonoBehaviour
{
    [System.Serializable]
    public class SceneSpawn
    {
        public string sceneName;        // Nom exact de la scène (ex: "MainMenuLobby")
        public string spawnObjectName;  // Nom du GameObject spawn dans cette scène (ex: "Spawn_Menu")
    }

    [Header("Configuration des spawns")]
    public SceneSpawn[] spawns;

    [Header("Debug")]
    public bool logDebug = true;

    private void Awake()
    {
        // On garde le rig entre les scènes
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Gère le cas où la scène actuelle a déjà un spawn
        TryPositionForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryPositionForScene(scene.name);
    }

    private void TryPositionForScene(string sceneName)
    {
        foreach (var s in spawns)
        {
            if (s.sceneName == sceneName)
            {
                var spawnGo = GameObject.Find(s.spawnObjectName);

                if (spawnGo != null)
                {
                    transform.SetPositionAndRotation(spawnGo.transform.position, spawnGo.transform.rotation);
                    if (logDebug)
                        Debug.Log($"[XRScenePositioner] Moved rig to '{s.spawnObjectName}' in scene '{sceneName}'.");
                }
                else
                {
                    if (logDebug)
                        Debug.LogWarning($"[XRScenePositioner] spawn '{s.spawnObjectName}' not found in scene '{sceneName}'.");
                }

                return;
            }
        }

        if (logDebug)
            Debug.Log($"[XRScenePositioner] No spawn configured for scene '{sceneName}'.");
    }
}
