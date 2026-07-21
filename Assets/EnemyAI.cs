using UnityEngine;

public class EnemyAI : MonoBehaviour {

    [Header("References")]
    public Rigidbody rb;
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float acceleration = 12f;
    public float rotationSpeed = 10f;
    public float stoppingDistance = 1.4f;

    [Header("Attack")]
    public float attackRange = 1.8f;
    public float attackDamage = 15f;
    public float attackCooldown = 1.2f;

    [Header("Physical Interaction")]
    public float playerPushForce = 2f;

    private PlayerHealth playerHealth;
    private float nextAttackTime;

    void Start() {
        if (rb == null) {
            rb = GetComponent<Rigidbody>();
        }

        if (player == null) {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null) {
                player = playerObject.transform;
            }
        }

        if (player != null) {
            playerHealth =
                player.GetComponent<PlayerHealth>();
        }
        else {
            Debug.LogWarning(
                gameObject.name +
                " could not find an object tagged Player."
            );
        }
    }

    void Update() {
        if (player == null) {
            return;
        }

        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distanceToPlayer <= attackRange) {
            TryAttack();
        }
    }

    void FixedUpdate() {
        if (player == null || rb == null) {
            return;
        }

        Vector3 directionToPlayer =
            player.position -
            transform.position;

        directionToPlayer.y = 0f;

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
        Vector3 horizontalVelocity = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            Vector3.zero,
            acceleration * 0.25f * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(
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

        if (playerHealth == null) {
            Debug.LogWarning(
                "Enemy could not find PlayerHealth."
            );

            return;
        }

        nextAttackTime =
            Time.time + attackCooldown;

        playerHealth.TakeDamage(
            attackDamage
        );

        Debug.Log(
            gameObject.name +
            " attacked the player."
        );
    }
}
