using System.Collections;
using UnityEngine;

public class PlayerBandage : MonoBehaviour {

    [Header("References")]
    public PlayerHealth playerHealth;
    public PlayerAnimation playerAnimation;
    public PlayerController playerController;

    [Header("Bandage Settings")]
    public int maxBandages = 3;
    public int currentBandages = 3;
    public float healAmount = 25f;
    public float bandageDuration = 3f;
    public float useCooldown = 0.5f;

    private float nextUseTime;
    private bool isBandaging;

    void Awake() {
        if (playerHealth == null) {
            playerHealth = GetComponentInParent<PlayerHealth>();
        }

        if (playerAnimation == null) {
            playerAnimation = GetComponentInParent<PlayerAnimation>();
        }

        if (playerController == null) {
            playerController = GetComponentInParent<PlayerController>();
        }
    }

    void Start() {
        UpdateBandageUI();
    }

    void Update() {
        if (
            Input.GetKeyDown(KeyCode.B) &&
            Time.time >= nextUseTime &&
            !isBandaging
        ) {
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

        StartCoroutine(BandageRoutine());
    }

    IEnumerator BandageRoutine() {
        isBandaging = true;

        if (playerAnimation != null) {
            playerAnimation.PlayBandageAnimation();
        }

        if (playerController != null) {
            playerController.SetBandaging(true);
        }

        if (UIManager.Instance != null) {
            UIManager.Instance.ShowBandaging(true);
        }

        yield return new WaitForSeconds(bandageDuration);

        currentBandages--;
        playerHealth.Heal(healAmount);
        UpdateBandageUI();

        nextUseTime = Time.time + useCooldown;
        isBandaging = false;

        if (playerController != null) {
            playerController.SetBandaging(false);
        }

        if (UIManager.Instance != null) {
            UIManager.Instance.ShowBandaging(false);
        }

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

    public void RefillBandages() {
        currentBandages = maxBandages;
        UpdateBandageUI();
    }

    private void UpdateBandageUI() {
        if (UIManager.Instance != null) {
            UIManager.Instance.UpdateBandages(currentBandages);
        }
    }
}
