using UnityEngine;

public class CardReader : MonoBehaviour
{
    [Header("Porte et lumière")]
    public GameObject door;          // porte à déverrouiller
    public GameObject lightObject;   // lumière de signalisation
    public Material redLight;
    public Material greenLight;

    [Header("Son")]
    public AudioClip unlockBeep;     // bip quand la lumière devient verte
    private AudioSource audioSource;

    private bool isUnlocked = false;

    private void Start()
    {
        // Assigne le rouge au départ
        if (lightObject != null)
            lightObject.GetComponent<Renderer>().material = redLight;

        // Ajoute un AudioSource si inexistant
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isUnlocked && other.CompareTag("Card"))
        {
            UnlockDoor();
        }
    }

    private void UnlockDoor()
    {
        isUnlocked = true;

        // Déverrouille la porte via DoorsOpening
        if (door != null)
        {
            DoorsOpening doorScript = door.GetComponent<DoorsOpening>();
            if (doorScript != null)
                doorScript.UnlockDoor();
        }

        // Change la lumière
        if (lightObject != null)
            lightObject.GetComponent<Renderer>().material = greenLight;

        // Joue le bip
        if (unlockBeep != null && audioSource != null)
            audioSource.PlayOneShot(unlockBeep);

        Debug.Log("Porte déverrouillée et lumière verte !");
    }
}
