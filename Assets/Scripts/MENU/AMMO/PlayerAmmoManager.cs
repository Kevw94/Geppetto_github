using UnityEngine;
using System;

namespace MikeNspired.XRIStarterKit
{
	public class PlayerAmmoManager : MonoBehaviour, IAmmoManager
	{
		public int totalAmmo = 0;
		public static PlayerAmmoManager Instance;

		public event Action<int> OnAmmoChanged;

		private bool hasReceivedAmmo;

		private void Awake()
		{
			Instance = this;
			AmmoManagerLocator.Register(this);
		}

		private void OnDestroy()
		{
			if (Instance == this)
				Instance = null;
			AmmoManagerLocator.Unregister(this);
		}

		public bool TryConsumeAmmo(int requested, out int given)
		{
			Debug.Log($"[AmmoManager] Requesting: {requested}, BeforeTotal: {totalAmmo}");
			given = Mathf.Min(requested, totalAmmo);
			totalAmmo -= given;
			Debug.Log($"[AmmoManager] Given: {given}, AfterTotal: {totalAmmo}");
			NotifyAmmoChanged();
			return given > 0;
		}

		public void NotifyAmmoChanged()
		{
			OnAmmoChanged?.Invoke(totalAmmo);
		}

		public int GetAmmo() => totalAmmo;
		public bool HasReceivedAmmo => hasReceivedAmmo;

		public void AddAmmo(int amount)
		{
			totalAmmo += amount;
			hasReceivedAmmo = totalAmmo > 0;
			NotifyAmmoChanged();
		}

	}


}
