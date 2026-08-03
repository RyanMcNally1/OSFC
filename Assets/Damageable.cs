using UnityEngine;

public class Damageable : MonoBehaviour {

    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private DamageFlash damageFlash;
    private EnemyAnimation enemyAnimation;

    void Awake() {
        damageFlash = GetComponent<DamageFlash>();
        enemyAnimation = GetComponent<EnemyAnimation>();

        if (damageFlash == null) {
            damageFlash = GetComponentInChildren<DamageFlash>();
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

        if (damageFlash != null) {
            damageFlash.Flash();
        }

        Debug.Log(
            $"{gameObject.name} took {damage} damage. " +
            $"Remaining health: {currentHealth}"
        );

        if (currentHealth <= 0f) {
            Die();
        }

        if (
            currentHealth > 0f &&
            enemyAnimation != null
        ) {
            enemyAnimation.PlayHitAnimation();
        }
    }

    void Die() {
        Debug.Log($"{gameObject.name} died.");

        EnemyAI enemyAI = GetComponent<EnemyAI>();

        if (enemyAI != null) {
            enemyAI.enabled = false;
        }

        Collider enemyCollider = GetComponent<Collider>();

        if (enemyCollider != null) {
            enemyCollider.enabled = false;
        }

        Rigidbody enemyRigidbody = GetComponent<Rigidbody>();

        if (enemyRigidbody != null) {
            enemyRigidbody.linearVelocity = Vector3.zero;
            enemyRigidbody.isKinematic = true;
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