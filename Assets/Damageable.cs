using UnityEngine;

public class Damageable : MonoBehaviour {

    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private DamageFlash damageFlash;

    void Awake() {
        damageFlash = GetComponent<DamageFlash>();

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
    }

    void Die() {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }
}