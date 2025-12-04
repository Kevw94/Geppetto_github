using UnityEngine;

/// <summary>
/// Interface for enemy entities in the game.
/// </summary>
public interface IEnemy
{
    /// <summary>
    /// Deal damage to the player or target.
    /// </summary>
    void DealDamage();

    /// <summary>
    /// Handle the death of this enemy.
    /// </summary>
    void Die();

    /// <summary>
    /// Play a scream sound event.
    /// </summary>
    void PlayScreamEvent();
}
