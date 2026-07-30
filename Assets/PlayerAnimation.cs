using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    [Header("References")]
    public Animator animator;
    public Rigidbody playerRigidbody;

    [Header("Movement")]
    public float maximumMoveSpeed = 6f;
    public float animationDampTime = 0.1f;

    void Update() {
        UpdateMovementAnimation();
    }

    void UpdateMovementAnimation() {
        if (animator == null || playerRigidbody == null) {
            return;
        }

        Vector3 horizontalVelocity = new Vector3(
            playerRigidbody.linearVelocity.x,
            0f,
            playerRigidbody.linearVelocity.z
        );

        float normalizedSpeed = Mathf.Clamp01(
            horizontalVelocity.magnitude / maximumMoveSpeed
        );

        animator.SetFloat(
            "MoveSpeed",
            normalizedSpeed,
            animationDampTime,
            Time.deltaTime
        );
    }
}