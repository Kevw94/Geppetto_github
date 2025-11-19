using UnityEngine;

public class DoorsOpening : MonoBehaviour
{
    [Header("État")]
    [SerializeField] private bool isClosed = true;
    [SerializeField] private bool isLocked = true;

    [Header("Sons")]
    [SerializeField] private AudioSource openSound;
    [SerializeField] private AudioSource closeSound;
    [SerializeField] private AudioSource lockedSound;

    private HingeJoint hinge;
    private Rigidbody rb;

    private void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();

        if (hinge == null)
            Debug.LogWarning("⚠ Aucun HingeJoint trouvé sur la porte !");
        if (rb == null)
            Debug.LogWarning("⚠ Aucun Rigidbody trouvé sur la porte !");
        else
            rb.isKinematic = true; // La porte reste fixe tant qu’elle est verrouillée
    }

    /// <summary>
    /// Appelée quand quelque chose tente d'interagir avec la porte (joueur, trigger, etc.)
    /// </summary>
    public void TryInteract(Transform interactor)
    {

        if (isLocked)
        {
            PlaySound(lockedSound);
            return;
        }

        if (isClosed)
            OpenDoor();
        else
            CloseDoor();
    }

    /// <summary>
    /// Déverrouille la porte
    /// </summary>
    public void UnlockDoor()
    {
        isLocked = false;

        if (rb != null)
            rb.isKinematic = false; // Autorise le mouvement une fois déverrouillée
    }

    /// <summary>
    /// Verrouille la porte
    /// </summary>
    public void LockDoor(bool locked)
    {
        isLocked = locked;

        if (rb != null)
            rb.isKinematic = locked; // Reste fixe si verrouillée
    }

    private void OpenDoor()
    {
        isClosed = false;

        if (hinge != null)
        {
            JointLimits l = hinge.limits;
            l.min = -90f;
            l.max = 90f;
            hinge.limits = l;
        }

        PlaySound(openSound);
    }

    private void CloseDoor()
    {
        isClosed = true;

        if (hinge != null)
        {
            JointLimits l = hinge.limits;
            l.min = 0f;
            l.max = 0f;
            hinge.limits = l;
        }

        PlaySound(closeSound);
    }

    private void PlaySound(AudioSource source)
    {
        if (source != null)
            source.Play();
    }
}
