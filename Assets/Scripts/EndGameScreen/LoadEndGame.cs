using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoadEndGame : MonoBehaviour
{
    [Header("Scene & Delay")]
    public string sceneName = "EndGameScreen";
    public float delayAfterFade = 1f;

    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 2f;

    [Header("Audio")]
    public AudioSource RescueIncoming;

    private bool hasTriggered = false;
    private XRBaseInteractable interactable;

    private void Awake()
    {
        // Récupère l'interactable et abonne la fonction
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnInteract);

        // Assure que le Canvas/Panel est désactivé au départ
        if (fadeImage != null)
            fadeImage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnInteract);
    }

    private void OnInteract(SelectEnterEventArgs args)
    {
        if (hasTriggered) return;
        hasTriggered = true;
        RescueIncoming.Play();

        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (fadeImage != null)
        {
            // Active le panel au moment du fade
            fadeImage.gameObject.SetActive(true);

            // Initialise alpha à 0
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;

            // Fade progressif vers alpha = 1
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
            c.a = 1f;
            fadeImage.color = c;
        }

        // Attente supplémentaire si besoin
        yield return new WaitForSeconds(delayAfterFade);

        // Chargement de la scène
        SceneManager.LoadScene(sceneName);
    }
}
