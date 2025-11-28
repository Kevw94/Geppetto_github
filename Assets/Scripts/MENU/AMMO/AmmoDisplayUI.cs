using TMPro;
using UnityEngine;
using MikeNspired.XRIStarterKit;

public class AmmoDisplayUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;

    private void Start()
    {
        if (AmmoManagerLocator.Instance != null)
        {
            UpdateAmmo(AmmoManagerLocator.Instance.GetAmmo());
            AmmoManagerLocator.Instance.OnAmmoChanged += UpdateAmmo;
        }
    }

    private void UpdateAmmo(int newAmount)
    {
        ammoText.text = $"{newAmount}";
    }

    private void OnDestroy()
    {
        if (AmmoManagerLocator.Instance != null)
            AmmoManagerLocator.Instance.OnAmmoChanged -= UpdateAmmo;
    }
}
