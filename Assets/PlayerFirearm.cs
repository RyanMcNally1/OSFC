using System.Collections;
using UnityEngine;

public class PlayerFirearm : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform muzzle;

    [Header("Weapon Settings")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private float range = 150f;
    [SerializeField] private float roundsPerMinute = 600f;

    [Header("Ammo")]
    public int magazineSize = 30;
    public int currentAmmo = 30;
    public int maxReserveAmmo = 180;
    public int reserveAmmo = 180;
    private float reloadDuration = 1.8f;

    private float nextFireTime;
    private bool isReloading;

    [Header("Animation")]
    public PlayerAnimation playerAnimation;

    private void Start() {
        if (playerCamera == null)
        {
            Debug.LogError("PlayerFirearm needs a Player Camera reference.", this);
        }

        if (muzzle == null)
        {
            Debug.LogWarning(
                "PlayerFirearm has no muzzle assigned. Shooting still works, " +
                "but muzzle effects cannot be positioned correctly.",
                this
            );
        }

        currentAmmo = Mathf.Clamp(currentAmmo, 0, magazineSize);

        UpdateAmmoUI();
    }

    private void Update() {
        if (isReloading) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            StartReload();
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) {
            TryFire();
        }
    }

    private void TryFire() {
        if (currentAmmo <= 0) {
            Debug.Log("No ammo. Press R to reload.");
            return;
        }

        if (playerAnimation != null) {
            playerAnimation.PlayFireAnimation();
        }

        float secondsBetweenShots = 60f / roundsPerMinute;
        nextFireTime = Time.time + secondsBetweenShots;

        currentAmmo--;

        FireRaycast();
        UpdateAmmoUI();

        Debug.Log($"Fired. Ammo: {currentAmmo} / {reserveAmmo}");
    }

    private void FireRaycast() {
        if (playerCamera == null) {
            return;
        }

        Ray aimRay = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 targetPoint;

        if (Physics.Raycast(aimRay, out RaycastHit cameraHit, range, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
            targetPoint = cameraHit.point;
        }
        else {
            targetPoint = aimRay.origin + aimRay.direction * range;
        }

        Vector3 shotOrigin = muzzle != null
            ? muzzle.position
            : playerCamera.transform.position;

        Vector3 shotDirection = (targetPoint - shotOrigin).normalized;

        if (Physics.Raycast(shotOrigin, shotDirection, out RaycastHit weaponHit, range, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
            Debug.Log($"Hit: {weaponHit.collider.name}");

            Debug.DrawLine(shotOrigin, weaponHit.point, Color.red, 1f);

            Damageable damageable =
                weaponHit.collider.GetComponentInParent<Damageable>();

            if (damageable != null){
                damageable.TakeDamage(damage);
            }
        }
        else {
            Debug.DrawRay(
                shotOrigin,
                shotDirection * range,
                Color.yellow,
                1f
            );
        }
    }

    private void StartReload() {
        if (isReloading) {
            return;
        }

        if (currentAmmo >= magazineSize) {
            Debug.Log("Magazine is already full.");
            return;
        }

        if (reserveAmmo <= 0) {
            Debug.Log("No reserve ammunition.");
            return;
        }

        StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        isReloading = true;

            if (playerAnimation != null) {
                playerAnimation.PlayReloadAnimation();
                playerAnimation.SetReloading(true);
            }

        UIManager.Instance.ShowReloading(true);

        yield return new WaitForSeconds(reloadDuration);

        int ammoNeeded = magazineSize - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;

            if (playerAnimation != null) {
                playerAnimation.SetReloading(false);
            }

        UIManager.Instance.ShowReloading(false);
        UpdateAmmoUI();
    }

    public void RefillAmmo() {
        currentAmmo = magazineSize;
        reserveAmmo = maxReserveAmmo;

        UIManager.Instance.UpdateAmmo(
            currentAmmo,
            reserveAmmo
        );
    }

    private void UpdateAmmoUI() {
    if (UIManager.Instance != null) {
        UIManager.Instance.UpdateAmmo(currentAmmo, reserveAmmo);
        }
    }

    public void RefreshUI() {
        UpdateAmmoUI();
    }
}