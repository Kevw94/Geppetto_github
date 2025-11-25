using UnityEngine;

public class TvHandler : MonoBehaviour
{
    [Header("GameObject à activer/désactiver")]
    public GameObject tv;

    // Fonction à appeler depuis l'événement Activate de l'XRGrabInteractable
    public void ToggleTv()
    {
        if (tv != null)
        {
            tv.SetActive(!tv.activeSelf);
        }
        else
        {
            Debug.LogWarning("Aucun GameObject assigné à TVController !");
        }
    }
}
