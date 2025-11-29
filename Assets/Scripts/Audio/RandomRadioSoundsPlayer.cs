using UnityEngine;
using System.Collections;

public class RandomOverlaySounds : MonoBehaviour
{
    [Header("Main Looping Sound")]
    public AudioSource loopSource; // Ton son en boucle

    [Header("Random Sounds")]
    public AudioSource[] randomSounds; // liste de petits sons à jouer par-dessus

    [Header("Random Interval")]
    public float minInterval = 3f;
    public float maxInterval = 10f;

    private void Start()
    {
        // Lance le son principal en boucle si défini
        if (loopSource != null && !loopSource.isPlaying)
        {
            loopSource.loop = true;
            loopSource.Play();
        }

        // Lance la coroutine pour les sons aléatoires
        StartCoroutine(PlayRandomSounds());
    }

    private IEnumerator PlayRandomSounds()
    {
        while (true)
        {
            // Attente aléatoire
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // Choisir un son aléatoire
            if (randomSounds.Length > 0)
            {
                int index = Random.Range(0, randomSounds.Length);
                AudioSource sound = randomSounds[index];

                if (sound != null)
                    sound.Play();
            }
        }
    }
}
