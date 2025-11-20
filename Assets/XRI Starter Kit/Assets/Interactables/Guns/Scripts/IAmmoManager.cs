using System;

namespace MikeNspired.XRIStarterKit
{
    /// <summary>
    /// Lightweight abstraction so starter kit code does not rely on project specific ammo classes.
    /// </summary>
    public interface IAmmoManager
    {
        bool TryConsumeAmmo(int requested, out int given);
        event Action<int> OnAmmoChanged;
        int GetAmmo();
        void AddAmmo(int amount);
        void NotifyAmmoChanged();
        bool HasReceivedAmmo { get; }
    }

    public static class AmmoManagerLocator
    {
        public static IAmmoManager Instance { get; private set; }

        public static void Register(IAmmoManager manager)
        {
            Instance = manager;
        }

        public static void Unregister(IAmmoManager manager)
        {
            if (Instance == manager)
                Instance = null;
        }
    }
}
