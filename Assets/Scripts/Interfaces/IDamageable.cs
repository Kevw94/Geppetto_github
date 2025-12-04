using UnityEngine;

/// <summary>
/// Interface for objects that can take damage.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Apply damage to this object.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    /// <param name="damageSource">The GameObject that caused the damage.</param>
    void TakeDamage(float damage, GameObject damageSource);
}
