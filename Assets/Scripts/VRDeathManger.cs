using UnityEngine;
using System.Collections;

public class VRDeathManager : MonoBehaviour
{
    [Header("Death Screen Settings")]
    public CanvasGroup deathScreen;
    public GameObject gameOverText;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    [Header("VR Movement Controllers")]
    public MonoBehaviour[] scriptsToDisableOnDeath;

    private bool isDead = false;

    public void TriggerDeath()
    {
        if (isDead) return;
        isDead = true;

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        float t = 0;

        // Fade to black
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            deathScreen.alpha = alpha;
            yield return null;
        }

        deathScreen.alpha = 1;

        // Disable VR movement scripts (locomotion, teleports…)
        foreach (var script in scriptsToDisableOnDeath)
        {
            if (script != null)
                script.enabled = false;
        }

        // Show Game Over Text
        gameOverText?.SetActive(true);

        Debug.Log("🟥 GAME OVER – VR Death triggered");
    }
}
