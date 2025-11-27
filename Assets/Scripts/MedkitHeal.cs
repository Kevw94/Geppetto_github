using UnityEngine;
using System.Collections;

public class MedkitHeal : MonoBehaviour
{
    [Header("Paramètres du kit de soin")]
    public float healAmount = 25f;
    public float useDuration = 2f;

    [Header("Audio")]
    public AudioSource useSound;

    [Header("Référence du joueur")]
    public HaileyHealth playerHealth;

    [Header("Input (nom du bouton)")]
    public string healButton = "JoystickButton1";
    // B sur Oculus / VR

    private bool isHealing = false;

    void Update()
    {
        if (playerHealth == null) return;

        // Si le joueur maintient et qu'on n'est pas déjà en train de soigner
        if (Input.GetButton(healButton) && !isHealing)
        {
            StartCoroutine(HealRoutine());
        }
    }

    private IEnumerator HealRoutine()
    {
        isHealing = true;
        float timer = 0f;

        if (useSound != null)
            useSound.Play();

        while (timer < useDuration)
        {
            // Annule si le joueur lâche le bouton
            if (!Input.GetButton(healButton))
            {
                Debug.Log("Soin annulé !");
                isHealing = false;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Applique le soin
        playerHealth.Heal(healAmount);
        Debug.Log("Soin réussi ! +" + healAmount + " HP");

        Destroy(gameObject); // Le medkit se détruit
    }
}
