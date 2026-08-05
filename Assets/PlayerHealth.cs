using System.Collections;
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

    [Header("Death")]
    [SerializeField] private float deathScreenDelay = 2f;

    private bool isDead;

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

    public void ResetHealth() {
        currentHealth = maxHealth;
        RefreshUI();
    }

    void Die() {
        if (isDead) {
            return;
        }

        isDead = true;
        currentHealth = 0f;

        RefreshUI();

        PlayerController controller =
            GetComponent<PlayerController>();

        if (controller != null) {
            controller.enabled = false;
        }

        Rigidbody playerRigidbody =
            GetComponent<Rigidbody>();

        if (playerRigidbody != null) {
            playerRigidbody.linearVelocity =
                Vector3.zero;

            playerRigidbody.angularVelocity =
                Vector3.zero;
        }

        if (playerAnimation != null) {
            playerAnimation.PlayDeathAnimation();
        }

        StartCoroutine(
            ShowDeathScreenAfterAnimation()
        );
    }

    IEnumerator ShowDeathScreenAfterAnimation() {
        yield return new WaitForSeconds(
            deathScreenDelay
        );

        PauseMenu pauseMenu =
            FindAnyObjectByType<PauseMenu>();

        if (pauseMenu != null) {
            pauseMenu.ShowDeathScreen();
        }
        else {
            Debug.LogWarning(
                "PlayerHealth could not find the PauseMenu."
            );
        }
    }

        public void RefreshUI() {
        if (UIManager.Instance != null) {
            UIManager.Instance.UpdateHealth(
                currentHealth,
                maxHealth
            );
        }
    }
}
