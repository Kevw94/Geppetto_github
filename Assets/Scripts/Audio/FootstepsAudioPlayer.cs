using UnityEngine;

public class FootstepsAudioPlayer : MonoBehaviour
{
    [Header("Footstep Settings")]
    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public float stepInterval = 0.5f;

    [Header("Movement Detection")]
    public CharacterController controller;
    public float speedThreshold = 0.2f;

    private float stepTimer;

    private void Update()
    {
        if (controller == null)
            return;

        float speed = controller.velocity.magnitude;

        // Le joueur se déplace ?
        if (speed > speedThreshold)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; // reset quand on arrête de bouger
        }
    }

    private void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0 || audioSource == null)
            return;

        // Choix aléatoire d'un pas
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

        audioSource.PlayOneShot(clip);
    }
}
