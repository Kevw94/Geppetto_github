
using UnityEngine;

public class AmbientLoopController : MonoBehaviour
{
    public AudioSource audioSource;

    [Tooltip("Time (in seconds) where looping should begin")]
    public float loopStartTime = 10.2f;

    [Tooltip("Time (in seconds) where the track ends")]
    public float loopEndTime = 59f;

    void Update()
    {
        if (audioSource.isPlaying && audioSource.time >= loopEndTime)
        {
            audioSource.time = loopStartTime;
        }
    }
}
