using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurations")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Slider healthBar;

    [Header("Animation")]
    public PlayerAnimation playerAnimation;

    [Header("Damage Effects")]
    public DamageFlash damageFlash;

    [Header("Invulnerability")]
    public float invulnerabilityDuration = 2f;
    private float invulnerableUntil;

    void Start() {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (playerAnimation == null) {
            playerAnimation =
                GetComponent<PlayerAnimation>();
        }

        if (damageFlash == null) {
            damageFlash =
                GetComponent<DamageFlash>();
        }
    }

    public void TakeDamage(float damage) {
        if (currentHealth <= 0f) {
            return;
        }

        if (damage <= 0f) {
            return;
        }

        if (Time.time < invulnerableUntil) {
            return;
        }

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maxHealth
        );

        invulnerableUntil =
            Time.time + invulnerabilityDuration;

        if (damageFlash != null) {
            damageFlash.Flash();
        }

        if (
            currentHealth > 0f &&
            playerAnimation != null
        ) {
            playerAnimation.PlayHitAnimation();
        }

        UpdateHealthUI();

        if (currentHealth <= 0f) {
            Die();
        }
    }

    public void Heal(float amount) {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthUI();
    }

    void UpdateHealthUI() {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    void Die() {
        Debug.Log("Player Died");

        // Disable player controls
        // Play death animation
        // Show Game Over screen
    }

    void Update() {
    if (Input.GetKeyDown(KeyCode.H))
    {
        TakeDamage(10);
        }
    }
}
