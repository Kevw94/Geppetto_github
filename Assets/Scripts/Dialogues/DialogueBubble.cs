using System.Collections;
using UnityEngine;
using TMPro;

public class VRDialogueBubble : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public CanvasGroup cg;
    public float fadeSpeed = 2f;
    public float typeSpeed = 0.03f;

    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        cg.alpha = 0f;
    }

    void Update()
    {
        // Suit le regard du joueur
        transform.LookAt(cam);
        transform.forward = -transform.forward;
    }

    // Version publique avec durée d'affichage
    public void ShowDialogue(string message, float displayTime = 5f)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayDialogue(message, displayTime));
    }

    private IEnumerator DisplayDialogue(string message, float displayTime)
    {
        // Fade in
        while (cg.alpha < 1f)
        {
            cg.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Typewriter effect
        dialogueText.text = "";
        foreach (char c in message)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        // Attendre la durée définie
        yield return new WaitForSeconds(displayTime);

        // Fade out
        while (cg.alpha > 0f)
        {
            cg.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    public void HideDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while (cg.alpha > 0f)
        {
            cg.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}
