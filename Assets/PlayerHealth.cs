using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [Header("Configurations")]
    public float maxHealth = 100f;
    public float currentHealth;

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
        RefreshUI();

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
        if (
            currentHealth <= 0f ||
            damage <= 0f ||
            Time.time < invulnerableUntil
        ) {
            return;
        }

        currentHealth = Mathf.Clamp(
            currentHealth - damage,
            0f,
            maxHealth
        );

        invulnerableUntil =
            Time.time + invulnerabilityDuration;

        RefreshUI();

        if (damageFlash != null) {
            damageFlash.Flash();
        }

        if (currentHealth <= 0f) {
            Die();
            return;
        }

        if (playerAnimation != null) {
            playerAnimation.PlayHitAnimation();
        }
    }

    public void Heal(float amount) {
        if (
            amount <= 0f ||
            currentHealth <= 0f
        ) {
            return;
        }

        currentHealth = Mathf.Clamp(
            currentHealth + amount,
            0f,
            maxHealth
        );

        RefreshUI();
    }

    public void ResetHealth() {
        isDead = false;
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
        if (UIManager.Instance == null) {
            return;
        }

        UIManager.Instance.UpdateHealth(
            currentHealth,
            maxHealth
        );
    }
}
