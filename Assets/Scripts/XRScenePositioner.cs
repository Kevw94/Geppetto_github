using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using System.Collections;

public class XRScenePositioner : MonoBehaviour
{
    [System.Serializable]
    public class SceneSpawn
    {
        public string sceneName;
        public string spawnObjectName;
    }

    [Header("Configuration des spawns")]
    public SceneSpawn[] spawns;

    [Header("Debug")]
    public bool logDebug = true;

    private XROrigin xrOrigin;

    private void Awake()
    {
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

    private IEnumerator Start()
    {
        // Laisse le temps à OpenXR d'initialiser
        yield return new WaitForSeconds(0.5f);
        xrOrigin = GetComponent<XROrigin>();
        if (xrOrigin == null)
        {
            Debug.LogError("[XRScenePositioner] ❌ Aucun XROrigin trouvé sur cet objet !");
            yield break;
        }

        TryPositionForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(PositionAfterLoad(scene.name));
    }

    private IEnumerator PositionAfterLoad(string sceneName)
    {
        // Attendre un peu que les objets de la nouvelle scène soient bien instanciés
        yield return new WaitForSeconds(0.5f);
        TryPositionForScene(sceneName);
    }

    private void TryPositionForScene(string sceneName)
    {
        if (xrOrigin == null)
        {
            Debug.LogWarning("[XRScenePositioner] ⚠️ XROrigin manquant !");
            return;
        }

        foreach (var s in spawns)
        {
            if (s.sceneName == sceneName)
            {
                GameObject spawnGo = GameObject.Find(s.spawnObjectName);

                if (spawnGo == null)
                {
                    if (logDebug)
                        Debug.LogWarning($"[XRScenePositioner] ⏳ Spawn '{s.spawnObjectName}' introuvable dans '{sceneName}' (sera réessayé).");

                    // Relance une vérif un peu plus tard (utile pour Quest)
                    StartCoroutine(RetryFindSpawn(s.spawnObjectName));
                    return;
                }

                MoveToSpawn(spawnGo.transform, sceneName);
                return;
            }
        }

        if (logDebug)
            Debug.Log($"[XRScenePositioner] ℹ️ Aucun spawn configuré pour '{sceneName}'.");
    }

    private IEnumerator RetryFindSpawn(string spawnName)
    {
        yield return new WaitForSeconds(0.5f);
        var spawnGo = GameObject.Find(spawnName);
        if (spawnGo != null)
            MoveToSpawn(spawnGo.transform, SceneManager.GetActiveScene().name);
    }

    private void MoveToSpawn(Transform spawn, string sceneName)
    {
        xrOrigin.MoveCameraToWorldLocation(spawn.position);
        xrOrigin.transform.rotation = spawn.rotation;

        if (logDebug)
            Debug.Log($"[XRScenePositioner] ✅ Rig déplacé sur '{spawn.name}' (scène : {sceneName}).");
    }
}
