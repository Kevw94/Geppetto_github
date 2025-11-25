using UnityEngine;

public class DoorsOpening : MonoBehaviour
{
    [Header("État")]
    [SerializeField] private bool isLocked = true;

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
            rb.isKinematic = isLocked;
    }


    public void UnlockDoor()
    {
        isLocked = false;

        if (rb != null)
            rb.isKinematic = isLocked;
    }

    public void LockDoor(bool locked)
    {
        isLocked = locked;

        if (rb != null)
            rb.isKinematic = locked;
    }

}
