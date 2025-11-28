using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(VRDeathManager))]
public class HaileyHealth : MonoBehaviour
{
    [Header("Health Parameters")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Circular UI Elements")]
    public Image HP;
    public TextMeshProUGUI HPText;

    [Header("Flash Effect Settings")]
    public float lowHealthThreshold = 20f;
    public float flashSpeed = 3f;
    public Color normalColor = Color.green;
    public Color warningColor = new Color(1f, 0.65f, 0f);
    public Color flashColor = Color.red;

    [Header("Audio")]
    public AudioSource damageSound;
    public AudioSource lowHealthLoop;

    [Header("VR Death Manager")]
    private VRDeathManager deathManager;
    private bool isDead = false;
    private bool isFlashing = false;
    private bool isLowHealthSoundPlaying = false;


    void Start()
    {
        deathManager = GetComponent<VRDeathManager>();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        UpdateHealthUI();

        if (currentHealth <= lowHealthThreshold && !isFlashing)
        {
            StartCoroutine(FlashEffect());
        }

        if (currentHealth <= lowHealthThreshold && !isLowHealthSoundPlaying)
        {
            if (lowHealthLoop != null)
            {
                lowHealthLoop.loop = true;
                lowHealthLoop.Play();
            }
            isLowHealthSoundPlaying = true;
        }
        else if (currentHealth > lowHealthThreshold && isLowHealthSoundPlaying)
        {
            if (lowHealthLoop != null)
                lowHealthLoop.Stop();

            isLowHealthSoundPlaying = false;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (damageSound != null)
            damageSound.Play();

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth > lowHealthThreshold && isLowHealthSoundPlaying)
        {
            if (lowHealthLoop != null)
                lowHealthLoop.Stop();
            isLowHealthSoundPlaying = false;
        }
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
                HP.color = warningColor;
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
                HP.color = Color.Lerp(flashColor, warningColor, t);
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
        if (isDead) return;
        isDead = true;

        Debug.Log("Hailey is dead");

        if (deathManager != null)
            deathManager.TriggerDeath();
    }
}
