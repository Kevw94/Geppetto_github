using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderDebug : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} a détecté un OnTriggerEnter avec {other.name}");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"{gameObject.name} est en contact avec {other.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"{gameObject.name} a détecté un OnTriggerExit avec {other.name}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{gameObject.name} a détecté un OnCollisionEnter avec {collision.gameObject.name}");
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log($"{gameObject.name} est en collision avec {collision.gameObject.name}");
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log($"{gameObject.name} a détecté un OnCollisionExit avec {collision.gameObject.name}");
    }
}
