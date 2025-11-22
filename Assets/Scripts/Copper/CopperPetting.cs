using UnityEngine;

public class CopperPetting : MonoBehaviour
{
    [Header("References")]
    public Animator dogAnimator;
    public AudioSource happySoundSource;
    public AudioClip happyClip;

    [Header("Settings")]
    public string Layer = "PokeOnly";
    public float happyDuration = 2f;

    private bool isBeingPetted = false;
    private int pokeLayer;

    void Start()
    {
        pokeLayer = LayerMask.NameToLayer(Layer);
        if (pokeLayer == -1)
            Debug.LogError("Le layer " + Layer + " n'existe pas !");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isBeingPetted) return;

        if (other.gameObject.layer == pokeLayer)
        {
            StartCoroutine(PetRoutine());
        }
    }

    private System.Collections.IEnumerator PetRoutine()
    {
        isBeingPetted = true;

        // Animation happy
        if (dogAnimator != null)
            dogAnimator.SetInteger("ActionType_int", 11); // happy

        // Son happy
        if (happySoundSource != null && happyClip != null)
            happySoundSource.PlayOneShot(happyClip);

        yield return new WaitForSeconds(happyDuration);

        if (dogAnimator != null)
            dogAnimator.SetInteger("ActionType_int", 0);

        isBeingPetted = false;
    }
}
