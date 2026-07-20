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

    private bool hasExploded;

    void Start() {
        StartCoroutine(Fuse());
    }

    void Explode() {
        if (hasExploded) {
            return;
        }

        hasExploded = true;

        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position,
            explosionRadius,
            damageLayers,
            QueryTriggerInteraction.Ignore
        );

        HashSet<Damageable> damagedTargets = new HashSet<Damageable>();

        foreach (Collider hitCollider in hitColliders) {
            Damageable damageable =
                hitCollider.GetComponentInParent<Damageable>();

            if (damageable != null && !damagedTargets.Contains(damageable)) {
                damageable.TakeDamage(explosionDamage);
                damagedTargets.Add(damageable);
            }

            Rigidbody hitRigidbody = hitCollider.attachedRigidbody;

            if (hitRigidbody != null) {
                hitRigidbody.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius
                );
            }
        }

        Debug.Log("Grenade exploded.");

        Destroy(gameObject);
    }

    IEnumerator Fuse() {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
