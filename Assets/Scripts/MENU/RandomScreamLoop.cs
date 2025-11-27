using UnityEngine;
using System.Collections;

public class RandomScreamLoop : MonoBehaviour
{
    public AudioSource screamAudioSource;
    public float initialDelayMin = 5f;
    public float initialDelayMax = 10f;
    public float repeatDelayMin = 1f;
    public float repeatDelayMax = 5f;

    private void Start()
    {
        StartCoroutine(PlayScreamsWithRandomDelay());
    }

    private IEnumerator PlayScreamsWithRandomDelay()
    {
        float initialDelay = Random.Range(initialDelayMin, initialDelayMax);
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            screamAudioSource.Play();

            yield return new WaitForSeconds(screamAudioSource.clip.length);

            float waitTime = Random.Range(repeatDelayMin, repeatDelayMax);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
