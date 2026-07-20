using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurations")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Slider healthBar;

    void Start() {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
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
