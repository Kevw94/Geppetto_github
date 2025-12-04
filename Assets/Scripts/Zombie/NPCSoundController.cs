using UnityEngine;

/// <summary>
/// Manages sound effects for NPC entities like zombies.
/// </summary>
public class NPCSoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource impactAudioSource;
    [SerializeField] private AudioSource spawnAudioSource;
    [SerializeField] private AudioSource screamAudioSource;
    [SerializeField] private AudioSource deathAudioSource;
    [SerializeField] private AudioSource vocalAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] impactClips;
    [SerializeField] private AudioClip[] spawnClips;
    [SerializeField] private AudioClip[] screamClips;
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private AudioClip[] vocalClips;

    [Header("Settings")]
    [SerializeField] private float vocalPlayChance = 0.3f;
    private bool isRandomVocalEnabled = true;

    /// <summary>
    /// Play an impact/hit sound.
    /// </summary>
    public void PlayImpact()
    {
        PlayRandomClip(impactAudioSource, impactClips);
    }

    /// <summary>
    /// Play a spawn sound.
    /// </summary>
    public void PlaySpawn()
    {
        PlayRandomClip(spawnAudioSource, spawnClips);
    }

    /// <summary>
    /// Play a scream sound.
    /// </summary>
    public void PlayScream()
    {
        PlayRandomClip(screamAudioSource, screamClips);
    }

    /// <summary>
    /// Play a death sound.
    /// </summary>
    public void PlayDeath()
    {
        PlayRandomClip(deathAudioSource, deathClips);
    }

    /// <summary>
    /// Play a random vocal sound (if enabled).
    /// </summary>
    public void PlayRandomVocal()
    {
        if (!isRandomVocalEnabled)
            return;

        if (Random.value < vocalPlayChance)
        {
            PlayRandomClip(vocalAudioSource, vocalClips);
        }
    }

    /// <summary>
    /// Enable or disable random vocal sounds.
    /// </summary>
    /// <param name="enabled">True to enable, false to disable.</param>
    public void SetRandomVocalEnabled(bool enabled)
    {
        isRandomVocalEnabled = enabled;
    }

    /// <summary>
    /// Stop all audio playback.
    /// </summary>
    public void StopAllSounds()
    {
        if (impactAudioSource != null) impactAudioSource.Stop();
        if (spawnAudioSource != null) spawnAudioSource.Stop();
        if (screamAudioSource != null) screamAudioSource.Stop();
        if (deathAudioSource != null) deathAudioSource.Stop();
        if (vocalAudioSource != null) vocalAudioSource.Stop();
    }

    /// <summary>
    /// Play a random clip from the provided array.
    /// </summary>
    private void PlayRandomClip(AudioSource audioSource, AudioClip[] clips)
    {
        if (audioSource == null || clips == null || clips.Length == 0)
            return;

        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        if (randomClip != null)
        {
            audioSource.PlayOneShot(randomClip);
        }
    }
}
