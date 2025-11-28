using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;


public class LoadEndGame : MonoBehaviour
{
    public string sceneName = "EndGameScreen";   // Nom de la sc�ne � charger
    public float delay = 5f;               // Temps d'attente avant le chargement

    private bool hasTriggered = false;     // Pour �viter plusieurs d�clenchements

    private void Awake()
    {
        // R�cup�re automatiquement l'interactable
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        // Abonne la fonction � l'�v�nement d'interaction
        interactable.selectEntered.AddListener(OnInteract);
    }

    private void OnInteract(SelectEnterEventArgs args)
    {
        if (hasTriggered) return;
        hasTriggered = true;

        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}
