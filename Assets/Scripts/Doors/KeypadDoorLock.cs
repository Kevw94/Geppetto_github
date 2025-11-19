using System.Collections;
using UnityEngine;

public class KeypadDoorLock_Central : MonoBehaviour
{
    [Header("Code et saisie")]
    [SerializeField] private string correctCode = "1589";
    private string currentCode = "";

    [Header("Porte")]
    [SerializeField] private DoorsOpening doorToUnlock;

    [Header("Sphères LED")]
    [SerializeField] private GameObject redSphere;   // devient rouge ON/OFF
    [SerializeField] private GameObject greenSphere; // devient verte ON/OFF

    [Header("Sons")]
    [SerializeField] private AudioSource digitBeep;
    [SerializeField] private AudioSource successBeep;
    [SerializeField] private AudioSource failBeep;

    [Header("Quads (chiffres)")]
    [SerializeField] private GameObject[] digitQuads;

    private void Start()
    {
        Debug.Log("=== Keypad Setup Start ===");

        // On désactive les LED au début
        if (redSphere != null) redSphere.SetActive(false);
        if (greenSphere != null) greenSphere.SetActive(false);

        foreach (var quad in digitQuads)
        {
            Debug.Log("Préparation du quad : " + quad.name);

            // Ajout collider si besoin
            if (!quad.TryGetComponent(out Collider col))
            {
                col = quad.AddComponent<BoxCollider>();
                Debug.LogWarning("BoxCollider ajouté automatiquement à " + quad.name);
            }

            col.isTrigger = true;

            // Ajout / récupération du trigger
            if (!quad.TryGetComponent(out KeypadQuadTrigger trig))
            {
                trig = quad.AddComponent<KeypadQuadTrigger>();
                Debug.Log($"KeypadQuadTrigger ajouté à {quad.name}");
            }

            trig.Init(this, quad.name);
        }

        Debug.Log("=== Keypad Setup Complete ===");
    }

    public void PressDigit(string digit)
    {
        Debug.Log("DIGIT PRESSÉ : " + digit);

        digitBeep?.Play();

        currentCode += digit;

        if (currentCode.Length >= correctCode.Length)
        {
            if (currentCode == correctCode)
            {
                Debug.Log("CODE CORRECT !");
                successBeep?.Play();

                if (greenSphere != null) greenSphere.SetActive(true);
                if (redSphere != null) redSphere.SetActive(false);

                doorToUnlock?.UnlockDoor();
            }
            else
            {
                Debug.Log("CODE FAUX !");
                failBeep?.Play();

                if (redSphere != null) redSphere.SetActive(true);
                if (greenSphere != null) greenSphere.SetActive(false);
            }

            StartCoroutine(ResetCode());
        }
    }

    private IEnumerator ResetCode()
    {
        yield return new WaitForSeconds(1.2f);

        currentCode = "";

        if (redSphere != null) redSphere.SetActive(false);
        if (greenSphere != null) greenSphere.SetActive(false);
    }
}

public class KeypadQuadTrigger : MonoBehaviour
{
    private KeypadDoorLock_Central keypad;
    private string digit;

    public void Init(KeypadDoorLock_Central k, string d)
    {
        keypad = k;
        digit = d;
    }

    private void OnTriggerEnter(Collider other)
    {
        keypad.PressDigit(digit);
    }
}
