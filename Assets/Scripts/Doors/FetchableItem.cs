using UnityEngine;

public class FetchableItem : MonoBehaviour
{
    [Header("References")]
    public CopperBehaviour copper;
    public Transform player;
    public float throwForce = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = true;
        rb.isKinematic = false;
    }

    void Update()
    {
        // Pour test : lancer l'objet avec la touche F
        if (Input.GetKeyDown(KeyCode.F))
        {
            ThrowTowardsCopper();
        }
    }

    public void ThrowTowardsCopper()
    {
        if (copper == null) return;

        // On enl�ve la parent� si l�objet �tait port�
        transform.SetParent(null);

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;

        Vector3 direction = (copper.transform.position - transform.position).normalized;
        rb.AddForce(direction * throwForce, ForceMode.VelocityChange);

        // Pr�venir Copper de venir chercher l�objet
        copper.ThrowObject(transform);
    }
}
