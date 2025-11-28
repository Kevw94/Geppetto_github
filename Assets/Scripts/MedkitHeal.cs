using UnityEngine;
using System.Collections;

public class Medkit : MonoBehaviour
{
    [Header("Healing Settings")]
    public float healAmount = 25f;
    public float healDuration = 2f;

    [Header("FX")]
    public ParticleSystem healFX;
    public AudioSource healLoopSound;

    private bool isHealing = false;
    private HaileyHealth hailey;
    private bool healCompleted = false;

    private void Start()
    {
        hailey = FindAnyObjectByType<HaileyHealth>();
    }

    public void StartHealing()
    {
        if (!isHealing && hailey != null)
            StartCoroutine(HealProcess());
    }

    public void StopHealing()
    {
        isHealing = false;

        if (healFX != null)
            healFX.Stop();

        if (healLoopSound != null)
            healLoopSound.Stop();

        if (healCompleted)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator HealProcess()
    {
        isHealing = true;
        healCompleted = false; // reset

        if (healFX != null)
            healFX.Play();

        if (healLoopSound != null)
            healLoopSound.Play();

        float timer = 0f;

        while (timer < healDuration && isHealing)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (isHealing)
        {
            hailey.Heal(healAmount);
            healCompleted = true; // ← ici on indique que le heal s'est terminé
        }

        StopHealing();
    }

}
