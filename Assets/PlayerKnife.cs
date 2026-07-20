using UnityEngine;

public class PlayerKnife : MonoBehaviour {

    [Header("References")]
    public Camera playerCamera;

    [Header("Knife Settings")]
    public float damage = 40f;
    public float attackRange = 2f;
    public float attackRadius = 0.5f;
    public float attackCooldown = 0.6f;

    [Header("Target Detection")]
    public LayerMask targetLayers = ~0;

    private float nextAttackTime;

    void Update() {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime) {
            Attack();
        }
    }

    void Attack() {
        nextAttackTime = Time.time + attackCooldown;

        Vector3 attackOrigin = playerCamera.transform.position;
        Vector3 attackDirection = playerCamera.transform.forward;

        if (Physics.SphereCast(
            attackOrigin,
            attackRadius,
            attackDirection,
            out RaycastHit hit,
            attackRange,
            targetLayers,
            QueryTriggerInteraction.Ignore
        )) {
            Damageable damageable = hit.collider.GetComponentInParent<Damageable>();

            if (damageable != null) {
                damageable.TakeDamage(damage);
                Debug.Log("Knife hit: " + hit.collider.name);
            }
            else {
                Debug.Log("Knife struck: " + hit.collider.name);
            }
        }
        else {
            Debug.Log("Knife missed.");
        }
    }

    void OnDrawGizmosSelected() {
        if (playerCamera == null) {
            return;
        }

        Vector3 attackOrigin = playerCamera.transform.position;
        Vector3 attackEnd =
            attackOrigin + playerCamera.transform.forward * attackRange;

        Gizmos.DrawWireSphere(attackEnd, attackRadius);
        Gizmos.DrawLine(attackOrigin, attackEnd);
    }
}
