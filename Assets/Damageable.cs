using UnityEngine;

public class Damageable : MonoBehaviour {

    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private DamageFlash damageFlash;
    private EnemyAnimation enemyAnimation;
    private EnemyAI enemyAI;

    public float CurrentHealth {
        get {
            return currentHealth;
        }
    }

    public float MaxHealth {
        get {
            return maxHealth;
        }
    }

    void Awake() {
        damageFlash = GetComponent<DamageFlash>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        enemyAI = GetComponent<EnemyAI>();

        if (damageFlash == null) {
            damageFlash =
                GetComponentInChildren<DamageFlash>();
        }
    }

    void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        if (damage <= 0f || currentHealth <= 0f) {
            return;
        }

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maxHealth
        );

        if (damageFlash != null) {
            damageFlash.Flash();
        }

        if (
            enemyAI != null &&
            enemyAI.isBoss &&
            UIManager.Instance != null
        ) {
            UIManager.Instance.UpdateBossHealth(
                currentHealth
            );
        }

        Debug.Log(
            $"{gameObject.name} took {damage} damage. " +
            $"Remaining health: {currentHealth}"
        );

        if (currentHealth <= 0f) {
            Die();
            return;
        }

        if (enemyAnimation != null) {
            enemyAnimation.PlayHitAnimation();
        }
    }

    void Die() {
        Debug.Log($"{gameObject.name} died.");

        if (
            enemyAI != null &&
            enemyAI.isBoss &&
            UIManager.Instance != null
        ) {
            UIManager.Instance.HideBossHealth();
        }

        if (enemyAI != null) {
            enemyAI.enabled = false;
        }

        Collider enemyCollider =
            GetComponent<Collider>();

        if (enemyCollider != null) {
            enemyCollider.enabled = false;
        }

        Rigidbody enemyRigidbody =
            GetComponent<Rigidbody>();

        if (enemyRigidbody != null) {
            enemyRigidbody.linearVelocity =
                Vector3.zero;

            enemyRigidbody.isKinematic = true;
        }

        if (enemyAI != null && enemyAI.isBoss) {
            EnemyAI.BossDefeated = true;

            if (UIManager.Instance != null) {
                UIManager.Instance.HideBossHealth();
            }
        }

        if (enemyAnimation != null) {
            enemyAnimation.PlayDeathAnimation();
        }

        Destroy(
            gameObject,
            3f
        );
    }
}