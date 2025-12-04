using UnityEngine;
using System;

/// <summary>
/// Manages health for enemy entities.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Parameters")]
    public float maxHealth = 100f;
    private float currentHealth;

    /// <summary>
    /// Event triggered when the enemy takes damage.
    /// </summary>
    public event Action<float> OnTakeDamage;

    /// <summary>
    /// Event triggered when the enemy dies.
    /// </summary>
    public event Action OnDeath;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Apply damage to this enemy.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnTakeDamage?.Invoke(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Get the current health value.
    /// </summary>
    /// <returns>The current health.</returns>
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Get the maximum health value.
    /// </summary>
    /// <returns>The maximum health.</returns>
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// Get the health as a normalized value (0-1).
    /// </summary>
    /// <returns>Health normalized to 0-1 range.</returns>
    public float GetHealthNormalized()
    {
        return maxHealth > 0 ? currentHealth / maxHealth : 0f;
    }

    /// <summary>
    /// Check if the enemy is dead.
    /// </summary>
    /// <returns>True if dead, false otherwise.</returns>
    public bool IsDead()
    {
        return isDead;
    }

    /// <summary>
    /// Handle the death of this enemy.
    /// </summary>
    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        OnDeath?.Invoke();
    }

    /// <summary>
    /// Restore health to maximum.
    /// </summary>
    public void Heal()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    /// <summary>
    /// Restore a specific amount of health.
    /// </summary>
    /// <param name="amount">The amount to heal.</param>
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}
