using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HaileyHealth : MonoBehaviour
{
    [Header("Paramètres de vie")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Circulaire")]
    public Image HP;
    public TextMeshProUGUI HPText;

    [Header("Effet de clignotement")]
    public float lowHealthThreshold = 20f;
    public float flashSpeed = 3f;
    public Color normalColor = Color.green;
    public Color flashColor = Color.red;

    private bool isFlashing = false;

    void Start()
    {
        // currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (currentHealth <= lowHealthThreshold && !isFlashing)
        {
            StartCoroutine(FlashEffect());
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (HP != null)
        {
            HP.fillAmount = currentHealth / maxHealth;

            if (currentHealth > 50)
            {
                HP.color = Color.green;
            }
            else if (currentHealth > lowHealthThreshold)
            {
                HP.color = new Color(1f, 0.65f, 0f);
            }
        }

        if (HPText != null)
        {
            HPText.text = Mathf.CeilToInt(currentHealth).ToString();
        }
    }

    private IEnumerator FlashEffect()
    {
        isFlashing = true;

        while (currentHealth <= lowHealthThreshold)
        {
            if (HP != null)
            {
                float t = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed));
                HP.color = Color.Lerp(flashColor, normalColor, t);
            }
            yield return null;
        }

        if (HP != null)
            HP.color = normalColor;

        isFlashing = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateHealthUI();
    }
#endif

    private void Die()
    {
        Debug.Log("Le joueur est mort !");
    }
}
