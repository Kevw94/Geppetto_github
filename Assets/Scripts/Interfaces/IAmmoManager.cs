using System;

namespace MikeNspired.XRIStarterKit
{
    /// <summary>
    /// Interface for managing ammunition in the game.
    /// </summary>
    public interface IAmmoManager
    {
        /// <summary>
        /// Event triggered when ammo count changes.
        /// </summary>
        event Action<int> OnAmmoChanged;

        /// <summary>
        /// Try to consume ammo from the manager.
        /// </summary>
        /// <param name="requested">The amount of ammo requested.</param>
        /// <param name="given">The actual amount of ammo given (out parameter).</param>
        /// <returns>True if any ammo was given, false otherwise.</returns>
        bool TryConsumeAmmo(int requested, out int given);

        /// <summary>
        /// Notify listeners that the ammo count has changed.
        /// </summary>
        void NotifyAmmoChanged();

        /// <summary>
        /// Get the current ammo count.
        /// </summary>
        /// <returns>The total ammo available.</returns>
        int GetAmmo();

        /// <summary>
        /// Add ammo to the manager.
        /// </summary>
        /// <param name="amount">The amount of ammo to add.</param>
        void AddAmmo(int amount);
    }
}
