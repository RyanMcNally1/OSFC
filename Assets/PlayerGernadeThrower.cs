using UnityEngine;

public class PlayerGrenadeThrower : MonoBehaviour {

    [Header("References")]
    public Camera playerCamera;
    public GameObject grenadePrefab;
    public Transform throwPoint;

    [Header("Grenade Inventory")]
    [SerializeField] private int maxGrenades = 3;
    [SerializeField] private int currentGrenades = 3;

    [Header("Throw Settings")]
    public float throwForce = 14f;
    public float upwardForce = 3f;
    public float throwCooldown = 0.75f;

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
        if (Input.GetMouseButtonDown(0) && Time.time >= nextThrowTime) {
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
            Debug.LogWarning("Grenade throw point has not been assigned.");
            return;
        }

        if (playerCamera == null) {
            Debug.LogWarning("Player camera has not been assigned.");
            return;
        }

        GameObject grenadeObject = Instantiate(
            grenadePrefab,
            throwPoint.position,
            throwPoint.rotation
        );

        Rigidbody grenadeRigidbody =
            grenadeObject.GetComponent<Rigidbody>();

        if (grenadeRigidbody == null) {
            Debug.LogError("The grenade prefab needs a Rigidbody.");
            Destroy(grenadeObject);
            return;
        }

        Vector3 throwDirection =
            playerCamera.transform.forward * throwForce +
            Vector3.up * upwardForce;

        grenadeRigidbody.AddForce(
            throwDirection,
            ForceMode.VelocityChange
        );

        currentGrenades--;
        nextThrowTime = Time.time + throwCooldown;

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

    private void UpdateGrenadeUI() {
        if (UIManager.Instance != null) {
            UIManager.Instance.UpdateGrenades(currentGrenades);
        }
    }
}