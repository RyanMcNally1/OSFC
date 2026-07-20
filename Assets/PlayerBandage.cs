using UnityEngine;

public class PlayerBandage : MonoBehaviour {

    [Header("References")]
    public PlayerHealth playerHealth;

    [Header("Bandage Settings")]
    public int maxBandages = 3;
    public int currentBandages = 3;
    public float healAmount = 25f;
    public float useCooldown = 0.5f;

    private float nextUseTime;

    void Awake() {
        if (playerHealth == null) {
            playerHealth = GetComponentInParent<PlayerHealth>();
        }
    }

    void Start() {
        UpdateBandageUI();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.B) && Time.time >= nextUseTime) {
            TryUseBandage();
        }
    }

    void TryUseBandage() {
        if (playerHealth == null) {
            Debug.LogWarning("PlayerHealth has not been assigned.");
            return;
        }

        if (currentBandages <= 0) {
            Debug.Log("No bandages remaining.");
            return;
        }

        if (playerHealth.currentHealth >= playerHealth.maxHealth) {
            Debug.Log("Already at full health.");
            return;
        }

        currentBandages--;
        nextUseTime = Time.time + useCooldown;

        playerHealth.Heal(healAmount);
        UpdateBandageUI();

        Debug.Log(
            $"Bandage used. Health: {playerHealth.currentHealth}, " +
            $"Bandages: {currentBandages}"
        );
    }

    public void AddBandages(int amount) {
        currentBandages += amount;
        currentBandages = Mathf.Clamp(
            currentBandages,
            0,
            maxBandages
        );

        UpdateBandageUI();
    }

    private void UpdateBandageUI() {
        if (UIManager.Instance != null) {
            UIManager.Instance.UpdateBandages(currentBandages);
        }
    }
}
