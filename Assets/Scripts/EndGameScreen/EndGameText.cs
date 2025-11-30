using UnityEngine;
using TMPro;

public class TMPFadeAndSwitch : MonoBehaviour
{
    public TMP_Text textToFade;
    public GameObject objectToEnable;
    public float fadeDuration = 5f;

    private void Start()
    {
        if (textToFade != null)
            StartCoroutine(FadeOutAndSwitch());
    }

    private System.Collections.IEnumerator FadeOutAndSwitch()
    {
        Color startColor = textToFade.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            textToFade.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }

        textToFade.gameObject.SetActive(false);

        if (objectToEnable != null)
            objectToEnable.SetActive(true);
    }
}
