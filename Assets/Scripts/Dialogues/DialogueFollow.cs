using UnityEngine;

public class DialogueFollow : MonoBehaviour
{
    public Transform playerCamera; // la caméra du XR Origin
    public Vector3 offset = new Vector3(0, 0.1f, 1.5f); // 1.5 m devant, 0.1 m au-dessus
    public float followSpeed = 5f; // vitesse de déplacement lissé

    void LateUpdate()
    {
        if (playerCamera == null) return;

        // Position : devant le joueur avec offset
        Vector3 targetPos = playerCamera.position + playerCamera.TransformDirection(offset);

        // Lissage du mouvement pour éviter le “jerk”
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        // Toujours orientée vers la caméra
        Vector3 lookDir = transform.position - playerCamera.position;
        lookDir.y = 0; // si tu veux que la boîte reste droite
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * followSpeed);
    }
}
