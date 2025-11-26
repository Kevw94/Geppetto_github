using UnityEngine;

public class DialogueOnStart : MonoBehaviour
{
    [Header("Dialogue System")]
    public VRDialogueBubble bubble;
    [TextArea] public string startMessage;
    public float delayBeforeStart = 5f;
    public float displayTime = 5f; // Durée pendant laquelle le dialogue reste visible

    void Start()
    {
        // Lance le dialogue après le délai
        Invoke(nameof(ShowStartDialogue), delayBeforeStart);
    }

    void ShowStartDialogue()
    {
        // Affiche le dialogue et le fait disparaître automatiquement après displayTime secondes
        bubble.ShowDialogue(startMessage, displayTime);
    }
}
