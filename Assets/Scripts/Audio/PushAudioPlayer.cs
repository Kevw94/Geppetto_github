using MikeNspired.XRIStarterKit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRPushButton))]
[RequireComponent(typeof(AudioSource))]
public class PushAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void OnPush(ActivateEventArgs args)
    {
            audioSource.Play();
    }
}
