using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log(
            $"{gameObject.name} took {damage} damage. " +
            $"Remaining health: {currentHealth}"
        );

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }
}
