using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    [Header("References")]
    public Animator animator;
    public Rigidbody playerRigidbody;
    public Transform movementReference;

    [Header("Movement")]
    public float maximumMoveSpeed = 6f;
    public float animationDampTime = 0.1f;

    [Header("Grounding")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    void Update() {
        UpdateMovementAnimation();
        UpdateJumpAnimation();
    }

    void UpdateMovementAnimation() {
        if (
            animator == null ||
            playerRigidbody == null ||
            movementReference == null
        ) {
            return;
        }

        Vector3 horizontalVelocity = new Vector3(
            playerRigidbody.linearVelocity.x,
            0f,
            playerRigidbody.linearVelocity.z
        );

        Vector3 localVelocity =
            movementReference.InverseTransformDirection(
                horizontalVelocity
            );

        float moveSpeed = Mathf.Clamp(
            localVelocity.z / maximumMoveSpeed,
            -1f,
            1f
        );

        float moveX = Mathf.Clamp(
            localVelocity.x / maximumMoveSpeed,
            -1f,
            1f
        );

        animator.SetFloat(
            "MoveSpeed",
            moveSpeed,
            animationDampTime,
            Time.deltaTime
        );

        animator.SetFloat(
            "MoveX",
            moveX,
            animationDampTime,
            Time.deltaTime
        );
    }

    void UpdateJumpAnimation() {
        if (
            animator == null ||
            playerRigidbody == null
        ) {
            return;
        }

        bool isGrounded = CheckGrounded();

        animator.SetBool(
            "IsGrounded",
            isGrounded
        );

        animator.SetFloat(
            "VerticalSpeed",
            playerRigidbody.linearVelocity.y
        );
    }

    bool CheckGrounded() {
        if (groundCheck == null) {
            return false;
        }

        return Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );
    }

    public void PlayJumpAnimation() {
        if (animator == null) {
            return;
        }

        animator.ResetTrigger("Jump");
        animator.SetTrigger("Jump");
    }

    void OnDrawGizmosSelected() {
        if (groundCheck == null) {
            return;
        }

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }

    public void PlayFireAnimation() {
        if (animator == null) {
            return;
        }

        animator.ResetTrigger("Fire");
        animator.SetTrigger("Fire");
    }

    public void PlayReloadAnimation() {
        if (animator == null) {
            return;
        }

        animator.ResetTrigger("Reload");
        animator.SetTrigger("Reload");
    }

    public void SetReloading(bool isReloading) {
        if (animator == null) {
            return;
        }

        animator.SetBool(
            "IsReloading",
            isReloading
        );
    }
}