using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    [Header("References")]
    public Rigidbody rb;
    public Transform player;
    public Transform eyePoint;

    [Header("Detection")]
    public float detectionRange = 15f;
    public LayerMask lineOfSightLayers = ~0;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float acceleration = 12f;
    public float rotationSpeed = 10f;
    public float stoppingDistance = 1.4f;

    [Header("Attack")]
    public float attackRange = 1.8f;
    public float attackDamage = 15f;
    public float attackCooldown = 1.2f;
    public float attackHitDelay = 0.35f;

    [Header("Animation")]
    public EnemyAnimation enemyAnimation;

    private PlayerHealth playerHealth;
    private float nextAttackTime;
    private bool canSeePlayer;
    private bool hasDetectedPlayer;

    void Start() {
        if (rb == null) {
            rb = GetComponent<Rigidbody>();
        }

        if (enemyAnimation == null) {
            enemyAnimation = GetComponent<EnemyAnimation>();
        }

        if (player == null) {
            PlayerController playerController =
                FindAnyObjectByType<PlayerController>();

            if (playerController != null) {
                player = playerController.transform;
            }
        }

        if (player != null) {
            playerHealth =
                player.GetComponent<PlayerHealth>();
        }
        else {
            Debug.LogWarning(
                gameObject.name +
                " could not find the player."
            );
        }
    }

    void Update() {
        if (player == null) {
            return;
        }

        if (!hasDetectedPlayer && CanDetectPlayer()) {
            hasDetectedPlayer = true;

            Debug.Log(
                gameObject.name +
                " detected the player."
            );
        }

        if (!hasDetectedPlayer) {
            return;
        }

        float distanceToPlayer =
            GetHorizontalDistanceToPlayer();

        if (distanceToPlayer <= attackRange) {
            TryAttack();
        }
    }

    void FixedUpdate() {
        if (player == null || rb == null) {
            return;
        }

        if (!hasDetectedPlayer) {
            SlowDown();
            return;
        }

        Vector3 directionToPlayer =
            GetDirectionToPlayer();

        float distanceToPlayer =
            directionToPlayer.magnitude;

        if (directionToPlayer.sqrMagnitude > 0.01f) {
            RotateTowardsPlayer(
                directionToPlayer
            );
        }

        if (distanceToPlayer > stoppingDistance) {
            MoveTowardsPlayer(
                directionToPlayer.normalized
            );
        }
        else {
            SlowDown();
        }
    }

    bool CanDetectPlayer() {
        Vector3 directionToPlayer =
            GetDirectionToPlayer();

        float distanceToPlayer =
            directionToPlayer.magnitude;

        if (distanceToPlayer > detectionRange) {
            return false;
        }

        Vector3 rayOrigin;

        if (eyePoint != null) {
            rayOrigin = eyePoint.position;
        }
        else {
            rayOrigin =
                transform.position +
                Vector3.up * 1.5f;
        }

        Vector3 targetPosition =
            player.position +
            Vector3.up * 1f;

        Vector3 rayDirection =
            targetPosition - rayOrigin;

        float rayDistance =
            rayDirection.magnitude;

        rayDirection.Normalize();

        if (Physics.Raycast(
            rayOrigin,
            rayDirection,
            out RaycastHit hit,
            rayDistance,
            lineOfSightLayers,
            QueryTriggerInteraction.Ignore
        )) {
            bool hitPlayer =
                hit.transform == player ||
                hit.transform.IsChildOf(player);

            return hitPlayer;
        }

        return false;
    }

    Vector3 GetDirectionToPlayer() {
        Vector3 targetPosition =
            player.position;

        targetPosition.y =
            transform.position.y;

        return targetPosition -
            transform.position;
    }

    float GetHorizontalDistanceToPlayer() {
        return GetDirectionToPlayer().magnitude;
    }

    void MoveTowardsPlayer(Vector3 direction) {
        Vector3 targetVelocity =
            direction * moveSpeed;

        Vector3 currentHorizontalVelocity =
            new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

        Vector3 newHorizontalVelocity =
            Vector3.MoveTowards(
                currentHorizontalVelocity,
                targetVelocity,
                acceleration * Time.fixedDeltaTime
            );

        rb.linearVelocity =
            new Vector3(
                newHorizontalVelocity.x,
                rb.linearVelocity.y,
                newHorizontalVelocity.z
            );
    }

    void SlowDown() {
        Vector3 horizontalVelocity =
            new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

        horizontalVelocity =
            Vector3.MoveTowards(
                horizontalVelocity,
                Vector3.zero,
                acceleration *
                Time.fixedDeltaTime
            );

        rb.linearVelocity =
            new Vector3(
                horizontalVelocity.x,
                rb.linearVelocity.y,
                horizontalVelocity.z
            );
    }

    void RotateTowardsPlayer(
        Vector3 direction
    ) {
        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        Quaternion newRotation =
            Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed *
                Time.fixedDeltaTime
            );

        rb.MoveRotation(newRotation);
    }

    void TryAttack() {
        if (Time.time < nextAttackTime) {
            return;
        }

        nextAttackTime =
            Time.time + attackCooldown;

        if (enemyAnimation != null) {
            enemyAnimation.PlayAttackAnimation();
        }

        StartCoroutine(
            DelayedAttack()
        );
    }

    IEnumerator DelayedAttack() {
        yield return new WaitForSeconds(
            attackHitDelay
        );

        ApplyAttackDamage();
    }

    public void ApplyAttackDamage() {
        if (
            player == null ||
            playerHealth == null
        ) {
            return;
        }

        if (!CanDetectPlayer()) {
            return;
        }

        float distanceToPlayer =
            GetHorizontalDistanceToPlayer();

        if (distanceToPlayer > attackRange) {
            return;
        }

        playerHealth.TakeDamage(
            attackDamage
        );

        Debug.Log(
            gameObject.name +
            " hit the player."
        );
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        if (player == null) {
            return;
        }

        Vector3 rayOrigin;

        if (eyePoint != null) {
            rayOrigin = eyePoint.position;
        }
        else {
            rayOrigin =
                transform.position +
                Vector3.up * 1.5f;
        }

        Vector3 targetPosition =
            player.position +
            Vector3.up * 1f;

        Gizmos.DrawLine(
            rayOrigin,
            targetPosition
        );
    }
}
