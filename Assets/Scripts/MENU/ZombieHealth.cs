using UnityEngine;
using UnityEngine.SceneManagement;
using MikeNspired.XRIStarterKit;

public class ZombieHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 30;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, GameObject damageSource)
    {
        currentHealth -= Mathf.RoundToInt(damage);
        Debug.Log("Zombie hit! HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Zombie killed!");
        SceneManager.LoadScene("City", LoadSceneMode.Single);
    }
}
