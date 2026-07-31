using System.Collections;
using UnityEngine;

public class PlayerGrenadeThrower : MonoBehaviour {

    [Header("References")]
    public Camera playerCamera;
    public GameObject grenadePrefab;
    public Transform throwPoint;

    [Header("Grenade Inventory")]
    public int maxGrenades = 3;
    public int currentGrenades = 3;

    [Header("Throw Settings")]
    public float throwForce = 14f;
    public float upwardForce = 3f;
    public float throwCooldown = 0.75f;

    [Header("Animation")]
    public PlayerAnimation playerAnimation;
    public float grenadeReleaseDelay = 0.45f;

    private bool isThrowing;
    private float nextThrowTime;

    public int CurrentGrenades {
        get {
            return currentGrenades;
        }
    }

    void Start() {
        UpdateGrenadeUI();
    }

    void Update() {
        if (
            Input.GetMouseButtonDown(0) &&
            Time.time >= nextThrowTime &&
            !isThrowing
        ) {
            TryThrowGrenade();
        }
    }

    void TryThrowGrenade() {
        if (currentGrenades <= 0) {
            Debug.Log("No grenades remaining.");
            return;
        }

        if (grenadePrefab == null) {
            Debug.LogWarning("Grenade prefab has not been assigned.");
            return;
        }

        if (throwPoint == null) {
            Debug.LogWarning("Throw point has not been assigned.");
            return;
        }

        if (playerCamera == null) {
            Debug.LogWarning("Player camera has not been assigned.");
            return;
        }

        StartCoroutine(ThrowGrenadeRoutine());
    }

    IEnumerator ThrowGrenadeRoutine() {
        isThrowing = true;
        nextThrowTime = Time.time + throwCooldown;

        if (playerAnimation != null) {
            playerAnimation.PlayGrenadeThrowAnimation();
        }

        yield return new WaitForSeconds(
            grenadeReleaseDelay
        );

        ReleaseGrenade();

        isThrowing = false;
    }

    void ReleaseGrenade() {
        GameObject grenadeObject = Instantiate(
            grenadePrefab,
            throwPoint.position,
            throwPoint.rotation
        );

        Rigidbody grenadeRigidbody =
            grenadeObject.GetComponent<Rigidbody>();

        if (grenadeRigidbody == null) {
            Debug.LogError(
                "The grenade prefab needs a Rigidbody."
            );

            Destroy(grenadeObject);
            return;
        }

        Vector3 aimPoint;

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out RaycastHit hit,
            50f
        )) {
            aimPoint = hit.point;
        }
        else {
            aimPoint =
                playerCamera.transform.position +
                playerCamera.transform.forward * 50f;
        }

        Vector3 throwDirection =
            (aimPoint - throwPoint.position).normalized;

        Vector3 throwVelocity =
            throwDirection * throwForce +
            Vector3.up * upwardForce;

        grenadeRigidbody.AddForce(
            throwVelocity,
            ForceMode.VelocityChange
        );

        grenadeRigidbody.AddForce(
            throwDirection,
            ForceMode.VelocityChange
        );

        currentGrenades--;
        UpdateGrenadeUI();
    }

    public void AddGrenades(int amount) {
        currentGrenades += amount;
        currentGrenades = Mathf.Clamp(
            currentGrenades,
            0,
            maxGrenades
        );

        UpdateGrenadeUI();
    }

    public void RefillGrenades() {
        currentGrenades = maxGrenades;

        UIManager.Instance.UpdateGrenades(
            currentGrenades
        );
    }

    private void UpdateGrenadeUI() {
        if (UIManager.Instance != null) {
            UIManager.Instance.UpdateGrenades(currentGrenades);
        }
    }
}