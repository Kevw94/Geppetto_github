using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DialogueTrigger : MonoBehaviour
{
    public VRDialogueBubble bubble;
    [TextArea] public string message;
    public float displayTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<XROrigin>())
        {
            bubble.ShowDialogue(message, displayTime);
        }
    }
}
