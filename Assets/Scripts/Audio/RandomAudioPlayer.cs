using UnityEngine;
using System.Collections;

public class RandomAudioPlayer : MonoBehaviour
{
    [Header("Réglages du délai aléatoire (en secondes)")]
    public float minDelay = 2f;
    public float maxDelay = 5f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        StartCoroutine(PlayRandomly());
    }

    private IEnumerator PlayRandomly()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            audioSource.Play();
        }
    }
}
