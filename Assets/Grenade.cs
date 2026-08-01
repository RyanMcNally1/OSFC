using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    [Header("Explosion Settings")]
    public float fuseTime = 3f;
    public float explosionRadius = 5f;
    public float explosionDamage = 75f;
    public float explosionForce = 700f;

    [Header("Detection")]
    public LayerMask damageLayers = ~0;

    [Header("Explosion Visual")]
    public GameObject explosionRadiusVisual;
    public float explosionFlashTime = 0.1f;

    private bool hasExploded;

    void Start() {
        if (explosionRadiusVisual != null) {
            explosionRadiusVisual.SetActive(false);
        }

        StartCoroutine(Fuse());
    }

    IEnumerator Fuse() {
        yield return new WaitForSeconds(fuseTime);

        StartCoroutine(ExplodeRoutine());
    }

    IEnumerator ExplodeRoutine() {
        if (hasExploded) {
            yield break;
        }

        hasExploded = true;

        ApplyExplosionDamage();
        ShowExplosionVisual();

        Debug.Log("Grenade exploded.");

        yield return new WaitForSeconds(
            explosionFlashTime
        );

        Destroy(gameObject);
    }

    void ApplyExplosionDamage() {
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position,
            explosionRadius,
            damageLayers,
            QueryTriggerInteraction.Ignore
        );

        HashSet<Damageable> damagedTargets =
            new HashSet<Damageable>();

        HashSet<Rigidbody> pushedRigidbodies =
            new HashSet<Rigidbody>();

        foreach (Collider hitCollider in hitColliders) {
            Damageable damageable =
                hitCollider.GetComponentInParent<Damageable>();

            if (
                damageable != null &&
                !damagedTargets.Contains(damageable)
            ) {
                damageable.TakeDamage(
                    explosionDamage
                );

                damagedTargets.Add(
                    damageable
                );
            }

            Rigidbody hitRigidbody =
                hitCollider.attachedRigidbody;

            if (
                hitRigidbody != null &&
                !pushedRigidbodies.Contains(hitRigidbody)
            ) {
                hitRigidbody.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius
                );

                pushedRigidbodies.Add(
                    hitRigidbody
                );
            }
        }
    }

    void ShowExplosionVisual() {
        if (explosionRadiusVisual == null) {
            return;
        }

        explosionRadiusVisual.transform.localPosition = Vector3.zero;
        explosionRadiusVisual.transform.localRotation = Quaternion.identity;

        explosionRadiusVisual.SetActive(true);
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }
}
