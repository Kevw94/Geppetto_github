using System.Collections;
using UnityEngine;

public class KeypadDoorLock_Central : MonoBehaviour
{
    [Header("Code et saisie")]
    [SerializeField] private string correctCode = "0609";
    private string currentCode = "";

    [Header("Porte")]
    [SerializeField] private DoorsOpening doorToUnlock;

    [Header("Sphères LED")]
    [SerializeField] private GameObject redSphere;
    [SerializeField] private GameObject greenSphere;

    [Header("Sons")]
    [SerializeField] private AudioSource digitBeep;
    [SerializeField] private AudioSource successBeep;
    [SerializeField] private AudioSource failBeep;

    [Header("Quads (chiffres)")]
    [SerializeField] private GameObject[] digitQuads;

    private void Start()
    {

        if (redSphere != null) redSphere.SetActive(false);
        if (greenSphere != null) greenSphere.SetActive(false);

        foreach (var quad in digitQuads)
        {

            if (!quad.TryGetComponent(out Collider col))
            {
                col = quad.AddComponent<BoxCollider>();
            }

            col.isTrigger = true;

            if (!quad.TryGetComponent(out KeypadQuadTrigger trig))
            {
                trig = quad.AddComponent<KeypadQuadTrigger>();
            }

            trig.Init(this, quad.name);
        }

    }

    public void PressDigit(string digit)
    {

        digitBeep?.Play();

        currentCode += digit;

        if (currentCode.Length >= correctCode.Length)
        {
            if (currentCode == correctCode)
            {
                successBeep?.Play();

                if (greenSphere != null) greenSphere.SetActive(true);
                if (redSphere != null) redSphere.SetActive(false);

                doorToUnlock?.UnlockDoor();
            }
            else
            {
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
