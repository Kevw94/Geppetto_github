namespace MikeNspired.XRIStarterKit
{
    /// <summary>
    /// Service locator for accessing the IAmmoManager instance.
    /// </summary>
    public static class AmmoManagerLocator
    {
        private static IAmmoManager instance;

        /// <summary>
        /// Get the current IAmmoManager instance.
        /// </summary>
        public static IAmmoManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Register an IAmmoManager instance.
        /// </summary>
        /// <param name="ammoManager">The ammo manager to register.</param>
        public static void Register(IAmmoManager ammoManager)
        {
            instance = ammoManager;
        }

        /// <summary>
        /// Unregister the current IAmmoManager instance.
        /// </summary>
        /// <param name="ammoManager">The ammo manager to unregister (for safety check).</param>
        public static void Unregister(IAmmoManager ammoManager)
        {
            if (instance == ammoManager)
            {
                instance = null;
            }
        }

        /// <summary>
        /// Clear the current instance.
        /// </summary>
        public static void Clear()
        {
            instance = null;
        }
    }
}
