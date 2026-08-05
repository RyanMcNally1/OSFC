using UnityEngine;

public class EnemyAnimation : MonoBehaviour {

    [Header("References")]
    public Animator animator;
    public Rigidbody enemyRigidbody;

    [Header("Movement")]
    public float maximumMoveSpeed = 4f;
    public float animationDampTime = 0.1f;

    [Header("Attack Event")]
    public EnemyAI enemyAI;

    void Update() {
        UpdateMovementAnimation();
    }

    void UpdateMovementAnimation() {
        if (animator == null || enemyRigidbody == null) {
            return;
        }

        Vector3 horizontalVelocity = new Vector3(
            enemyRigidbody.linearVelocity.x,
            0f,
            enemyRigidbody.linearVelocity.z
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

    public void PlayAttackAnimation() {
        if (animator == null) {
            return;
        }

        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
    }

    public void PlayHitAnimation() {
        if (animator == null) {
            return;
        }

        animator.ResetTrigger("TakeHit");
        animator.SetTrigger("TakeHit");
    }

    public void PlayDeathAnimation() {
        if (animator == null) {
            return;
        }

        animator.SetBool("IsDead", true);
        animator.ResetTrigger("Die");
        animator.SetTrigger("Die");
    }

    public void PlaySpecialAttackAnimation() {
        if (animator == null) {
            return;
        }

        animator.ResetTrigger("Attack");
        animator.ResetTrigger("SpecialAttack");
        animator.SetTrigger("SpecialAttack");
    }
}
